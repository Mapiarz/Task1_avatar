using UnityEngine;

// TODO: Rename to AutomaticDataGenerator
public class AutomaticDataGenerator : MonoBehaviour, IDataSource
{
    Quaternion delta, generatedValue;
    /// <summary>
    /// a specific value of euler angle that is maximal available rotation, changes direction after reaching this limit
    /// </summary>
    float rotationMaximumValue;
    HumanBodyBones limb;
    DataFrame data;
    void Awake()
    {
        limb = HumanBodyBones.LeftLowerArm;
        rotationMaximumValue = 90;

        generatedValue.eulerAngles = new Vector3 (0.0f, 0.0f, 0.5f);
        delta.eulerAngles = new Vector3(0.1f, 0.1f, 0.1f);
        data = new DataFrame();
        
    }

    /// <summary>
    /// method that defines which limb is moved by the transform
    /// </summary>
    void SetLimbTo(HumanBodyBones limb_)
    {
        limb = limb_;
    }

    public void SetLimbLeftArm()
    {
        SetLimbTo( HumanBodyBones.LeftLowerArm );
    }

    public void SetLimbRightArm()
    {
        SetLimbTo(HumanBodyBones.RightUpperArm);
    }

    public void SetLimbLeftLeg()
    {
        SetLimbTo(HumanBodyBones.LeftLowerLeg);
    }

    public void SetLimbRightLeg()
    {
        SetLimbTo(HumanBodyBones.LeftUpperLeg);
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
        if (generatedValue.x >= rotationMaximumValue)
        {
            ChangeDirection();
        }

        if (generatedValue.x <= -rotationMaximumValue)
        {
            ChangeDirection();
        }

        generatedValue.eulerAngles = generatedValue.eulerAngles + delta.eulerAngles;
        data.rotation = generatedValue;
        data.limb = limb;
    }

    public DataFrame GetData()
    {
        return (data);
    }

    void Update()
    {
        GenerateData();
    }
}

