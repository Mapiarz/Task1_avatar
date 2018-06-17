using UnityEngine;

/// <summary>
/// class of data recieved by AvatarController
/// </summary>
public class DataFrame
{
    public HumanBodyBones Limb { get; set; }
    public Quaternion Rotation { get; set; }
}

