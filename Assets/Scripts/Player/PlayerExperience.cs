using UnityEngine;
using Zenject;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider;

    [Inject] private Player.Settings _settings;
    [Inject] private SignalBus _signalBus;

    private int currentExp;
    private int level = 1;
    private int currentMaxExp;

    private void Start()
    {
        currentMaxExp = GetMaxExperienceByCurrentLevel(level);
        circleCollider.radius = _settings.CollectExpRange;
        _signalBus.Fire(new UpdatePlayerLevel
        {
            NewLevel = level,
            TargetExperience = currentMaxExp
        });
    }

    public void Add(int addExp)
    {
        int preLevel = level;

        while (addExp > 0)
        {
            int needExpToLevelUp = currentMaxExp - currentExp;
            if(addExp >= needExpToLevelUp)
            {
                currentExp = 0;
                addExp -= needExpToLevelUp;
                level++;
                currentMaxExp = GetMaxExperienceByCurrentLevel(level);
            }
            else
            {
                currentExp += addExp;
                addExp = 0;
            }
        }

        if(preLevel != level)
        {
            Debug.Log($"Upgrade: {level - preLevel} level");
            _signalBus.Fire(new UpdatePlayerLevel
            {
                NewLevel = level,
                PreLevel = preLevel,
                TargetExperience = currentMaxExp
            });
        }
        _signalBus.Fire(new UpdatePlayerExperience { CurrentExperience = currentExp });
    }

    public int GetMaxExperienceByCurrentLevel(int level)
    {
        int exp = 5;
        if (level < 2) return exp;
        exp += (level - 1) * 10;
        if (level < 20) return exp;
        exp += (level - 19) * 13;
        if(level < 40) return exp;
        return exp + (level - 39) * 16; 
    }
}
