using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGeneration : MonoBehaviour
{

}
/*
    Seperate gameobject that in fixed update generates constantly data, changing quaternions by one every frame. 
    It is defined in the avatar to get quaternion DataGenerator.data and apply it to different bodyparts. Need to group bodyparts or select them as int number of joint.
    Application is switched inside of AvatarController. Required methods:fixed update changing quaternions value. 

     */
