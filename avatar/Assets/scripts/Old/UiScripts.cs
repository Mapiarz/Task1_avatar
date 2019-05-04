using UnityEngine;

/// <summary>
/// class related to physical objects in the Unity UI, responds to users actions
/// </summary>
public class UiScripts : MonoBehaviour {

    public GameObject button, sliders;
    public OldAvatarController controller;

    /// <summary>
    /// bool responsible for changing state of UI elements to opposite
    /// </summary>
    bool boolToggle;

    void Awake()
    {
        boolToggle = false;
    }

    /// <summary>
    /// on click of Manual Drive button rearranges interface, activates sliders, allows manual control in the AvatarController
    /// </summary>
    public void ChangeState()
    {
        button.SetActive(boolToggle);
        sliders.SetActive(!boolToggle);
        controller.ChangeDataSource();
        boolToggle = !boolToggle;
    }
}
