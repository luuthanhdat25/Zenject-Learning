using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

public class EnemyInstaller : Installer<EnemyInstaller>
{
    [Inject] private Settings _settings;

    public override void InstallBindings()
    {
        Container.BindFactory<Vector2, DeadKnightEnemy, DeadKnightEnemy.Factory>()
            .FromMonoPoolableMemoryPool(x => x.WithInitialSize(10)
                .FromComponentInNewPrefab(_settings.DeadKnightPrefab)
                .WithGameObjectName(_settings.DeadKnightPrefab.name));
    }

    [Serializable]
    public class Settings
    {
        public DeadKnightEnemy DeadKnightPrefab;

        public List<EnemyData> EnemySettings;
    }

    [Serializable]
    public class EnemyData
    {
        public string Id;
        public int BaseHP = 1;
        public int HPIncreasePerWave = 2;
        public float Speed = 200;
        public int Damage = 1;
        public float DamageIncreasePerWave = 0.6f;
        public bool canBeKnockBack = true;
    }
}
