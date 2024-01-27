using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    [SerializeField]
    Button StartButton;
    void Start()
    {
        gameObject.SetActive(false);
        GridManager.Instance.SpawnStartButton.AddListener(StartButtonActive);
    }

    private void StartButtonActive()
    {
        gameObject.SetActive(true);
        StartButton.onClick.AddListener(OnClickedEvent);

    }

    private void OnClickedEvent()
    {
        GridManager.Instance.SpawnTrains();
        gameObject.SetActive(false);
    }
}
