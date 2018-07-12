using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScreenWidget : BaseScreenWidget
{
    [SerializeField] Text text;
    ExerciseType exercise;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        exercise = screenController.SelectedExerciseType;
        ChangeText();
    }

    void ChangeText()
    {
        text.text = $"Instructions how to use sensors in {exercise} configuration";
    }

    public void AcknowledgeAndGoToNextScreen()
    {
        GoToNextScreen();
    }
}
//Instructions how to use sensors in 