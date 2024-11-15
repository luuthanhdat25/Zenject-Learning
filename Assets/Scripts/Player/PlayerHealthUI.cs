using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI textMesh;

    private SignalBus _signalBus;

    [Inject]
    private void Contruct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<UpdatePlayerHealth>(UpdateHealthBar);
    }

    private void UpdateHealthBar(UpdatePlayerHealth args)
    {
        healthBar.fillAmount = args.HealthPersent;
        textMesh.text = (args.HealthPersent * 100).ToString();
    }
}
