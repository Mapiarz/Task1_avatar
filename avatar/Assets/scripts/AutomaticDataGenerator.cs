using UnityEngine;

public class AutomaticDataGenerator : MonoBehaviour, IDataSource
{
    Quaternion delta;
    /// <summary>
    /// a specific value of euler angle that is maximal available rotation, changes direction after reaching this limit
    /// </summary>
    float rotationMaximumValue;
    DataFrame data;

    void Start()
    {
        data = new DataFrame();
        data.Limb = HumanBodyBones.LeftLowerArm;
        rotationMaximumValue = 80;
        delta = Quaternion.Euler(new Vector3(0.5f, 0.5f, 0.5f));
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
        SetLimbTo( HumanBodyBones.LeftLowerArm );
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

    /// <summary>
    /// changes direction of rotation, used in class and in UI callback
    /// </summary>
    public void ChangeDirection()
    {
        delta.eulerAngles = -delta.eulerAngles;
    }

    /// <summary>
    /// method responsible for generating data
    /// <see cref="rotationMaximumValue"/> 
    /// </summary>
    void GenerateData()
    {
        if (data.Rotation.eulerAngles.x >= rotationMaximumValue || data.Rotation.eulerAngles.x <= -rotationMaximumValue)
        {
            ChangeDirection();
        }

        data.Rotation = Quaternion.Euler(data.Rotation.eulerAngles + delta.eulerAngles);
    }

    public DataFrame GetData()
    {
        return data;
    }

    void Update()
    {
        GenerateData();
    }
}

