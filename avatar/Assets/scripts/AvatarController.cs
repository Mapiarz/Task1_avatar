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
    /// dictionary containing all bones controlled by specific sensors, edited in code, controls other dictionaries <see cref="availableSensors"/>
    /// </summary>
    Dictionary<int, Transform> PortBonesDictionary;
    /// <summary>
    /// containing initial position of the avatar, standing still
    /// </summary>
    Dictionary<Transform, Quaternion> InitialDictionary;
    /// <summary>
    /// containing offset of every bone
    /// </summary>
    Dictionary<Transform, Quaternion> CalibrationDictionary;

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
    /// assigns bones to sensors uses port numbers
    /// </summary>
    /// <param name="infos"></param>
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
    /// deleting sensors that are not controlling any limb
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
        GetInitialTransforms();
    }

    /// <summary>
    /// finding all bones in the avatar, assigning bones to ports
    /// </summary>
    void MapBones()
    {
        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
        //collects one by one bones from HumanBodyBones and Avatar, combines them, even if null, one global dictionary allows to create smaller
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

    /// <summary>
    /// <see cref="InitialDictionary"/> 
    /// </summary>
    void GetInitialTransforms()
    {

        InitialDictionary = new Dictionary<Transform, Quaternion>
        {
            { bonesDictionary[HumanBodyBones.LeftUpperLeg], bonesDictionary[HumanBodyBones.LeftUpperLeg].rotation },
            { bonesDictionary[HumanBodyBones.LeftLowerLeg], bonesDictionary[HumanBodyBones.LeftLowerLeg].rotation }
        };
        Debug.Log(InitialDictionary[bonesDictionary[HumanBodyBones.LeftUpperLeg]].eulerAngles);
    }

    /// <summary>
    /// <see cref="CalibrationDictionary"/>
    /// callback from button
    /// </summary>
    public void Calibre()
    {
        var quatDelta1 = Quaternion.Euler(animatorComponent.GetBoneTransform(HumanBodyBones.LeftUpperLeg).rotation.eulerAngles - InitialDictionary[bonesDictionary[HumanBodyBones.LeftUpperLeg]].eulerAngles);
        var quatDelta2 = Quaternion.Euler(animatorComponent.GetBoneTransform(HumanBodyBones.LeftLowerLeg).rotation.eulerAngles - InitialDictionary[bonesDictionary[HumanBodyBones.LeftLowerLeg]].eulerAngles);
        CalibrationDictionary = new Dictionary<Transform, Quaternion>
        {
            { bonesDictionary[HumanBodyBones.LeftUpperLeg], quatDelta1 },
            { bonesDictionary[HumanBodyBones.LeftLowerLeg], quatDelta2 }
        };

    }

    /// <summary>
    /// applies rotation
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
            if (CalibrationDictionary == null)
            {
                //Debug.Log($"Got rotation: {rotation}; (x: {rotation.eulerAngles.x}; y: {rotation.eulerAngles.y}; z: {rotation.eulerAngles.z}");
                transform.rotation = rotation;
            }
            else
            {
                transform.rotation = Quaternion.Euler(rotation.eulerAngles - CalibrationDictionary[transform].eulerAngles);
            }
            yield return null;
        }
    }
}
