using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField]
    Vector3 eulerPosition;
    [SerializeField]
    AutomaticDataGenerator dataSource;
    // [SerializeField] ... manualDataSource

    // TODO: Task after cleaning code up: Extract slider code into a separate data generation class (ManualUserDataGenerator)
    DataFrame data;
    IDataSource currentDataSource;
    Animator animatorComponent;
    Dictionary<HumanBodyBones, Transform> bonesDictionary;
    HumanBodyBones limb;
    bool manualFlag;
    //Transform[] bones;
    void Start()
    {
        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
        manualFlag = false;
        //bones = new Transform[30];
        animatorComponent = GetComponent<Animator>();
        MapBones();  
        for (HumanBodyBones i = 0; (int)i < 30; i++)
        {
            Debug.Log(bonesDictionary[i]);
        }
    }

    /// <summary>
    /// finding and assigning all bones to specific transforms inside a dictionary <HumanBodyBones, Transform>
    /// </summary>
    void MapBones()
    {
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
        Debug.Log("Maping Done");
    }

   
    void Update()
    {
        if ( !manualFlag )
        {
           ManageData();
        }
        else
        {
            bonesDictionary[limb].transform.localEulerAngles = eulerPosition;
        }
    }

    /// <summary>
    /// aquiring data from a dataSource, then converting data and assigning them to transforms
    /// </summary>
    void ManageData ()
    {
        // TODO: SImplify to getting and assigning data
        data = currentDataSource.GetData();
        AssignData();
    }

    /// <summary>
    /// assiging data to specific bones
    /// </summary>
    void AssignData() 
    {
        bonesDictionary[data.limb].rotation = data.rotation;

    }



    /// <summary>
    /// returns present position of specific joint
    /// </summary>
    Vector3 GetPresentRotation(HumanBodyBones limb)
    {
        return bonesDictionary[limb].transform.localEulerAngles;
    }

    /// <summary>
    /// turns on and off manual drive, activated in the interface by a user
    /// </summary>
    public void ManualSwitch()
    {
        manualFlag = !manualFlag;
    }

    /// <summary>
    /// methods required to manual control of the avatar
    /// </summary>
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
}

/*    // TODO: This optimalization is unneeded at this moment - remove it
    /// <summary>
    /// Returns true if new rotation for a joint is considered 'visible' - bigger than some arbitrary delta threshold, otherwise false
    /// </summary>
    bool ChangeOfPosition(HumanBodyBones limb)
    {
        //method compares current position of every joint with its next position from data in vector3 eulerPosition. If differrence is greater than 0.1 (considered visible) then returns true and assigns data, else false and does nothing
        Vector3 delta;

        delta = eulerPosition - GetPresentRotation(limb);

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
    */
