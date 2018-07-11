using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHello : UIWidget
{

    void Awake()
    {
        Debug.Log("początek UIHello");
        DOTween.Init();
        ReadyToSkip = false;
        Active = true;
        Choice = 0;
    }
}
