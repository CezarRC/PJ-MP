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

    PhotonView photonView;

    public GameObject myReadyStatus;

    public string myPlayerName;

    public photonConnect pConnect;

    public lobbyManager lManager;

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneFinishedLoading;
        DontDestroyOnLoad(this.transform);
        PhotonNetwork.automaticallySyncScene = true;
        photonView = PhotonView.Get(this);
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
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            lManager.OnMasterLeftRoom();
        }
    }

    [PunRPC]
    void OnClientLeavingRoom()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            lManager.OnClientLeftRoom();
        }
    }

    IEnumerator OnPartnerLeftGame()
    {
        PhotonNetwork.LoadLevel("PartnerDisconnected");
        yield return new WaitForSeconds(5);
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void OnQuitInGame()
    {
        OnPartnerLeftGame();
    }

    public void OnClickQuitInGame()
    {
        photonView.RPC("OnQuitInGame", PhotonTargets.Others);
        PhotonNetwork.LoadLevel("Main Menu");
        PhotonNetwork.Disconnect();
    }

    public void GetOutOfTheRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("OnMasterClientLeavingRoom", PhotonTargets.Others);
        }
        else
        {
            photonView.RPC("OnClientLeavingRoom", PhotonTargets.Others);
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
        lManager.gameObject.SetActive(true);

        lManager.OnJoinRoom(pButton.playerName.text, selection.getSelectedChar());
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

    public string GetMyPlayerName()
    {
        return myPlayerName;
    }
    
    [PunRPC]
    public void OnPlayerDied(string name)
    {
        GameObject.Find(name).SetActive(false);
    }

    [PunRPC]
    public void OnPlayerRespawn(string name)
    {
        GameObject.Find(name).SetActive(true);
    }

    public void RespawnPlayer(string name)
    {
        photonView.RPC("OnPlayerRespawn", PhotonTargets.All, name);
    }

    public void MyPlayerDied(string name)
    {
        myPlayerName = name;
        photonView.RPC("OnPlayerDied", PhotonTargets.All, name);
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
