using System.Collections;
using UnityEngine;


public class UIMaster : MonoBehaviour {

    [SerializeField] IWidget[] widgets;
    [SerializeField] UIHello uiHello;
    [SerializeField] UIChoice uiChoice;
    int selectedExercise;

    CanvasGroup alteredPanel;
    public void Awake()
    {
        widgets = new IWidget[] { uiHello, uiChoice };
        StartCoroutine(ActivateWidget());
    }

    /// <summary>
    /// coroutine responsible for activation and deactivation of screens
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateWidget()
    {
        bool uiEnd = true;
        int currentWidget = 0;

        while (uiEnd)
        {
            if (widgets[currentWidget].ReadyToSkip)
            {
                if (widgets[currentWidget].Choice != 0)
                {
                    selectedExercise = widgets[currentWidget].Choice;
                }
                widgets[currentWidget].Deactivate();
                currentWidget++;
            }

            if (widgets[widgets.Length - 1].ReadyToSkip)
            {
                uiEnd = false;
            }

            else
            {
                widgets[currentWidget].Activate();
            }

            yield return null;
        }
    }


}
