using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmArea : MonoBehaviour
{
    private int health = 100;
    // Start is called before the first frame update
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
