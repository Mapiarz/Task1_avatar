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

    void Start()
    {
        data = new DataFrame();
        currentDataSource = generatedDataSource;
        bonesDictionary = new Dictionary<HumanBodyBones, Transform>();
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
    }

    void Update()
    {
        AssignData();
    }

    /// <summary>
    /// assiging data to specific bones
    /// </summary>
    void AssignData() 
    {
        bonesDictionary[currentDataSource.GetData().limb].rotation = currentDataSource.GetData().rotation;
    }

    /// <summary>
    /// changes the source of data, activated in the interface by a user
    /// </summary>
    public void ChangeDataSource()
    {
        if ((object)currentDataSource == generatedDataSource)
        {
            currentDataSource = manualDataSource;
        }

        else
        {
            currentDataSource = generatedDataSource;
        }
    }
}
