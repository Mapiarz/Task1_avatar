﻿using System.Collections.Generic;
using UnityEngine;

public class OldAvatarController : MonoBehaviour
{
    [SerializeField]
    AutomaticDataGenerator generatedDataSource;
    [SerializeField]
    ManualDataGenerator manualDataSource;

    IDataSource currentDataSource;
    Animator animatorComponent;
    Dictionary<HumanBodyBones, Transform> bonesDictionary;
    Dictionary<int, Transform> PortBonesDictionary;

    void Start()
    {
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

        PortBonesDictionary = new Dictionary<int, Transform>
        {
            { 10000, bonesDictionary[HumanBodyBones.LeftUpperArm] },
            { 10001, bonesDictionary[HumanBodyBones.LeftLowerArm] }
        };
    }

    void Update()
    {
        AssignData();
    }

    /// <summary>
    /// assiging data to specific bones, got from Source.GetData()
    /// <see cref="IDataSource"/>
    /// </summary>
    void AssignData() 
    {
        bonesDictionary[currentDataSource.GetData().Limb].rotation = currentDataSource.GetData().Rotation;
    }

    /// <summary>
    /// changes the source of data, activated in the interface by a user
    /// </summary>
    public void ChangeDataSource()
    {
        if ((Object)currentDataSource == generatedDataSource)
        {
            currentDataSource = manualDataSource;
        }

        else
        {
            currentDataSource = generatedDataSource;
        }
    }
}
