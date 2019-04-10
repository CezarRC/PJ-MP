using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
        GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
