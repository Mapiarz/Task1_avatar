using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController : MonoBehaviour
{
    public Vector3 q;
    private Animator animatorComponent = null;
    protected Transform[] bones;
    //protected Quaternion[] initialRotations;
    public DataGeneration dataSource;
    string[] data;
    int limb;
    private void MapBones()
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
        animatorComponent = GetComponent<Animator>();
        MapBones();
        for (int i = 0; i < 30; i++)
        {
            Debug.Log(bones[i] + " " + i.ToString()) ;
        }
    }

    void Update()
    {
        ManageData();
    }

    private void ManageData ()
    {
        data = dataSource.GetData().Split('|');
        
        switch (data[0])
        {
            case "1":
                limb = 5;
                break;
            case "2":
                limb = 11;
                break;
            case "3":
                limb = 17;
                break;
            case "4":
                limb = 21;
                break;
            default:
                Debug.Log("incorrect value of limb");
                break;
        }

        q.x = float.Parse(data[1]);
        q.y = float.Parse(data[2]);
        q.z = float.Parse(data[3]);
        Debug.Log(q);
        AssignData();
    }

    private bool ChangeOfState(int joint)
    {
        float deltaX, deltaY, deltaZ;

        deltaX = q.x - PreviousRotation(joint).x;

        if (deltaX < 0f)
        {
            deltaX = -deltaX;
        }

        deltaY = q.y - PreviousRotation(joint).y;

        if (deltaY < 0f)
        {
            deltaY = -deltaY;
        }

        deltaZ = q.z - PreviousRotation(joint).z;

        if (deltaZ < 0f)
        {
            deltaZ = -deltaZ;
        }

        if (deltaX > 0.1 || deltaY > 0.1 || deltaZ > 0.1)//visible change
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void AssignData()
    {
        Debug.Log(ChangeOfState(limb));
        if (ChangeOfState(limb))
        {

            bones[limb].transform.localEulerAngles = q;
        }
    }

    private Vector3 PreviousRotation(int i)
    {
        return bones[i].transform.localEulerAngles;

    }

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
		
//		{25, HumanBodyBones.LeftShoulder},
//        {26, HumanBodyBones.RightShoulder},
//        {27, HumanBodyBones.LeftIndexProximal},
//       {28, HumanBodyBones.RightIndexProximal},
//        {29, HumanBodyBones.LeftThumbProximal},
//        {30, HumanBodyBones.RightThumbProximal},
    };

    public void SliderSetX(float x)
    {
        q.x = x;
    }
    public void SliderSetY(float y)
    {
        q.y = y;
    }
    public void SliderSetZ(float z)
    {
        q.z = z;
    }
}


