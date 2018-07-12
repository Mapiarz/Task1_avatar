using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class BaseScreenWidget : MonoBehaviour
{
    [SerializeField] protected ScreenWidgetController screenController;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] float fadeDuration = 0.5f;

    protected bool IsShowing { get; set; }

    protected virtual void Awake()
    {
        Assert.IsNotNull( screenController );
        Assert.IsNotNull( canvasGroup );

        // Start hidden
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;

        gameObject.SetActive( true );

        IsShowing = false;
    }

    public void Show()
    {
        StartCoroutine( ShowCoroutine() );
    }

    public void Hide()
    {
        StartCoroutine( HideCoroutine() );
    }

    public IEnumerator ShowCoroutine()
    {
        Assert.IsFalse( IsShowing );

        canvasGroup.alpha = 0f;
        var tweener = canvasGroup.DOFade( 1f, fadeDuration );

        yield return tweener.WaitForCompletion();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        IsShowing = true;
    }

    public IEnumerator HideCoroutine()
    {
        Assert.IsTrue( IsShowing );

        canvasGroup.alpha = 1f;
        var tweener = canvasGroup.DOFade( 0f, fadeDuration );

        yield return tweener.WaitForCompletion();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        IsShowing = false;
    }

    protected void GoToNextScreen()
    {
        screenController.GoToNextScreen( this );
    }

    protected void GoToPreviousScreen()
    {
        screenController.GoToPreviousScreen( this );
    }
}
