using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private List<Weapon> weaponList;
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private Transform weaponPool;
    [SerializeField] private float radius = .5f;

    private void Start()
    {
        for(int i = 1; i <= 6; i++)
        {
            var newWeapon = Instantiate(weaponPrefab, weaponPool);
            weaponList.Add(newWeapon);
        }

        weaponPrefab.gameObject.SetActive(false);

        float gapRadius = (float)360 / weaponList.Count;
        float startRotation = 0;

        foreach (Weapon weapon in weaponList)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, startRotation);
            weapon.transform.localPosition = GetPositionOnCircle(transform.localPosition, rotation, radius);
            weapon.gameObject.SetActive(true);
            startRotation += gapRadius;
        }
    }

    public static Vector3 GetPositionOnCircle(Vector3 startPoint, Quaternion rotation, float radius)
    {
        float angle = rotation.eulerAngles.z * Mathf.Deg2Rad;

        float x = startPoint.x + radius * Mathf.Cos(angle);
        float y = startPoint.y + radius * Mathf.Sin(angle);

        return new Vector3(x, y, startPoint.z);
    }

    /*private void FixedUpdate()
    {
        bool hasEnemy = false;
        if (!hasEnemy)
        {
            Vector2 targetDirection = GetMoveInput();
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            foreach (var weapon in weaponList)
            {
                weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }

    private const string INPUT_HORIZONTAL = "Horizontal";
    private const string INPUT_VERTICAL = "Vertical";
    private Vector2 GetMoveInput()
    {
        float moveX = Input.GetAxisRaw(INPUT_HORIZONTAL);
        float moveY = Input.GetAxisRaw(INPUT_VERTICAL);

        return new Vector2(moveX, moveY);
    }*/
}
