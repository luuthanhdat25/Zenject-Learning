using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private List<Weapon> weaponList;
    [SerializeField] private Transform weaponPool;
    [SerializeField] private float radius = .5f;

    private void Start()
    {
        float gapRadius = (float)360 / weaponList.Count;
        float startRotation = 90;

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
}
