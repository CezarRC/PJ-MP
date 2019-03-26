using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbyManager : MonoBehaviour
{
    public GameObject host_panel, client_panel;

    public void OnHostReady()
    {
        host_panel.GetComponent<Image>().color = Color.green;
    }
    public void OnClientReady()
    {
        client_panel.GetComponent<Image>().color = Color.green;
    }
    public bool CanStartGame()
    {
        if((host_panel.GetComponent<Image>().color   == Color.green) && 
           (client_panel.GetComponent<Image>().color == Color.green))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
