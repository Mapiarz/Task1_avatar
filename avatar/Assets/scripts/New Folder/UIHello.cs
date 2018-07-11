using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    bool Active { get; set; }
    /// <summary>
    /// bool read by the master, ends the widget in the master
    /// </summary>
    bool ReadyToSkip { get; set; }

    /// <summary>
    /// callback from button
    /// </summary>
    public void SkipOnClick()
    {
        ReadyToSkip = true;
    }

    /// <summary>
    /// return to beginngin
    /// </summary>
    public void GoBack()
    {
        ReadyToSkip = false;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        Active = true;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Active = false;
    }

    /// <summary>
    /// set at start
    /// </summary>
    void Awake()
    {
        ReadyToSkip = false;
    }
}
