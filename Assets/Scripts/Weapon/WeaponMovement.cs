using UnityEngine;
using Zenject;

public class WeaponMovement : MonoBehaviour
{
    [SerializeField] private WeaponDetect weaponDetect;
    
    private InputManager _inputManager;
    private bool isRotatable = true;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!isRotatable) return;
        transform.rotation = GetLookTargetRotation();
    }

    private Quaternion GetLookTargetRotation()
    {
        Vector2 targetDirection = weaponDetect.NearestTarget
            ? weaponDetect.NearestTarget.position - transform.position
            : _inputManager.GetMoveInput();

        return Quaternion.Euler(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg);
    }
}
