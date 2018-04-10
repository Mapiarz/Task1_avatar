using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualDrive : MonoBehaviour {

    public GameObject button, sliders;
    public bool boolToggle;
    public AvatarController controller;

    private void Awake()
    {
        boolToggle = false;
    }

    public void ChangeState()
    {
        button.SetActive(boolToggle);
        sliders.SetActive(!boolToggle);
        controller.ManualSwitch();
        boolToggle = !boolToggle;
    }
}
