using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

    [SerializeField]
    private string[] coolDescriptors = { "gloriously", "bravely", "righteously", 
        "in a rage induced haze", "with great judgement", "in a cool looking manner",
        "with little deliberation", "ruthlessly", "forcefully"
    }; 

    [SerializeField]
    private string[] killingWords = {
        "slayed", "defeated", "overcame", "ended the existence of", "razed", "disgraced", "coughed on"
    };

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
            
            string randomDescriptor = coolDescriptors[Random.Range(0, coolDescriptors.Length)];
            string randomKillingWord = killingWords[Random.Range(0, killingWords.Length)];

            List<EnemyModifier> mods = new List<EnemyModifier>();
            mods.Add(runtimeChoices.enemyModifiers[i-1]);

            EndSceneVisuals instance = Instantiate(endSceneVisuals, transform);
            TextMeshProUGUI tmpText = instance.gameObject.GetComponent<TextMeshProUGUI>();
            tmpText.text = "The " + runtimeChoices.chosenHero.myName + ", " + randomDescriptor + " " + 
            randomKillingWord + " a " + runtimeChoices.enemies[i-1].GenerateName(mods);

            
            instance.playerImages[0].sprite = playerSpr;
            instance.playerImages[1].sprite = playerSpr;

            instance.environment.sprite = runtimeChoices.chosenEnvironments[i - 1].environmentSprite;
            instance.enemyImage.sprite = runtimeChoices.enemies[i - 1].sprite;
            instance.itemImage.sprite = runtimeChoices.playerItems[i - 1].itemSprite;




        }
        Instantiate(creditsText, transform); //Credits text..   

    }

    private void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        baseText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}
