using Aisens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTester : MonoBehaviour
{
    [SerializeField] SensorManagerBehaviour sensorManager;
    [SerializeField] Animator animatorComponent;

    Coroutine discoveryCoroutine;

    Dictionary<HumanBodyBones, Transform> bonesDictionary;
    Dictionary<int, Transform> PortBonesDictionary;

    /// <summary>
    /// finding and assigning all bones to specific transforms inside a dictionary <HumanBodyBones, Transform>
    /// </summary>
    void MapBones()
    {
        Debug.Log("maping bones");
        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
        foreach (var bone in (HumanBodyBones[])System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (animatorComponent != null & animatorComponent.GetBoneTransform(bone) != null)
            {
                bonesDictionary.Add(bone, animatorComponent.GetBoneTransform(bone));
            }

            else
            {
                bonesDictionary.Add(bone, null);
            }
        }
        PortBonesDictionary = new Dictionary<int, Transform>
        {
            { 10000, bonesDictionary[HumanBodyBones.LeftUpperArm] },
            { 10001, bonesDictionary[HumanBodyBones.LeftLowerArm] }
        };
        Debug.Log(PortBonesDictionary[10000]);
    }

    public void CreateServer()
    {
        if ( discoveryCoroutine != null )
        {
            Debug.Log("corutine null");
            return;  // Note: Sensor discovery coroutine doesn't support cancellation so we have to return when multiple attempts are made
        }
        Debug.Log("corutine discoverSensors");
        discoveryCoroutine = StartCoroutine( DiscoverSensors() );
    }

    IEnumerator DiscoverSensors()
    {
        yield return sensorManager.DiscoverSensors( 5000, ( result ) =>
        {
            Debug.Log("discovering");
            if ( result != null )
            {
                Debug.Log( $"Discovered {result.Count} sensors" );
                AssignSensors( result );
            }
            else
            {
                Debug.LogWarning( "Sensor discovery failed" );
            }
        } );

        discoveryCoroutine = null;
    }

    void AssignSensors( IList<ISensorInfo> infos )
    {
        Debug.Log("assigning sensors");
        var handles = sensorManager.ConnectToSensors( infos );

        MapBones();

        for ( int i = 0; i < handles.Count; i++ )
        {
            Debug.Log(handles[i].GetHashCode());
            if (infos[i].GetHashCode() == 10000 || infos[i].GetHashCode() == 10001)
            {
                Debug.Log("one is correct");
                StartCoroutine(RotateBone(handles[i], PortBonesDictionary[infos[i].GetHashCode()]));
            }
        }
    }

    IEnumerator RotateBone( ISensorHandle handle, Transform transform )
    {
        yield return new WaitUntil( () => handle.CanGetDatagram );

        while ( handle.IsConnectionOpen )
        {
            var rotation = handle.GetDatagram().Rotation;
            // Debug.Log( $"Got rotation: {rotation}; (x: {rotation.eulerAngles.x}; y: {rotation.eulerAngles.y}; z: {rotation.eulerAngles.z}" );

            transform.rotation = rotation;

            yield return null;
        }
    }
}
