using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonHandler : MonoBehaviour
{
    public photonButton pButton;

    public GameObject mainPlayer;

    public Transform engSpawnPlace, scrapperSpawnPlace;

    public CharSelection selection;

    public GameObject myReadyStatus;

    public lobbyManager lManager;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform);
    }

    public void CreateRoom()
    {
        if (pButton.playerName.text.Length >= 3)
        {
            if (pButton.hostGame.text.Length >= 1)
            {
                PhotonNetwork.CreateRoom(pButton.hostGame.text, new RoomOptions() { MaxPlayers = 2 }, null);
            }
            else
            {
                pButton.hostGame.placeholder.GetComponent<UnityEngine.UI.Text>().color = Color.red;
            }
        }
        else
        {
            pButton.playerName.placeholder.GetComponent<UnityEngine.UI.Text>().color = Color.red;
        }
    }

    public void JoinRoom()
    {
        if(selection.getSelectedChar() != null)
        {
            if (pButton.playerName.text.Length >= 3)
            {
                PhotonNetwork.player.NickName = pButton.playerName.text;
                if (pButton.joinGame.text.Length >= 1)
                {
                    PhotonNetwork.JoinOrCreateRoom(pButton.joinGame.text, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
                }
                else
                {
                    pButton.joinGame.placeholder.GetComponent<UnityEngine.UI.Text>().color = Color.red;
                }
            }
            else
            {
                pButton.playerName.placeholder.GetComponent<UnityEngine.UI.Text>().color = Color.red;
            }
        }
    }

    [PunRPC]
    void OnMasterClientLeavingRoom()
    {
        lManager.OnMasterLeftRoom();
    }

    public void GetOutOfTheRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("OnMasterClientLeavingRoom", PhotonTargets.Others);
        }
        PhotonNetwork.LeaveRoom();
        lManager.Deactivate();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        lManager.OnClientLeftRoom();
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        lManager.OnSomeoneJoined(player);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("We are connected to the room!");

        lManager.gameObject.SetActive(true);

        lManager.OnJoinRoom(pButton.playerName.text);
    }

    public void MoveScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Main Menu")
        {

        }
        if(scene.name == "FirstLevel")
        {
            spawnPlayer();
        }
    }

    private void spawnPlayer()
    {
        if (selection.getSelectedChar() == "Engineer")
        {
            PhotonNetwork.Instantiate("Engineer", engSpawnPlace.position, engSpawnPlace.rotation, 0);
        }
        else if (selection.getSelectedChar() == "Scrapper")
        {
            PhotonNetwork.Instantiate("Scrapper", scrapperSpawnPlace.position, scrapperSpawnPlace.rotation, 0);
        }
    }

    public void OnReadyButton()
    {
        if (PhotonNetwork.isMasterClient)
        {
            lManager.OnHostReady();
        }
        else
        {
            lManager.OnClientReady();
        }
    }

    public void StartGame()
    {
        MoveScene("FirstLevel");
    }

}
