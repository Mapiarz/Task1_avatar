using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWidget
{

    /// <summary>
    /// ends the widget in the master, access from master
    /// </summary>
    bool ReadyToSkip { get; set; }

    /// <summary>
    /// choosen exercise, access from master
    /// </summary>
    int Choice { get; }

    void Activate();

    void Deactivate();
}
