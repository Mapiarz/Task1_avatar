using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHello : MonoBehaviour, IWidget {

    public int Choice { get; set; }

    public bool Active { get; set; }
    /// <summary>
    /// bool read by the master, ends the widget in the master
    /// </summary>
    public bool ReadyToSkip { get; set; }

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
        Sequence appear = DOTween.Sequence();
        appear.PrependInterval(1);
        appear.Append(GetComponent<CanvasGroup>().DOFade(1, 1f));
    }

    public void Deactivate()
    {
        //gameObject.SetActive(false);
        Active = false;
        Sequence appear = DOTween.Sequence();
        appear.Append(GetComponent<CanvasGroup>().DOFade(0, 1f));
        appear.OnComplete(() => gameObject.SetActive(false));
    }

    void Awake()
    {
        DOTween.Init();
        ReadyToSkip = false;
        Active = true;
        Choice = 0;
    }
}
