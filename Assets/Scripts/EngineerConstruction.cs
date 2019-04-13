using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerConstruction : MonoBehaviour
{
    private Camera mainCamera;
    public PhotonView pView;

    public GameObject selectedConstruction;

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

    void Construction(GameObject whereToBuild, GameObject construction)
    {
        
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
                    Debug.Log("Can't Build Here");
                }
                if (c.tag == "empty_place")
                {
                    Construction(c.gameObject, selectedConstruction);
                }
            }
        }
    }

}
