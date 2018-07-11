using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChoice : MonoBehaviour, IWidget
{

    /// <summary>
    /// ends the widget in the master, access from master
    /// </summary>
    public bool ReadyToSkip { get; set; }

    /// <summary>
    /// choosen exercise, access from master
    /// </summary>
    public int Choice { get; set; }

    /// <summary>
    /// callback from button
    /// </summary>
    public void SkipOnClick()
    {
        ReadyToSkip = true;
    }

    public bool Active { get; set; }

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
        Choice = 1;
    }

    /// <summary>
    /// selects exercise number 2, callback from button
    /// </summary>
    public void GetChoice2()
    {
        Choice = 2;
    }
}
