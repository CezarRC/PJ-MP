using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperRepair : MonoBehaviour
{
    private Camera mainCamera;
    public PhotonView pView;

    public int repairAmount = 3;
    public float repairAmount_buff = 1;

    void Start()
    {
        mainCamera = transform.Find("PlayerAnchor").Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (pView.isMine)
        {
            if (Input.GetMouseButton(0))
            {
                ScreenMouseRay();
            }
        }
    }

    void BuffRepairAmount(float buff)
    {
        repairAmount_buff += buff;
    }

    void Repair(GameObject construction)
    {
        //float repair_value = repairAmount * repairAmount_buff;
        //construction.GetComponent<Construction>().Repair()
    }

    void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (c.tag == "construction")
                {
                    Repair(c.gameObject);
                }
            }
        }
    }
}
