using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1000f;

    void Start()
    {
        Destroy(gameObject, 2);
        GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
    }
    
}
