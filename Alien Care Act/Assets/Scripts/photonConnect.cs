using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonConnect : MonoBehaviour
{
    public string versionName = "0.1";

    public GameObject connectingPanel;

    public GameObject connectionPanel;

    private void Awake()
    {
    }

    public void connectToGameServers()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);

        Debug.Log("Connectiong to photon Game Servers...");
        connectingPanel.SetActive(true);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("We are connected to master server");
        connectingPanel.SetActive(false);
        connectionPanel.SetActive(true);
    }

    private void OnJoinedLobby()
    {
        Debug.Log("On Joined Lobby");
    }

    private void OnDisconnectedFromPhoton()
    {
        SceneManager.LoadScene("LostConnection", LoadSceneMode.Single);
        Debug.Log("Disconnected from the Game Server");
    }

    private void OnFailedToConnectToPhoton()
    {
        Debug.Log("Failed to connect to Game Server");
    }

}
