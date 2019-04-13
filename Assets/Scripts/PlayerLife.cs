using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int Health = 1000;
    private bool Alive = true;

    public GameObject PlayerHolder;
    private photonHandler pHandler;
    private MenuManager lManager;
    
    void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        lManager = GameObject.Find("LevelManager").GetComponent<MenuManager>();
    }
    

    public bool IsAlive()
    {
        return Alive;
    }

    public void ApplyDamage(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            KillThisPlayer();
            ActivateRespawnMenu(true);
        }
    }
    
    private void ActivateRespawnMenu(bool flag)
    {
        lManager.ActivateDeathMenu(flag);
    }

    public void KillThisPlayer()
    {
        Alive = false;
        pHandler.MyPlayerDied(gameObject.name);
    }
    
    public void RespawnThisPlayer()
    {
        Alive = true;
        Health = 1000;
        pHandler.RespawnPlayer(gameObject.name);
        ActivateRespawnMenu(false);
    }
}
