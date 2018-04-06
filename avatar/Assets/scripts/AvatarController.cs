using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController : MonoBehaviour
{
    public Quaternion q;
    private Animator animatorComponent = null;
    protected Transform[] bones;
    protected Quaternion[] initialRotations;
    protected Quaternion[] localRotations;
    public Slider Sx, Sy, Sz;

    protected virtual void MapBones()
    {
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            if (!boneIndex2MecanimMap.ContainsKey(boneIndex))
                continue;

            bones[boneIndex] = animatorComponent ? animatorComponent.GetBoneTransform(boneIndex2MecanimMap[boneIndex]) : null;
        }
    }
    
    void Start()
    {
        bones = new Transform[30];
        initialRotations = new Quaternion[bones.Length];
        localRotations = new Quaternion[bones.Length];
        GetInitialRotations();
        animatorComponent = GetComponent<Animator>();
        MapBones();
        q = new Quaternion(100, 100, 100, 1);
        for (int i = 0; i < 30; i++)
        {
            Debug.Log(bones[i] + " " + i.ToString()) ;
        }
        Debug.Log(q);
        //bones[5].rotation = Data2AvatarRot(q, 5);

    }
    public void SliderSetX(float x)
    {
        q.x = x;
        //Debug.Log(Sx.value.ToString() + "1");
    }
    public void SliderSetY(float y)
    {
        q.y = y;
    }
    public void SliderSetZ(float z)
    {
        q.z = z;
    }
    void Update()
    {
        bones[5].rotation = q;
    }

    //does nothing??
    protected void GetInitialRotations()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                initialRotations[i] = bones[i].rotation;
                localRotations[i] = bones[i].localRotation;
            }
        }
    }

    protected Quaternion Data2AvatarRot(Quaternion jointRotation, int boneIndex)
    {
        Quaternion newRotation = jointRotation * initialRotations[boneIndex];

        return newRotation;
    }
    /*
    public Quaternion GetJointOrientation(int joint)
    {
     
            int index = dictUserIdToIndex[userId];

            if (index >= 0 && index < sensorData.bodyCount &&
               bodyFrame.bodyData[index].bIsTracked != 0)
            {
                if (flip)
                    return bodyFrame.bodyData[index].joint[joint].normalRotation;
                else
                    return bodyFrame.bodyData[index].joint[joint].mirroredRotation;
            }

        return Quaternion.identity;
    }
    */
    /*
    protected void TransformBone(JointType joint, int boneIndex, bool flip)
    {
        Transform boneTransform = bones[boneIndex];

        int iJoint = (int)joint;

        Quaternion jointRotation = GetJointOrientation(iJoint);
        // calculate the new orientation
        Quaternion newRotation = Data2AvatarRot(jointRotation, boneIndex);
            boneTransform.rotation = newRotation;
    }
    */


    protected static readonly Dictionary<int, HumanBodyBones> boneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
    {
        {0, HumanBodyBones.Hips},
        {1, HumanBodyBones.Spine},
//        {2, HumanBodyBones.Chest},
		{3, HumanBodyBones.Neck},
//		{4, HumanBodyBones.Head},
		
		{5, HumanBodyBones.LeftUpperArm},
        {6, HumanBodyBones.LeftLowerArm},
        {7, HumanBodyBones.LeftHand},

		
		{11, HumanBodyBones.RightUpperArm},
        {12, HumanBodyBones.RightLowerArm},
        {13, HumanBodyBones.RightHand},
//		{14, HumanBodyBones.RightIndexProximal},
//		{15, HumanBodyBones.RightIndexIntermediate},
//		{16, HumanBodyBones.RightThumbProximal},
		
		{17, HumanBodyBones.LeftUpperLeg},
        {18, HumanBodyBones.LeftLowerLeg},
        {19, HumanBodyBones.LeftFoot},
//		{20, HumanBodyBones.LeftToes},
		
		{21, HumanBodyBones.RightUpperLeg},
        {22, HumanBodyBones.RightLowerLeg},
        {23, HumanBodyBones.RightFoot},
//		{24, HumanBodyBones.RightToes},
		
		{25, HumanBodyBones.LeftShoulder},
        {26, HumanBodyBones.RightShoulder},
        {27, HumanBodyBones.LeftIndexProximal},
        {28, HumanBodyBones.RightIndexProximal},
        {29, HumanBodyBones.LeftThumbProximal},
        {30, HumanBodyBones.RightThumbProximal},
    };

}


