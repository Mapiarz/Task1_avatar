using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    private Animator animatorComponent = null;
    protected Transform[] bones;



    public Vector3 GetJointWorldPos(JointType jointType)
    {
        Vector3 jointPosition = DataSource.GetJointPosition(playerId, (int)jointType);
        Vector3 worldPosition = new Vector3(
            jointPosition.x - offsetPos.x,
            //            jointPosition.y - offsetPos.y + kinectManager.sensorHeight,  //!! this should be better investigated .. 
            jointPosition.y + offsetPos.y - kinectManager.sensorHeight,  //!! this workds better on my example 
            !mirroredMovement && !posRelativeToCamera ? (-jointPosition.z - offsetPos.z) : (jointPosition.z - offsetPos.z));

        Quaternion posRotation = mirroredMovement ? Quaternion.Euler(0f, 180f, 0f) * initialRotation : initialRotation;
        worldPosition = posRotation * worldPosition;

        return bodyRootPosition + worldPosition;
    }

}

public enum JointType : int //source kinectInterop
{
    SpineBase = 0,
    SpineMid = 1,
    Neck = 2,
    Head = 3,
    ShoulderLeft = 4,
    ElbowLeft = 5,
    WristLeft = 6,
    HandLeft = 7,
    ShoulderRight = 8,
    ElbowRight = 9,
    WristRight = 10,
    HandRight = 11,
    HipLeft = 12,
    KneeLeft = 13,
    AnkleLeft = 14,
    FootLeft = 15,
    HipRight = 16,
    KneeRight = 17,
    AnkleRight = 18,
    FootRight = 19,
    SpineShoulder = 20,
    HandTipLeft = 21,
    ThumbLeft = 22,
    HandTipRight = 23,
    ThumbRight = 24
    //Count = 25
}
protected static readonly Dictionary<KinectInterop.JointType, int> jointMap2boneIndex = new Dictionary<KinectInterop.JointType, int>
    {
        {KinectInterop.JointType.SpineBase, 0},
        {KinectInterop.JointType.SpineMid, 1},
        {KinectInterop.JointType.SpineShoulder, 2},
        {KinectInterop.JointType.Neck, 3},
        {KinectInterop.JointType.Head, 4},

        {KinectInterop.JointType.ShoulderLeft, 5},
        {KinectInterop.JointType.ElbowLeft, 6},
        {KinectInterop.JointType.WristLeft, 7},
        {KinectInterop.JointType.HandLeft, 8},

        {KinectInterop.JointType.HandTipLeft, 9},
        {KinectInterop.JointType.ThumbLeft, 10},

        {KinectInterop.JointType.ShoulderRight, 11},
        {KinectInterop.JointType.ElbowRight, 12},
        {KinectInterop.JointType.WristRight, 13},
        {KinectInterop.JointType.HandRight, 14},

        {KinectInterop.JointType.HandTipRight, 15},
        {KinectInterop.JointType.ThumbRight, 16},

        {KinectInterop.JointType.HipLeft, 17},
        {KinectInterop.JointType.KneeLeft, 18},
        {KinectInterop.JointType.AnkleLeft, 19},
        {KinectInterop.JointType.FootLeft, 20},

        {KinectInterop.JointType.HipRight, 21},
        {KinectInterop.JointType.KneeRight, 22},
        {KinectInterop.JointType.AnkleRight, 23},
        {KinectInterop.JointType.FootRight, 24},
    };