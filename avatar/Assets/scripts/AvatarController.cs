using Aisens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField] SensorManagerBehaviour sensorManager;
    [SerializeField] Animator animatorComponent;

    Coroutine discoveryCoroutine;
    /// <summary>
    /// dictionary containing all bones inside avatar
    /// </summary>
    Dictionary<HumanBodyBones, Transform> bonesDictionary;
    /// <summary>
    /// dictionary containing all pones controlled by specific sensors, edited
    /// </summary>
    Dictionary<int, Transform> PortBonesDictionary;

    /// <summary>
    /// finding and assigning all bones to specific transforms inside a dictionary <see cref="bonesDictionary"/> <HumanBodyBones, Transform>
    /// assigning specific bones from global dictionary to sensro dictionary <see cref="PortBonesDictionary"/>
    /// </summary>
    void MapBones()
    {
        Debug.Log("maping bones");

        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
        //collects one by one bones from HumanBodyBones and Avatar, combines them, also if null
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
        //create dictionary of sensor bones
        PortBonesDictionary = new Dictionary<int, Transform>
        {
            { 10000, bonesDictionary[HumanBodyBones.LeftUpperLeg] },
            { 10001, bonesDictionary[HumanBodyBones.LeftLowerLeg] }
        };
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

    /// <summary>
    /// deleting sensors that are not controlling any limb, not allowing to create handles
    /// </summary>
    /// <param name="sensors"></param>
    void SensorClearing(IList<ISensorInfo> sensors)
    {
        MapBones();
        foreach (ISensorInfo sensor in sensors)
        {
            if (!PortBonesDictionary.ContainsKey(sensor.GetHashCode()))
            {
                sensors.Remove(sensor);
            }
        }
    }

    void AssignSensors( IList<ISensorInfo> infos )
    {
        SensorClearing( infos );

        var handles = sensorManager.ConnectToSensors( infos );

        for ( int i = 0; i < handles.Count; i++ )
        {
            if (PortBonesDictionary.ContainsKey(infos[i].GetHashCode()))
            {
                StartCoroutine(RotateBone(handles[i], PortBonesDictionary[infos[i].GetHashCode()]));
            }
        }
    }

    /// <summary>
    /// due to differences in coordinate systems in the program and sensors, it is required to change axes
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    IEnumerator RotateBone( ISensorHandle handle, Transform transform )
    {
        yield return new WaitUntil( () => handle.CanGetDatagram );

        while ( handle.IsConnectionOpen )
        {
            var rotation = handle.GetDatagram().Rotation;
            // Debug.Log( $"Got rotation: {rotation}; (x: {rotation.eulerAngles.x}; y: {rotation.eulerAngles.y}; z: {rotation.eulerAngles.z}" );

            transform.rotation = Quaternion.Euler(-(rotation.eulerAngles.z-90), rotation.eulerAngles.y, rotation.eulerAngles.x);

            yield return null;
        }
    }
}
