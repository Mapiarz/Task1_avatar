using UnityEngine;
using DG.Tweening;

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
        Active = true;
        Sequence appear = DOTween.Sequence();
        appear.PrependInterval(1);
        appear.Append(GetComponent<CanvasGroup>().DOFade(1, 1f));
    }

    public void Deactivate()
    {
        Active = false;
        Sequence appear = DOTween.Sequence();
        appear.Append(GetComponent<CanvasGroup>().DOFade(0, 1f));
        appear.OnComplete(() => gameObject.SetActive(false));
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

    void Awake()
    {
        DOTween.Init();
        ReadyToSkip = false;
        Active = true;
        Choice = 0;
    }
}
