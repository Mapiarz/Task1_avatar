using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField]
    Vector3 eulerPosition;
    [SerializeField]
    DataGeneration dataSource;

    protected Transform[] bones;

    Animator animatorComponent = null;
    string[] data;
    int limb;
    public bool manualFlag;

    void Start()
    {
        manualFlag = false;
        bones = new Transform[30];
        animatorComponent = GetComponent<Animator>();
        MapBones();
        for (int i = 0; i < 30; i++)
        {
            Debug.Log(bones[i] + " " + i.ToString());
        }
    }

    void MapBones()
    {
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            if (!BoneIndex2MecanimMap.ContainsKey(boneIndex))
                continue;

            bones[boneIndex] = animatorComponent ? animatorComponent.GetBoneTransform(BoneIndex2MecanimMap[boneIndex]) : null;
        }
    }



    void Update()
    {
        if (!manualFlag)
        {
            ManageData();
        }
        else
            bones[limb].transform.localEulerAngles = eulerPosition;
    }

    void ManageData ()
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

        eulerPosition.x = float.Parse(data[1]);
        eulerPosition.y = float.Parse(data[2]);
        eulerPosition.z = float.Parse(data[3]);
        AssignData();
    }

    bool ChangeOfPosition(int joint)
    {
        Vector3 delta;

        delta = eulerPosition - GetPresentRotation(joint);

        delta.x = Mathf.Abs(delta.x);
        delta.y = Mathf.Abs(delta.y);
        delta.z = Mathf.Abs(delta.z);

        if (delta.x > 0.1 || delta.y > 0.1 || delta.z > 0.1) //visible change
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    void AssignData()
    {
        Debug.Log(ChangeOfPosition(limb));
        if (ChangeOfPosition(limb))
        {
            bones[limb].transform.localEulerAngles = eulerPosition;
        }
    }

    Vector3 GetPresentRotation(int i)
    {
        return bones[i].transform.localEulerAngles;
    }

    public void ManualSwitch()
    {
        manualFlag = !manualFlag;
    }

    public void SliderSetX(float x)
    {
        if (manualFlag)
        {
            eulerPosition.x = x;
        }
    }
    public void SliderSetY(float y)
    {
        if (manualFlag)
        {
            eulerPosition.y = y;
        }
    }
    public void SliderSetZ(float z)
    {
        if (manualFlag)
        {
            eulerPosition.z = z;
        }
    }

    protected static readonly Dictionary<int, HumanBodyBones> BoneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
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


}


