using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    private List<Enemy> enemyList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy e)) 
        {
            enemyList.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy e))
        {
            if (!enemyList.Contains(e)) 
            {
                enemyList.Remove(e);
            }
        }
    }
}
