using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneDisplay : MonoBehaviour
{
    public ChoiceCategory runtimeChoices;
    public SettingsScrObj gameSettings;

    public MusicManager musicManager;

    public EndSceneVisuals endSceneVisuals;
    private bool didHeroWin = false;

    [SerializeField]
    private float scrollSpeed;

    public GameObject baseText;
    public GameObject creditsText;

    private void Start()
    {

        foreach (PlayerItems item in runtimeChoices.playerItems)
        {
            didHeroWin = (item.itemName == "Victory Shades") ? true : didHeroWin;
        }

        /*
        if (!didHeroWin)
        {

            //EndSceneVisuals instance = Instantiate(endSceneVisuals, transform);
            //endSceneVisuals.GetComponent<TextMeshProUGUI>().text = "Not Spam2";
           
            //Hero lost/Deities won
            //MusicManager.PlayMusic();


            Instantiate(creditsText,transform); //Credits text..
            return;
        }
        */

        //Hero won/Deities lost:
        //MusicManager.PlayMusic();

        Sprite playerSpr = runtimeChoices.chosenHero.characterSprite;

        //if hero won:
        for (int i = 1; i <= runtimeChoices.runTimeLoopCount; i++)
        {
            //Add sprite of player (unchanging)
            //Add sprite of enemy (flip it upside down to indicate death)
            //Add item gained by player, potentially next to the player-sprite.

            EndSceneVisuals instance = Instantiate(endSceneVisuals, transform);
            instance.playerImages[0].sprite = playerSpr;
            instance.playerImages[1].sprite = playerSpr;

            instance.enemyImage.sprite = runtimeChoices.enemies[i - 1].sprite;
            instance.itemImage.sprite = runtimeChoices.playerItems[i - 1].itemSprite;

            instance.GetComponent<TextMeshProUGUI>().text = "EncounterTextPlaceholder";


        }
        Instantiate(creditsText, transform); //Credits text..   

    }

    private void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        baseText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}
