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
    HumanBodyBones limb;
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

    void MapBones() //assigning GameObjects to a vector in a planned way
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
            bones[(int)limb].transform.localEulerAngles = eulerPosition;
    }

    void ManageData () //aquiring data from a dataSource, converting data
    {
        data = dataSource.GetData().Split('|');

        limb = (HumanBodyBones)int.Parse(data[0]);

        eulerPosition.x = float.Parse(data[1]);
        eulerPosition.y = float.Parse(data[2]);
        eulerPosition.z = float.Parse(data[3]);
        AssignData();
    }

    void AssignData() //assiging data to specific bones
    {
        Debug.Log(ChangeOfPosition((int)limb));
        if (ChangeOfPosition((int)limb))
        {
            bones[(int)limb].transform.localEulerAngles = eulerPosition;
        }
    }

    bool ChangeOfPosition(int joint) // checking if there is visible change of position
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



    Vector3 GetPresentRotation(int i) //reading present position
    {
        return bones[i].transform.localEulerAngles;
    }

    public void ManualSwitch() //methods required to manual control of the avatar
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


