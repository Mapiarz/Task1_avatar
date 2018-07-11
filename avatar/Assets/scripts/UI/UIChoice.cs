using UnityEngine;
using DG.Tweening;

public class UIChoice : UIWidget
{

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

    void Awake()
    {
        DOTween.Init();
        ReadyToSkip = false;
        Active = true;
        Choice = 0;
    }
}
