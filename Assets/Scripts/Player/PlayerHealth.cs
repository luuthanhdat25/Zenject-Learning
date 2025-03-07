using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    [Inject] private Player player;
    [Inject] private SignalBus signalBus;

    private float currentHP;
    private float regenTimer;
    private float timeRegeneration;

    private void Awake()
    {
        currentHP = player.CurrentStats.MaxHP;
        signalBus.Subscribe<DealDamagePlayer>(OnGetHit);
        signalBus.Subscribe<UpdateHPRegeneration>(OnUpdateHPRegeneration);
        OnUpdateHPRegeneration();
    }

    private void OnUpdateHPRegeneration()
    {
        timeRegeneration = 5f / (1 + ((player.CurrentStats.HPRegeneration - 1) / 2.25f));
        regenTimer = 0;
    }

    private void Start()
    {
        signalBus.Fire(new UpdatePlayerHP()
        {
            CurrentHP = currentHP,
            MaxHP = player.CurrentStats.MaxHP
        });
    }

    private void OnGetHit(DealDamagePlayer args)
    {
        DeductHP(args.Value);
    }
 
    private void DeductHP(int value)
    {
        if (currentHP < 0) return;  
        if (currentHP > 0)
        {
            currentHP -= CalculateDamageReceivedAfterArmor(value);
            if (currentHP < 0) currentHP = 0;

            signalBus.Fire(new UpdatePlayerHP()
            {
                CurrentHP = currentHP,
                MaxHP = player.CurrentStats.MaxHP
            });

            signalBus.Fire<PlayerGetHit>();
        }

        if (currentHP == 0)
        {
            signalBus.Fire<PlayerDie>();
            Debug.Log("Player Die");
        }
    }

    private int CalculateDamageReceivedAfterArmor(int value)
    {
        int armor = player.CurrentStats.Armor;
        float damageReceivedPercent = armor >= 0
                                         ? 1f / (1f + (armor / 15f))
                                         : (15f - 2f * armor) / (15f - armor);
        return Mathf.RoundToInt(value * damageReceivedPercent);
    }

    private void PlusHP(int value)
    {
        if (currentHP >= player.CurrentStats.MaxHP) return;
        currentHP = Mathf.Min(currentHP + value, player.CurrentStats.MaxHP);
        signalBus.Fire(new UpdatePlayerHP()
        {
            CurrentHP = currentHP,
            MaxHP = player.CurrentStats.MaxHP
        });
    }

    private void FixedUpdate()
    {
        HandleHPRegeneration();
    }

    private void HandleHPRegeneration()
    {
        if (currentHP >= player.CurrentStats.MaxHP || player.CurrentStats.HPRegeneration < 1) return;
        regenTimer += Time.fixedDeltaTime;
        if(regenTimer >= timeRegeneration)
        {
            regenTimer = 0;
            PlusHP(1);
        }
    }
}
