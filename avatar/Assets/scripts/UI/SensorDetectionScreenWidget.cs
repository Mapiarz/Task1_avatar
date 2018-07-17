using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Aisens;

public class SensorDetectionScreenWidget : BaseScreenWidget
{
    [SerializeField] Button discoverServersButton;
    [SerializeField] SensorManagerBehaviour sensorManager;
    [SerializeField] AvatarController avatarController;
    [SerializeField] GameObject spinner;

    Coroutine discoveryCoroutine;
    bool discoveryFinished;

    protected override void Awake()
    {
        base.Awake();
        spinner.SetActive(false);
    }

    public void DiscoverServers()
    {

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
        spinner.SetActive(true);
        discoverServersButton.interactable = false;
        discoveryFinished = false;
        StartCoroutine(SpinnerRotation());
        yield return sensorManager.DiscoverSensors(5000, (result) =>
        {
            if (result != null)
            {
                Debug.Log($"Discovered {result.Count} sensors");
                screenController.NumberOfSensors = result.Count;
                avatarController.AssignSensors(result);
                discoveryFinished = true;
            }
            else
            {
                Debug.LogWarning("Sensor discovery failed");
                discoveryFinished = true;
            }
        });

        discoveryCoroutine = null;
        spinner.SetActive(false);
        discoverServersButton.interactable = true;
        GoToNextScreen();
    }

    /// <summary>
    /// starts the animation during discover sensor coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator SpinnerRotation()
    {
        while (!discoveryFinished)
        {
            spinner.transform.Rotate(0, 0, 100f * Time.deltaTime);
            yield return null;
        }
    }
}
