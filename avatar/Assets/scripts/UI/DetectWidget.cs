using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DetectWidget : BaseScreenWidget
{
    [SerializeField] Button goToNextSlideButton;
    [SerializeField] Button discoverServersButton;
    [SerializeField] AvatarController controller;
    [SerializeField] GameObject spinner;

    protected override void Awake()
    {
        base.Awake();
        spinner.SetActive(false);
        goToNextSlideButton.interactable = false;
    }

    public void StartDiscoveryCoroutine()
    {
        StartCoroutine( StartServerDiscovery() );
    }

    public IEnumerator StartServerDiscovery()
    {
        spinner.SetActive(true);

        discoverServersButton.interactable = false;

        controller.DiscoverServers();

        while(!controller.serversFound)
        {
            spinner.transform.Rotate(0, 0, 100f * Time.deltaTime);
            yield return null;
        }

        discoverServersButton.interactable = true;
        spinner.SetActive(false);
        goToNextSlideButton.interactable = true;
    }

    public void FinishedDiscoveryGoToNextSlide()
    {
        Assert.IsTrue(controller.serversFound);

        GoToNextScreen();
    }
}
