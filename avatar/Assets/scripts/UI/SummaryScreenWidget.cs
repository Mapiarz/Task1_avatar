using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenWidget : BaseScreenWidget
{
    [SerializeField] Text text;
    int numberOfSensors;
    ExerciseType exercise;

    public override IEnumerator ShowCoroutine()
    {
        numberOfSensors = screenController.NumberOfSensors;
        exercise = screenController.SelectedExerciseType;
        ChangeText();
        yield return StartCoroutine( base.ShowCoroutine() );
    }

    void ChangeText()
    {
        int expectedNumberOfSensors;
        //switch function defines the number of sensors to a single exercise
        switch ((int)exercise)
        {
            case 1:
                expectedNumberOfSensors = 5;
                break;
            case 2:
                expectedNumberOfSensors = 2;
                break;
            default:
                expectedNumberOfSensors = 0;
                break;
        }
        text.text = $"Connected to {numberOfSensors} Sensors. \n Expected {expectedNumberOfSensors} sensors. \n Continue?";
    }

    public void AcknowledgeAndGoToNextScreen()
    {
        GoToNextScreen();
    }

    /// <summary>
    /// allows to return to sensor discovery
    /// </summary>
    public void DisagreeAndGoToPreviousScreen()
    {
        GoToPreviousScreen();
    }

}
