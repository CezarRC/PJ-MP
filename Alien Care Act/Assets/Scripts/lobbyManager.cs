using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbyManager : MonoBehaviour
{
    public GameObject host_panel, client_panel;
    public string host_name, client_name;
    public photonHandler pHandler;

    public void OnHostReady()
    {
        host_panel.GetComponent<Image>().color = Color.green;
    }
    public void OnClientReady()
    {
        client_panel.GetComponent<Image>().color = Color.green;
    }

    public void OnJoinRoom(string player_name)
    {
        if (PhotonNetwork.isMasterClient)
        {
            host_name = player_name;
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host_name;
            host_panel.SetActive(true);
        }
        else
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.IsMasterClient)
                {
                    host_name = player.NickName;
                }
            }
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host_name;
            host_panel.SetActive(true);
            client_name = player_name;
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client_name;
            client_panel.SetActive(true);
        }
    }

    public void OnSomeoneJoined(PhotonPlayer player)
    {
        if (!player.IsMasterClient)
        {
            client_name = player.NickName;
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client_name;
            client_panel.SetActive(true);
        }
    }

    public void OnMasterLeftRoom()
    {
        if (!PhotonNetwork.isMasterClient) //Aqui eu já sou o Master Client ou não? Debugar para descobrir!
        {
            host_name = client_name;
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host_name;
            client_panel.SetActive(false);
            host_panel.GetComponent<Image>().color = Color.red;
        }
    }
    public void OnClientLeftRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            client_panel.SetActive(false);
        }
    }

    public void OnClickLeftRoom()
    {
        pHandler.GetOutOfTheRoom();
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

    public void Deactivate()
    {
        host_panel.SetActive(false);
        client_panel.SetActive(false);
        gameObject.SetActive(false);
    }
}
