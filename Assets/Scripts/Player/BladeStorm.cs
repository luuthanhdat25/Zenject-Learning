using UnityEngine;

public class BladeStorm : Weapon
{
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private Transform bladePrefab;
    [SerializeField] private int numberOfBlade = 1;
    [SerializeField] private int radius = 0;

    private float currentRotation;
    
    private void Start()
    {
        currentRotation = transform.eulerAngles.z;

        bladePrefab.gameObject.SetActive(false);
        float bladeRadius = (float)360 / numberOfBlade;
        float startRotation = 0;
        
        for (int i = 1; i <= numberOfBlade; i++)  
        {
            var newBlade = Instantiate(bladePrefab, transform);
            Quaternion rotation = Quaternion.Euler(0, 0, startRotation);
            newBlade.localRotation = rotation;
            newBlade.localPosition = GetPositionOnCircle(transform.localPosition, rotation, radius);
            newBlade.gameObject.SetActive(true);
            startRotation += bladeRadius;
        }
    }

    public static Vector3 GetPositionOnCircle(Vector3 startPoint, Quaternion rotation, float radius)
    {
        float angle = rotation.eulerAngles.z * Mathf.Deg2Rad;

        float x = startPoint.x + radius * Mathf.Cos(angle);
        float y = startPoint.y + radius * Mathf.Sin(angle);

        return new Vector3(x, y, startPoint.z);
    }

    void FixedUpdate()
    {
        currentRotation = (currentRotation - rotationSpeed * Time.fixedDeltaTime) % 360;
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
