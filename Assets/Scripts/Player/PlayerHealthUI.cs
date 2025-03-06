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
        _signalBus.Subscribe<UpdatePlayerHP>(UpdateHealthBar);
    }

    private void UpdateHealthBar(UpdatePlayerHP args)
    {
        healthBar.fillAmount = args.CurrentHP / args.MaxHP;
        textMesh.text = $"{args.CurrentHP}/{args.MaxHP}";
    }
}
