using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScreenWidget : BaseScreenWidget
{
    [SerializeField] Text text;
    ExerciseType exercise;

    public override IEnumerator ShowCoroutine()
    {
        exercise = screenController.SelectedExerciseType;
        ChangeText();
        yield return StartCoroutine(base.ShowCoroutine());
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