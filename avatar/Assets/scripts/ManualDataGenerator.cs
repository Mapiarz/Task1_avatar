using UnityEngine;

public class ManualDataGenerator : MonoBehaviour, IDataSource
{
    DataFrame data;
    Quaternion eulerQuaternion;
    Vector3 eulerPosition;

    void Start()
    {
        data = new DataFrame();
        data.limb = HumanBodyBones.LeftLowerArm;
        eulerPosition = new Vector3();
        eulerQuaternion = new Quaternion();
    }

    /// <summary>
    /// methods required to manual control of the avatar
    /// </summary>
    public void SliderSetX(float x)
    {
        eulerPosition.x = x;
    }

    public void SliderSetY(float y)
    {
        eulerPosition.y = y;
    }

    public void SliderSetZ(float z)
    {
        eulerPosition.z = z;
    }

    /// <summary>
    /// method that defines which limb is moved by the transform
    /// </summary>
    void SetLimbTo(HumanBodyBones limb_)
    {
        data.limb = limb_;
    }

    public void SetLimbLeftArm()
    {
        SetLimbTo(HumanBodyBones.LeftLowerArm);
    }

    public void SetLimbRightArm()
    {
        SetLimbTo(HumanBodyBones.RightLowerArm);
    }

    public void SetLimbLeftLeg()
    {
        SetLimbTo(HumanBodyBones.LeftUpperLeg);
    }

    public void SetLimbRightLeg()
    {
        SetLimbTo(HumanBodyBones.RightUpperLeg);
    }

    public DataFrame GetData()
    {
        eulerQuaternion.eulerAngles = eulerPosition;
        data.rotation = eulerQuaternion;
        return (data);
    }
}
