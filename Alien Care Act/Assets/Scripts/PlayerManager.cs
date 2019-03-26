using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    enum PlayerClass
    {
        Engineer,
        Scrapper
    }

    int player_class;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerClass(int player_class, Sprite player_sprite)
    {
        //This will need to be refactored in the future, we should add a SpriteBuffer Class to handle the sprites while in the game
        transform.Find("Sprite").GetComponent<Image>().sprite = player_sprite;
        this.player_class = player_class;
    }
}
