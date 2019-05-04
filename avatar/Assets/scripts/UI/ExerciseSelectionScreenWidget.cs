using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ExerciseSelectionScreenWidget : BaseScreenWidget
{
    [SerializeField] Button goToNextSlideButton;

    ExerciseType selectedExercise = ExerciseType.None;

    protected override void Awake()
    {
        base.Awake();

        goToNextSlideButton.interactable = false;    
    }

    public void SelectSquatExercise()
    {
        selectedExercise = ExerciseType.Squat;
        goToNextSlideButton.interactable = true;
    }

    public void SelectLegExercise()
    {
        selectedExercise = ExerciseType.Leg;
        goToNextSlideButton.interactable = true;
    }

    public void AcceptAndGoToNextSlide()
    {
        Assert.IsFalse( selectedExercise == ExerciseType.None );  // Can't proceed unless an exercise is selected

        screenController.SelectedExerciseType = selectedExercise;

        GoToNextScreen();
    }
}
