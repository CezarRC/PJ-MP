using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootEng : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bullet_start_point;

    public float fireRate = 0.2f;

    public AimObjectSpread aimObjectSpread;
    
    private float nextFire;

    private PhotonView pView;
    
    void Start()
    {
        pView = this.GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if (pView.isMine)
        {
            HandleInputs();
        }
    }

    public void Shoot(Vector3 bullet_end_point)
    {
        GameObject bullet = bulletPrefab;
        Vector2 direction = bullet_end_point - bullet_start_point.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        bullet.transform.position = bullet_start_point.position;
        PhotonNetwork.Instantiate("bullet", bullet.transform.position, bullet.transform.rotation, 0);
    }

    void HandleInputs()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(aimObjectSpread.GetSpreadPoint());
        }
    }

}
