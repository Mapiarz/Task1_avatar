using UnityEngine;

public class ManualDataGenerator : MonoBehaviour, IDataSource
{
    DataFrame data;
    Vector3 eulerPosition;

    void Start()
    {
        data = new DataFrame();
        data.Limb = HumanBodyBones.LeftLowerArm;
        eulerPosition = new Vector3();
    }

    /// <summary>
    /// methods required to manual control of the avatar
    /// </summary>
    public void SliderSetX(float x)
    {
        eulerPosition.x = x;
        data.Rotation = Quaternion.Euler(eulerPosition);
    }

    public void SliderSetY(float y)
    {
        eulerPosition.y = y;
        data.Rotation = Quaternion.Euler(eulerPosition);
    }

    public void SliderSetZ(float z)
    {
        eulerPosition.z = z;
        data.Rotation = Quaternion.Euler(eulerPosition);
    }

    /// <summary>
    /// method that defines which limb is moved by the transform
    /// </summary>
    void SetLimbTo(HumanBodyBones limb)
    {
        data.Limb = limb;
    }

    /// <summary>
    /// public methods due to UI button callback
    /// </summary>
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
        return data;
    }
}