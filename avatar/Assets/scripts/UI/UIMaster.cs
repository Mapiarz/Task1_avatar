using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UIMaster : MonoBehaviour {

    [SerializeField] IWidget[] widgets;
    [SerializeField] UIHello uiHello;
    [SerializeField] UIChoice uiChoice;
    [SerializeField] UIDetect uiDetect;
    [SerializeField] GameObject quad;
    int selectedExercise;

    CanvasGroup alteredPanel;
    public void Awake()
    {
        widgets = new IWidget[] { uiHello, uiChoice, uiDetect };
        StartCoroutine(ActivateWidget());
    }

    /// <summary>
    /// coroutine responsible for activation and deactivation of screens
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateWidget()
    {
        bool uiEnd = false;
        int currentWidget = 0;
        Debug.Log("korutyna");
        while (!uiEnd)
        {
            if (widgets[currentWidget].ReadyToSkip)
            {
                if (widgets[currentWidget].Choice != 0)
                {
                    selectedExercise = widgets[currentWidget].Choice;
                }

                widgets[currentWidget].Deactivate();
                currentWidget++;
                Debug.Log(widgets.Length - 1);
            }

            if (widgets[widgets.Length - 1].ReadyToSkip)
            {
                Debug.Log("tak");
                uiEnd = true;
                Sequence appear = DOTween.Sequence();
                appear.PrependInterval(1);
                appear.Append(quad.GetComponent<Renderer>().material.DOFade(0, 1f));
            }

            else
            {
                if (!widgets[currentWidget].Active)
                {
                    widgets[currentWidget].Activate();
                }

            }

            yield return null;
        }
    }


}
