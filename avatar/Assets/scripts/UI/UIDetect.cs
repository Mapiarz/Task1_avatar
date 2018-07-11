using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIDetect : UIWidget
{
    [SerializeField] GameObject image;
    [SerializeField] AvatarController controller;
    [SerializeField] GameObject skipButton;

    /// <summary>
    /// coroutine responsible for rotating a spinner during work of controller
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotate()
    {
        while (!controller.serversFound)
        {
            Debug.Log("servers not found");
            image.transform.Rotate(0, 0, 50 * Time.deltaTime);
            yield return null;
        }
        skipButton.SetActive(true);
        image.SetActive(false);
    }

    public void OnDiscoverClick()
    {
        image.SetActive(true);
        StartCoroutine(Rotate());
        controller.DiscoverServers();

    }
    void Awake()
    {
        skipButton.SetActive(false);
        image.SetActive(false);
        DOTween.Init();
        ReadyToSkip = false;
        Active = true;
        Choice = 0;
    }
}
