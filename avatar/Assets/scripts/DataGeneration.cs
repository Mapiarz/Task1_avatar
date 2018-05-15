using UnityEngine;

// TODO: Rename to AutomaticDataGenerator
public class DataGeneration : MonoBehaviour, IDataSource
{
    float x, y, z;
    float border; // TODO: Rename border to something more clear, add a comment
    int limb;  // TODO: Use HumanBodyBones instead of an int
    bool generationSwitch;  // TODO: Clarify what this is, rename as needed, add comments as needed

    void Awake()
    {
        border = 90;
        x = 0.0f;
        y = 0.5f;
        z = 0.5f;
        generationSwitch = false;
        limb = 5;
    }

    public void SetLimbLeftArm()
    {
        SetLimbTo( HumanBodyBones.LeftLowerArm );
    }

    public void SetLimbRightArm()
    {
        limb = 11;
    }

    public void SetLimbLeftLeg()
    {
        limb = 17;
    }

    public void SetLimbRightLeg()
    {
        limb = 21;
    }

    void SetLimbTo( HumanBodyBones limb )
    {
        limb = (int)limb;
    }

    public void ChangeDirection()
    {
        generationSwitch = !generationSwitch;
    }
    void GenerateData()
    {
        if (x >= border)
        {
            generationSwitch = true;
        }
        if (x <= -border)
        {
            generationSwitch = false;
        }
        if (!generationSwitch)
        {
            x = x + 0.1f;
            y = y + 0.1f;
            z = z + 0.1f;
        }
        if (generationSwitch)
        {
            x = x - 0.1f;
            y = y - 0.1f;
            z = z - 0.1f;
        }
    }

    /// <summary>
    /// TODO <see cref="generationSwitch"/> bla bla
    /// </summary>
    /// <returns></returns>
    public string GetData()
    {
        // 

        // TODO: Get rid of data generation as a string, create a class (e.g. DataFrame) with quaternion and HumanBodyBones
        //       To generate Quaternion from euler angles use Quaternion.Euler method
        return ( limb.ToString() + '|' + x.ToString() + '|' + y.ToString() + '|' + z.ToString());
    }

    void Update()
    {
        GenerateData();
    }
}

