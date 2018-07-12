using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DetectScreenWidget : BaseScreenWidget
{
    [SerializeField] Button discoverServersButton;
    [SerializeField] AvatarController avatarController;
    [SerializeField] GameObject spinner;

    int numberOfSensors;

    protected override void Awake()
    {
        base.Awake();
        numberOfSensors = -1;
        spinner.SetActive(false);
    }

    public void StartDiscoveryCoroutine()
    {
        StartCoroutine( StartServerDiscovery() );
    }

    public IEnumerator StartServerDiscovery()
    {
        spinner.SetActive(true);

        discoverServersButton.interactable = false;

        avatarController.DiscoverServers();

        while(!avatarController.discoveryFinished)
        {
            spinner.transform.Rotate(0, 0, 100f * Time.deltaTime);
            yield return null;
        }
        numberOfSensors = avatarController.numberOfSensors;
        Debug.Log($"DetectScreenWidget number {numberOfSensors}");
        screenController.NumberOfSensors = numberOfSensors;
        GoToNextScreen();
    }
}
