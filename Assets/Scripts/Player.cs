using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private SignalBus _signalBus;
    private Settings _settings;

    [Inject]
    private void Contruct(SignalBus signalBus, Settings settings)
    {
        this._signalBus = signalBus;
        this._settings = settings;
    }

    private int currentHealth;

    private void Awake()
    {
        currentHealth = _settings.Health;
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
                HealthPersent = (float)currentHealth / _settings.Health 
            });
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;

        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;
    }

    public Vector2 GetPosition() => transform.position;

    [System.Serializable]
    public class Settings
    {
        public int Health;
    }
}
