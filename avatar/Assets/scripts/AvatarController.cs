using Aisens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvatarController : MonoBehaviour
{
    /// <summary>
    /// sent to UIController
    /// </summary>
    public bool discoveryFinished;
    /// <summary>
    /// sent to UIController
    /// </summary>
    public int numberOfSensors;
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
    Dictionary<int, Transform> portBonesDictionary;
    /// <summary>
    /// containing initial position of the avatar, standing still
    /// </summary>
    Dictionary<Transform, Quaternion> initialDictionary;
    /// <summary>
    /// containing offset of every bone
    /// </summary>
    Dictionary<Transform, Quaternion> calibrationDictionary;
    private bool beginRotations;

    private void Start()
    {
        Assert.IsNotNull(animatorComponent, "Animator not found");
        beginRotations = false;
        MapBones();
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
            if (animatorComponent.GetBoneTransform(bone) != null)
            {
                bonesDictionary.Add(bone, animatorComponent.GetBoneTransform(bone));
            }

            else
            {
                bonesDictionary.Add(bone, null);
            }
        }
        //create dictionary of sensor bones
        portBonesDictionary = new Dictionary<int, Transform>
        {
            { 10000, bonesDictionary[HumanBodyBones.LeftUpperLeg] },
            { 10001, bonesDictionary[HumanBodyBones.LeftLowerLeg] },
            { 10002, bonesDictionary[HumanBodyBones.RightUpperLeg] },
            { 10003, bonesDictionary[HumanBodyBones.RightLowerLeg] },
            { 10004, bonesDictionary[HumanBodyBones.Hips] }
        };
    }

    public void DiscoverServers()
    {
        discoveryFinished = false;

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
                numberOfSensors = result.Count;
                Debug.Log($"avatarController number {numberOfSensors}");
                AssignSensors( result );
            }
            else
            {
                Debug.LogWarning( "Sensor discovery failed" );
            }
        } );
        discoveryFinished = true;
        discoveryCoroutine = null;
    }

    /// <summary>
    /// assigns bones to sensors uses port numbers
    /// </summary>
    /// <param name="infos"></param>
    void AssignSensors( IList<ISensorInfo> infos )
    {
        infos = SensorClearing( infos );

        var handles = sensorManager.ConnectToSensors( infos );

        for (int i = 0; i < handles.Count; i++)
        {
            Assert.IsTrue(portBonesDictionary.ContainsKey(infos[i].GetHashCode()), "Sensor not mapped");
            StartCoroutine(RotateBone(handles[i], portBonesDictionary[infos[i].GetHashCode()]));
        }
    }

    /// <summary>
    /// deleting sensors that are not controlling any limb
    /// </summary>
    /// <param name="sensors"></param>
    IList<ISensorInfo> SensorClearing(IList<ISensorInfo> sensors)
    {
        foreach (ISensorInfo sensor in sensors)
        {
            if (!portBonesDictionary.ContainsKey(sensor.GetHashCode()))
            {
                sensors.Remove(sensor);
            }
        }
        GetInitialTransforms();
        return sensors;
    }

    /// <summary>
    /// <see cref="InitialDictionary"/> 
    /// </summary>
    void GetInitialTransforms()
    {

        initialDictionary = new Dictionary<Transform, Quaternion>
        {
            { bonesDictionary[HumanBodyBones.LeftUpperLeg], bonesDictionary[HumanBodyBones.LeftUpperLeg].rotation },
            { bonesDictionary[HumanBodyBones.LeftLowerLeg], bonesDictionary[HumanBodyBones.LeftLowerLeg].rotation },
            { bonesDictionary[HumanBodyBones.RightUpperLeg], bonesDictionary[HumanBodyBones.RightUpperLeg].rotation },
            { bonesDictionary[HumanBodyBones.RightLowerLeg], bonesDictionary[HumanBodyBones.RightLowerLeg].rotation },
            { bonesDictionary[HumanBodyBones.Hips], bonesDictionary[HumanBodyBones.Hips].rotation }
        };
        Debug.Log(initialDictionary[bonesDictionary[HumanBodyBones.LeftUpperLeg]].eulerAngles);
    }

    /// <summary>
    /// <see cref="CalibrationDictionary"/>
    /// callback from button
    /// </summary>
    public void Calibrate()
    {
        var currentRotation1 = animatorComponent.GetBoneTransform(HumanBodyBones.LeftUpperLeg).rotation;
        var currentRotation2 = animatorComponent.GetBoneTransform(HumanBodyBones.LeftLowerLeg).rotation;
        var currentRotation3 = animatorComponent.GetBoneTransform(HumanBodyBones.RightUpperLeg).rotation;
        var currentRotation4 = animatorComponent.GetBoneTransform(HumanBodyBones.RightLowerLeg).rotation;
        var currentRotation5 = animatorComponent.GetBoneTransform(HumanBodyBones.Hips).rotation;

        var initialRotation1 = initialDictionary[bonesDictionary[HumanBodyBones.LeftUpperLeg]];
        var initialRotation2 = initialDictionary[bonesDictionary[HumanBodyBones.LeftLowerLeg]];
        var initialRotation3 = initialDictionary[bonesDictionary[HumanBodyBones.RightUpperLeg]];
        var initialRotation4 = initialDictionary[bonesDictionary[HumanBodyBones.RightLowerLeg]];
        var initialRotation5 = initialDictionary[bonesDictionary[HumanBodyBones.Hips]];

        var deltaRotation1 = Quaternion.Euler(currentRotation1.eulerAngles - initialRotation1.eulerAngles);
        var deltaRotation2 = Quaternion.Euler(currentRotation2.eulerAngles - initialRotation2.eulerAngles);
        var deltaRotation3 = Quaternion.Euler(currentRotation3.eulerAngles - initialRotation3.eulerAngles);
        var deltaRotation4 = Quaternion.Euler(currentRotation4.eulerAngles - initialRotation4.eulerAngles);
        var deltaRotation5 = Quaternion.Euler(currentRotation5.eulerAngles - initialRotation5.eulerAngles);

        calibrationDictionary = new Dictionary<Transform, Quaternion>
        {
            { bonesDictionary[HumanBodyBones.LeftUpperLeg], deltaRotation1 },
            { bonesDictionary[HumanBodyBones.LeftLowerLeg], deltaRotation2 },          
            { bonesDictionary[HumanBodyBones.RightUpperLeg], deltaRotation3 },
            { bonesDictionary[HumanBodyBones.RightLowerLeg], deltaRotation4 },
            { bonesDictionary[HumanBodyBones.Hips], deltaRotation5 }
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
            if (calibrationDictionary == null && beginRotations)
            {
                //Debug.Log($"Got rotation: {rotation}; (x: {rotation.eulerAngles.x}; y: {rotation.eulerAngles.y}; z: {rotation.eulerAngles.z}");
                transform.rotation = rotation;
            }
            else if (calibrationDictionary != null && beginRotations)
            {
                transform.rotation = Quaternion.Euler(rotation.eulerAngles - calibrationDictionary[transform].eulerAngles);
            }
            yield return null;
        }
    }

    /// <summary>
    /// starts writing sensors data to bones
    /// callback from button
    /// </summary>
    public void BeginRotations()
    {
        beginRotations = true;
    }
}
