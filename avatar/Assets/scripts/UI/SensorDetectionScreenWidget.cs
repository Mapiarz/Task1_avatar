using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Aisens;

public class SensorDetectionScreenWidget : BaseScreenWidget
{
    [SerializeField] Button discoverServersButton;
    [SerializeField] AvatarController avatarController;
    [SerializeField] GameObject spinner;
    [SerializeField] SensorManagerBehaviour sensorManager;

    Coroutine discoveryCoroutine;
    bool discoveryFinished;

    /// <summary>
    /// overriding allows another use of dicovery button if not all sensors were discovered
    /// </summary>
    /// <returns></returns>
    public override IEnumerator ShowCoroutine()
    {
        spinner.SetActive(false);
        discoverServersButton.interactable = true;
        yield return StartCoroutine(base.ShowCoroutine());
    }

    public void DiscoverServers()
    {
        discoveryFinished = false;

        if (discoveryCoroutine != null)
        {
            Debug.Log("corutine null");
            return;  // Note: Sensor discovery coroutine doesn't support cancellation so we have to return when multiple attempts are made
        }
        Debug.Log("corutine discoverSensors");
        discoveryCoroutine = StartCoroutine(DiscoverSensors());
    }

    IEnumerator DiscoverSensors()
    {
        StartCoroutine(StartRotatingAnimation());
        yield return sensorManager.DiscoverSensors(5000, (result) =>
        {
            if (result != null)
            {
                Debug.Log($"Discovered {result.Count} sensors");
                screenController.infos = result;
            }
            else
            {
                Debug.LogWarning("Sensor discovery failed");
            }
        });

        discoveryFinished = true;
        discoveryCoroutine = null;
    }

    public IEnumerator StartRotatingAnimation()
    {
        spinner.SetActive(true);

        discoverServersButton.interactable = false;

        while(!discoveryFinished)
        {
            spinner.transform.Rotate(0, 0, 100f * Time.deltaTime);
            yield return null;
        }
        GoToNextScreen();
    }
}
