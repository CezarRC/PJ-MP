using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum mode {idle, roaming, approaching, attacking };

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private int health, movementSpeed;
    [SerializeField]
    private float attackSpeed;
    private float nextAtk;

    public int healthMin, healthMax;
    public int moveMin, moveMax;
    public float atkSpeedMin, atkSpeedMax;
    public Collider2D aggroRange;

    [SerializeField]
    private Collider2D[] colliders;
    mode status;

    [SerializeField]
   private GameObject target;

    [SerializeField]
    private Vector2 idleTarget;

    

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(healthMin, healthMax);
        movementSpeed = Random.Range(moveMin, moveMax);
        attackSpeed = Random.Range(atkSpeedMin, atkSpeedMax);
        nextAtk = Time.time + attackSpeed;

        colliders = new Collider2D[10];
    }

    // Update is called once per frame
    void Update()
    {

        switch (status)
        {

            case (mode)0 :idle();break;
            case (mode)1: roam(); break;
            case (mode)2: approach();break;
            case (mode)3: attack();break;




        }
        
    }
 
    
    //choose a random spot nearby, call the roam function towards it
    void idle()
    {
        idleTarget = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        status = mode.roaming;

        
       
    }

    //roam until reaches the target spot, testing if the aggro range hits something, if hits, calls approach
    void roam ()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, idleTarget, movementSpeed * Time.deltaTime);

        aggroRange.GetContacts(colliders);
        if (colliders[0] != null)
        { 
        for (int i = 0; i < colliders.Length; i++)
        {
                if (colliders[i] == null)
                    break;

            else if (colliders[i].transform.gameObject.tag == "Player" || colliders[i].transform.gameObject.tag == "Buildings")
            {
                target = colliders[i].transform.gameObject;
                status = mode.approaching;
                
                return;
            }

            else if (colliders[i].transform.gameObject.tag == "wall")
            {
                Debug.Log("bati na parede");
                colliders = new Collider2D[10];
                status = mode.idle;
                return;
            }
        }
        }



        if (transform.position.AlmostEquals(idleTarget,movementSpeed))
        {
            status = mode.idle;
        }

    }

    //move towards aggro target, if close calls attack 
    void approach()
    {
        if(Vector2.Distance(transform.position,target.transform.position) > 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
        }

        else
        {
            status = mode.attacking;
        }
    }

    //check if can attack, if so attacks and enter delay mode
    void attack()
    {

        if(Time.time > nextAtk)
        {
            nextAtk = Time.time;
            //fun;'ao de ataque

            Debug.Log("Ataquei sim");
        }

    }

   
}
