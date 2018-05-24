using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField]
    Vector3 eulerPosition;
    [SerializeField]
    AutomaticDataGenerator generatedDataSource;
    [SerializeField]
    ManualDataGenerator manualDataSource;

    DataFrame data;
    IDataSource currentDataSource;
    Animator animatorComponent;
    Dictionary<HumanBodyBones, Transform> bonesDictionary;
    bool changeFlag;

    void Start()
    {
        data = new DataFrame();
        currentDataSource = generatedDataSource;
        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
        changeFlag = false;
        animatorComponent = GetComponent<Animator>();
        MapBones();  
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
        ManageData();
    }

    /// <summary>
    /// aquiring data from a dataSource, then converting data and assigning them to transforms
    /// </summary>
    void ManageData ()
    {
        data = currentDataSource.GetData();
        AssignData();
    }

    /// <summary>
    /// assiging data to specific bones
    /// </summary>
    void AssignData() 
    {
        bonesDictionary[data.limb].rotation = data.rotation;
        Debug.Log(data.limb);
    }

    /// <summary>
    /// changes the source of data, activated in the interface by a user
    /// </summary>
    public void ChangeDataSource()
    {
        changeFlag = !changeFlag;
        if (changeFlag)
        {
            currentDataSource = manualDataSource;
        }

        if (!changeFlag)
        {
            currentDataSource = generatedDataSource;
        }
    }
}
