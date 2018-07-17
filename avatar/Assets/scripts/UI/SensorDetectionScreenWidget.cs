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
    int numberOfSensors;

    protected override void Awake()
    {
        base.Awake();
        numberOfSensors = -1;
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
        yield return sensorManager.DiscoverSensors(5000, (result) =>
        {
            if (result != null)
            {
                Debug.Log($"Discovered {result.Count} sensors");
                screenController.NumberOfSensors = result.Count;
                avatarController.AssignSensors(result);
            }
            else
            {
                Debug.LogWarning("Sensor discovery failed");
            }
        });

        discoveryCoroutine = null;
        GoToNextScreen();
    }
}
