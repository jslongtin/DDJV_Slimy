using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{

    public float damage = 10f;

    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ennemi"))
        //{
           
        //    // do something
        //}

        //// Destroy the projectile after it collides with an object
        //Destroy(gameObject);
    }
}
