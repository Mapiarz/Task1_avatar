using UnityEngine;

public class AutomaticDataGenerator : MonoBehaviour, IDataSource
{
    Quaternion delta, generatedValue;
    /// <summary>
    /// a specific value of euler angle that is maximal available rotation, changes direction after reaching this limit
    /// </summary>
    float rotationMaximumValue;
    DataFrame data;

    void Start()
    {
        data = new DataFrame();
        data.limb = HumanBodyBones.LeftLowerArm;
        rotationMaximumValue = 90;
        generatedValue.eulerAngles = new Vector3 (0.0f, 0.0f, 0.5f);
        delta.eulerAngles = new Vector3(0.1f, 0.1f, 0.1f);
    }

    /// <summary>
    /// method that defines which limb is moved by the transform
    /// </summary>
    void SetLimbTo(HumanBodyBones limb)
    {
        data.limb = limb;
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
    /// changes direction of rotation
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
        if (generatedValue.x >= rotationMaximumValue || generatedValue.x <= -rotationMaximumValue)
        {
            ChangeDirection();
        }

        generatedValue.eulerAngles = generatedValue.eulerAngles + delta.eulerAngles;
        data.rotation = generatedValue;
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

