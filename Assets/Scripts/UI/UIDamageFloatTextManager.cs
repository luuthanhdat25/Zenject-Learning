using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIDamageFloatTextManager : IInitializable
{
    private SignalBus _signalBus;
    private DamageFloatingTextPool _damageFloatingTextPool;

    [Inject]
    public void Construct(SignalBus signalBus, DamageFloatingTextPool damageFloatingTextPool)
    {
        _signalBus = signalBus;
        _damageFloatingTextPool = damageFloatingTextPool;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<EnemyGetHit>(PopUpFloatingText);
    }

    private void PopUpFloatingText(EnemyGetHit args)
    {
        UIDamageFloatingText floatingText = _damageFloatingTextPool.Spawn();
        floatingText.UpdateUI(args.DamageHit.ToString(), args.IsCrit ? Color.red : Color.white, args.Position);
        floatingText.transform.position = args.Position;
    }
}
