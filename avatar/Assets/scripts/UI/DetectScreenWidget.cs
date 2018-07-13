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

    /// <summary>
    /// overriding allows another use of dicovery button if not all sensors were discovered
    /// </summary>
    /// <returns></returns>
    public override IEnumerator ShowCoroutine()
    {
        numberOfSensors = -1;
        spinner.SetActive(false);
        discoverServersButton.interactable = true;
        yield return StartCoroutine(base.ShowCoroutine());
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
        //waits for flag from avatarController
        while(!avatarController.discoveryFinished)
        {
            spinner.transform.Rotate(0, 0, 100f * Time.deltaTime);
            yield return null;
        }
        //assign number of discovered sensors to global variable in Widget Controller
        numberOfSensors = avatarController.infos.Count;
        Debug.Log($"DetectScreenWidget number {numberOfSensors}");
        screenController.NumberOfSensors = numberOfSensors;
        GoToNextScreen();
    }
}
