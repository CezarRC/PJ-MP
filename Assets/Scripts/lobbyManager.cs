using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbyManager : MonoBehaviour
{
    public GameObject host_panel, client_panel;

    public photonHandler pHandler;

    public PhotonView photonView;

    public PlayerLobbyPanel host, client;

    private void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    public void OnHostReady()
    {
        photonView.RPC("RPCSetHostReady", PhotonTargets.All);
    }
    
    public void OnClientReady()
    {
        photonView.RPC("RPCSetClientReady", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCSetHostReady()
    {
        host_panel.GetComponent<Image>().color = Color.green;
    }

    [PunRPC]
    public void RPCSetClientReady()
    {
        client_panel.GetComponent<Image>().color = Color.green;
    }

    [PunRPC]
    public void RPCStartGame()
    {
        pHandler.StartGame();
    }

    [PunRPC]
    public void RPCRequestHostClass()
    {
        Debug.Log("Requisitei a host_class");
        photonView.RPC("RPCSetHostClassInClient", PhotonTargets.Others, host.GetClass());
    }

    [PunRPC]
    public void RPCSetHostClassInClient(string host_class)
    {
        Debug.Log("Setei a classe do cliente para: " + host_class);
        host.SetClass(host_class);
    }

    [PunRPC]
    public void RPCRequestClientClass()
    {
        photonView.RPC("RPCSetClientClassInHost", PhotonTargets.Others, client.GetClass());
    }

    [PunRPC]
    public void RPCSetClientClassInHost(string client_class)
    {
        client.SetClass(client_class);
    }

    public void OnJoinRoom(string player_name, string selected_char)
    {
        if (PhotonNetwork.isMasterClient)
        {
            host.SetHostName(player_name);
            host.SetClass(selected_char);
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            host_panel.SetActive(true);
        }
        else
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.IsMasterClient)
                {
                    host.SetHostName(player.NickName);
                    photonView.RPC("RPCRequestHostClass", PhotonTargets.MasterClient);
                }
            }
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            host_panel.SetActive(true);
            client.SetClientName(player_name);
            client.SetClass(selected_char);
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client.GetClientName();
            client_panel.SetActive(true);
        }
    }

    public void OnSomeoneJoined(PhotonPlayer player)
    {
        if (!player.IsMasterClient)
        {
            client.SetClientName(player.NickName);
            photonView.RPC("RPCRequestClientClass", PhotonTargets.Others);
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client.GetClientName();
            client_panel.SetActive(true);
        }
    }

    public void OnMasterLeftRoom()
    {
        if (!PhotonNetwork.isMasterClient) //Aqui eu já sou o Master Client ou não? Debugar para descobrir!
        {
            host.SetHostName(client.GetClientName());
            host.SetClass(client.GetClass());
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            client_panel.SetActive(false);
            host_panel.GetComponent<Image>().color = Color.red;
        }
    }
    public void OnClientLeftRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            client_panel.SetActive(false);
            client_panel.GetComponent<Image>().color = Color.red;
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

    public void OnClickStartGame()
    {
        if (PhotonNetwork.isMasterClient && CanStartGame())
        {
            photonView.RPC("RPCStartGame", PhotonTargets.All);
        }
    }

    public void Deactivate()
    {
        host_panel.SetActive(false);
        client_panel.SetActive(false);
        gameObject.SetActive(false);
    }
}
