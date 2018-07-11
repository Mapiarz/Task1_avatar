using UnityEngine;
using DG.Tweening;

public class UIWidget : MonoBehaviour, IWidget
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
        Debug.Log("actiivate z UIWIdget");
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        Active = true;
        Sequence appear = DOTween.Sequence();
        appear.PrependInterval(1);
        appear.Append(GetComponent<CanvasGroup>().DOFade(1, 1f));
    }

    public void Deactivate()
    {
        Debug.Log("deactiivate z UIWIdget");
        Active = false;
        Sequence appear = DOTween.Sequence();
        appear.Append(GetComponent<CanvasGroup>().DOFade(0, 1f));
        appear.OnComplete(() => gameObject.SetActive(false));
    }
}
