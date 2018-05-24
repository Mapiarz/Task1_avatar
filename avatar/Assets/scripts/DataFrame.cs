using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class of data recieved by AvatarController
/// </summary>
public class DataFrame
{
    public HumanBodyBones limb { get; set; }
    public Quaternion rotation { get; set; }
}

