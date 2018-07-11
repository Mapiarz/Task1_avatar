using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChoice : MonoBehaviour {

    /// <summary>
    /// ends the widget in the master, access from master
    /// </summary>
    public bool ReadyToSkip;

    /// <summary>
    /// choosen exercise, access from master
    /// </summary>
    public int choice;

    /// <summary>
    /// callback from button
    /// </summary>
    public void SkipOnClick()
    {
        ReadyToSkip = true;
    }

    /// <summary>
    /// return to beginning
    /// </summary>
    public void GoBack()
    {
        ReadyToSkip = false;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// selects exercise number 1, callback from button
    /// </summary>
    public void GetChoice1()
    {
        choice = 1;
    }

    /// <summary>
    /// selects exercise number 2, callback from button
    /// </summary>
    public void GetChoice2()
    {
        choice = 2;
    }
}
