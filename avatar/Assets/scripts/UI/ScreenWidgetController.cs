using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ScreenWidgetController : MonoBehaviour
{
    [SerializeField] Image imageComponent;
    [SerializeField] List<BaseScreenWidget> screenWidgets;

    public ExerciseType SelectedExerciseType { get; set; } = ExerciseType.None;
    public int NumberOfSensors { get; set; } = 0;

public void Awake()
    {
        Assert.IsNotNull( imageComponent );
        Assert.IsNotNull( screenWidgets );
        Assert.IsTrue( screenWidgets.Count > 0 );

        var firstWidget = screenWidgets.First();

        StartCoroutine( StartUpCoroutine( firstWidget ) );
    }

    IEnumerator StartUpCoroutine( BaseScreenWidget firstWidget )
    {
        yield return new WaitForSeconds( 1f );

        yield return firstWidget.ShowCoroutine();
    }

    IEnumerator CloseDownCoroutine( BaseScreenWidget lastWidget )
    {
        yield return lastWidget.HideCoroutine();

        var t = imageComponent.DOFade( 0f, 0.5f );
        yield return t.WaitForCompletion();

        gameObject.SetActive( false );
    }

    IEnumerator ScreenTransitionCoroutine( BaseScreenWidget from, BaseScreenWidget to )
    {
        Assert.IsNotNull( from );
        Assert.IsNotNull( to );

        yield return from.HideCoroutine();
        yield return to.ShowCoroutine();
    }

    public void GoToNextScreen( BaseScreenWidget screenWidget )
    {
        Assert.IsNotNull( screenWidget );

        var nextScreen = GetNextScreenWidget( screenWidget );
        if ( nextScreen != null )
        {
            StartCoroutine( ScreenTransitionCoroutine( screenWidget, nextScreen ) );
        }
        else
        {
            StartCoroutine( CloseDownCoroutine( screenWidget ) );
        }
    }

    BaseScreenWidget GetNextScreenWidget( BaseScreenWidget currentScreenWidget )
    {
        Assert.IsNotNull( currentScreenWidget );
        Assert.IsTrue( screenWidgets.Contains( currentScreenWidget ) );

        var indexOf = screenWidgets.IndexOf( currentScreenWidget );

        if ( screenWidgets.Count > indexOf + 1 )
        {
            return screenWidgets[indexOf + 1];
        }
        else
        {
            return null;
        }
    }

    public void GoToPreviousScreen(BaseScreenWidget screenWidget)
    {
        Assert.IsNotNull(screenWidget);

        var previousScreen = GetPreviousScreenWidget(screenWidget);
        if (previousScreen != null)
        {
            StartCoroutine(ScreenTransitionCoroutine(screenWidget, previousScreen));
        }
        else
        {
            StartCoroutine(CloseDownCoroutine(screenWidget));
        }
    }

    BaseScreenWidget GetPreviousScreenWidget(BaseScreenWidget currentScreenWidget)
    {
        Assert.IsNotNull(currentScreenWidget);
        Assert.IsTrue(screenWidgets.Contains(currentScreenWidget));

        var indexOf = screenWidgets.IndexOf(currentScreenWidget);

        return screenWidgets[indexOf - 1];
    }
}
