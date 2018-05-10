using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGeneration : MonoBehaviour
{
    float x, y, z, border;
    int limb;
    bool generationSwitch;
    private void Awake()
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
        limb = 5;
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

    public string GetData()
    {
        return (limb.ToString() + '|' + x.ToString() + '|' + y.ToString() + '|' + z.ToString());
    }
    private void Update()
    {
        GenerateData();


    }
}

