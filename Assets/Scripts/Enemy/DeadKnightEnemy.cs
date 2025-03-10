using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DeadKnightEnemy : Enemy
{
    public class Factory : PlaceholderFactory<Vector2, DeadKnightEnemy>
    {

    }
}
