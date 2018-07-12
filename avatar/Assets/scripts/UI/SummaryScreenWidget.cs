using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenWidget : BaseScreenWidget
{
    [SerializeField] Text text;
    int numberOfSensors;
    ExerciseType exercise;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        numberOfSensors = screenController.NumberOfSensors;
        exercise = screenController.SelectedExerciseType;
        ChangeText();
    }

    void ChangeText()
    {
        int expectedNumberOfSensors;

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

    public void DisagreeAndGoToPreviousScreen()
    {
        GoToPreviousScreen();
    }

}
