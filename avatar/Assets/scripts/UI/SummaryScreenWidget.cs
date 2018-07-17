using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Aisens;

public class SummaryScreenWidget : BaseScreenWidget
{
    [SerializeField] Text text;
    [SerializeField] AvatarController avatarController;
    int numberOfSensors;
    ExerciseType exercise;

    public override IEnumerator ShowCoroutine()
    {
        numberOfSensors = screenController.infos.Count;
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

    /// <summary>
    /// assigns sensors list to avatar controller
    /// </summary>
    public void AcknowledgeAndGoToNextScreen()
    {
        GoToNextScreen();
        avatarController.infos = screenController.infos;
        avatarController.AssignSensors();
    }

    public void DisagreeAndGoToPreviousScreen()
    {
        GoToPreviousScreen();
    }

}
