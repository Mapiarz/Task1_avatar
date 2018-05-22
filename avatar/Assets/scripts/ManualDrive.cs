using UnityEngine;

/// <summary>
/// class related to physical objects in the Unity UI, responds to users actions
/// </summary>
public class ManualDrive : MonoBehaviour {

    public GameObject button, sliders;
    public AvatarController controller;

    /// <summary>
    /// bool responsible for changing state of UI elements to opposite
    /// </summary>
    bool boolToggle;

    private void Awake()
    {
        boolToggle = false;
    }
    /// <summary>
    /// on click of Manual Drive button rearranges interface, activates sliders, allows manual control in the AvatarControler
    /// </summary>
    public void ChangeState()
    {
        button.SetActive(boolToggle);
        sliders.SetActive(!boolToggle);
        controller.ManualSwitch();
        boolToggle = !boolToggle;
    }
}
