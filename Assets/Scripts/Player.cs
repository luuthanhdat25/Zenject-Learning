using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerExperience PlayerExperience;

    public Vector2 GetPosition() => transform.position;

    [System.Serializable]
    public class Settings //Default Stats
    {
        public int MaxHealth;
        public float MoveSpeed;
        public float Might; //Strength
        public float Area;
        public float Speed;
        public float Duration;
        public float CoolDown;
        public float Luck;
        public float CollectExpRange = 1;
    }
}
