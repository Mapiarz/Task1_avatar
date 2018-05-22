using UnityEngine;

// TODO: Rename to AutomaticDataGenerator
public class DataGeneration : MonoBehaviour, IDataSource
{
    Vector3 delta, generatedValue;
    /// <summary>
    /// a specific value of euler angle that is maximal available rotation, changes direction after reaching this limit
    /// </summary>
    float rotationMaximumValue;
    int limb;  // TODO: Use HumanBodyBones instead of an int

    void Awake()
    {
        rotationMaximumValue = 90;
        generatedValue = new Vector3 (0.0f, 0.0f, 0.5f);
        limb = 5;
        delta = new Vector3(0.1f, 0.1f, 0.1f);
    }

    /// <summary>
    /// method that defines which limb is moved by the transform
    /// </summary>
    void SetLimbTo(HumanBodyBones limb_)
    {
        limb = (int)limb_;
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
        delta = -delta;
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

        generatedValue = generatedValue + delta;

    }

    public string GetData()
    {
        // 

        // TODO: Get rid of data generation as a string, create a class (e.g. DataFrame) with quaternion and HumanBodyBones
        //       To generate Quaternion from euler angles use Quaternion.Euler method
        return ( limb.ToString() + '|' + generatedValue.x.ToString() + '|' + generatedValue.y.ToString() + '|' + generatedValue.z.ToString());
    }

    void Update()
    {
        GenerateData();
    }
}

