using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject gameMenu;
    public GameObject pHandler;

    void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy");
    }

    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            gameMenu.SetActive(!gameMenu.activeInHierarchy);
        }
    }
    public void QuitInGame()
    {
        pHandler.GetComponent<photonHandler>().OnClickQuitInGame();
    }
}
