using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerExperienceUI : MonoBehaviour
{
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Inject] private SignalBus _signalBus;

    private int maxLevelExperience;

    private void Awake()
    {
        _signalBus.Subscribe<UpdatePlayerExperience>(OnUpdateExperience);
        _signalBus.Subscribe<UpdatePlayerLevel>(OnUpdateLevel);
    }

    private void OnUpdateLevel(UpdatePlayerLevel args)
    {
        levelText.text = "LV " + args.NewLevel;
        maxLevelExperience = args.TargetExperience;
    }

    private void OnUpdateExperience(UpdatePlayerExperience args)
    {
        expBar.fillAmount = (float)args.CurrentExperience / maxLevelExperience;
    }
}
