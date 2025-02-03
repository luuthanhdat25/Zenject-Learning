using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    private SignalBus _signalBus;
    private Player.Settings _settings;

    [Inject]
    private void Contruct(SignalBus signalBus, Player.Settings settings)
    {
        this._signalBus = signalBus;
        this._settings = settings;
    }

    private int currentHealth;

    private void Awake()
    {
        currentHealth = _settings.MaxHealth;
        _signalBus.Subscribe<DealDamagePlayer>(OnGetHit);
    }

    private void Start()
    {
        _signalBus.Fire(new UpdatePlayerHealth()
        {
            HealthPersent = 1
        });
    }

    private void OnGetHit(DealDamagePlayer args)
    {
        if (currentHealth > 0)
        {
            Debug.Log(args.Value);
            currentHealth -= args.Value;
            if (currentHealth < 0) currentHealth = 0;

            _signalBus.Fire(new UpdatePlayerHealth()
            {
                HealthPersent = (float)currentHealth / _settings.MaxHealth
            });
        }
    }
}
