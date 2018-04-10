using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualDrive : MonoBehaviour {

    public GameObject Button, Sliders;


    public void ChangeState()
    {
        if (Button.activeSelf)
        {
            Button.SetActive(false);
        }
        if (!Button.activeSelf)
        {
            Button.SetActive(true);
        }
        if (Sliders.activeSelf)
        {
            Sliders.SetActive(false);
        }
        if (!Sliders.activeSelf)
        {
            Sliders.SetActive(true);
        }
    }
}
