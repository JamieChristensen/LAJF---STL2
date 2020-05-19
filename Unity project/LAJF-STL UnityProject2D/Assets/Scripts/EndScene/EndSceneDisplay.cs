using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class EndSceneDisplay : MonoBehaviour
{
    public ChoiceCategory runtimeChoices;
    public SettingsScrObj gameSettings;

    public MusicManager musicManager;

    public EndSceneVisuals endSceneVisuals;
    private bool didHeroWin = false;

    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float extraSpeedForScroll;

    public GameObject baseText;
    public GameObject creditsText;

    public KeyCode[] keysThatCanSpeedUpScene;

    [SerializeField]
    private string[] coolDescriptors = { "gloriously", "bravely", "righteously",
        "in a rage induced haze", "with great judgement", "in a cool looking manner",
        "with little deliberation", "ruthlessly", "forcefully"
    };

    [SerializeField]
    private string[] killingWords = {
        "slayed", "defeated", "overcame", "ended the existence of", "razed", "disgraced", "coughed on"
    };

    [SerializeField]
    private string[] heroDeathWords = {
        "got tired of life while battling", "got a heart-attack while fighting"
    };

    public GameObject victoriousVisuals;

    private void Start()
    {

        foreach (PlayerItems item in runtimeChoices.playerItems)
        {
            didHeroWin = (item.itemName == "Victory Shades") ? true : didHeroWin;
        }

        Sprite playerSpr = runtimeChoices.chosenHero.characterSprite;

        if (!didHeroWin)
        {
            for (int i = 1; i <= runtimeChoices.runTimeLoopCount; i++)
            {
                bool isLastRun = i == runtimeChoices.runTimeLoopCount;

                if (isLastRun)
                {
                    //Display player dying.
                    EndSceneVisuals lossInstance = Instantiate(endSceneVisuals, transform);
                    TextMeshProUGUI tmpText = lossInstance.gameObject.GetComponent<TextMeshProUGUI>();

                    string randomDeath = heroDeathWords[Random.Range(0, heroDeathWords.Length)];
                    List<EnemyModifier> mods = new List<EnemyModifier>();
                    mods.Add(runtimeChoices.enemyModifiers[i - 1]);

                    lossInstance.playerImages[0].sprite = playerSpr;
                    lossInstance.playerImages[0].rectTransform.Rotate(new Vector3(0, 180, 180)); //Flips it around X-axis. 

                    lossInstance.environment.sprite = runtimeChoices.chosenEnvironments[i - 1].environmentSprite;
                    lossInstance.tombstoneSpriteRenderer.gameObject.SetActive(false);
                    lossInstance.enemyImage.sprite = runtimeChoices.enemies[i - 1].sprite;

                    Destroy(lossInstance.itemImage.gameObject);
                    Destroy(lossInstance.chest.gameObject);


                    tmpText.text = "The " + runtimeChoices.chosenHero.myName + ", " + randomDeath + " " + runtimeChoices.enemies[i - 1].aOrAn + " " + runtimeChoices.enemies[i - 1].GenerateName(mods);
                }

                if (!isLastRun)
                {
                    List<EnemyModifier> mods = new List<EnemyModifier>();
                    mods.Add(runtimeChoices.enemyModifiers[i - 1]);
                    string randomDescriptor = coolDescriptors[Random.Range(0, coolDescriptors.Length)];
                    string randomKillingWord = killingWords[Random.Range(0, killingWords.Length)];

                    EndSceneVisuals victoryInstance = Instantiate(endSceneVisuals, transform);
                    TextMeshProUGUI tmpText = victoryInstance.gameObject.GetComponent<TextMeshProUGUI>();
                    tmpText.text = "The " + runtimeChoices.chosenHero.myName + ", " + randomDescriptor + " " +
                    randomKillingWord + " " + runtimeChoices.enemies[i - 1].aOrAn + " " + runtimeChoices.enemies[i - 1].GenerateName(mods);


                    victoryInstance.playerImages[0].sprite = playerSpr;


                    victoryInstance.environment.sprite = runtimeChoices.chosenEnvironments[i - 1].environmentSprite;
                    victoryInstance.tombstoneSpriteRenderer.material.SetTexture("_Texture2D", runtimeChoices.enemies[i - 1].sprite.texture);
                    victoryInstance.enemyImage.gameObject.SetActive(false);
                    victoryInstance.itemImage.sprite = runtimeChoices.playerItems[i - 1].itemSprite;
                }

            }

            VictoriousVisuals vicVis2 = Instantiate(victoriousVisuals, transform).GetComponent<VictoriousVisuals>();
            vicVis2.hero.sprite = playerSpr;
            vicVis2.hero.gameObject.SetActive(false);
            string baseDeityText = "Deities were victorious!";
            for (int j = 0; j < gameSettings.GetAmountOfPlayers() - 1; j++)
            {
                Image img = vicVis2.deities[j];
                vicVis2.aveText[j].text = "AVE \n" + runtimeChoices.chosenGods[j].godName;
                img.gameObject.SetActive(true);
                img.sprite = runtimeChoices.chosenGods[j].topBarIcon;
            }
            vicVis2.textVictorious.text = baseDeityText;
            //Instantiate(creditsText, transform); //Credits text..
            Debug.Log("player lost");
            return;
        }


        //Hero won/Deities lost:
        //MusicManager.PlayMusic();


        //if hero won:
        for (int i = 1; i <= runtimeChoices.runTimeLoopCount; i++)
        {
            //Add sprite of player (unchanging)
            //Add sprite of enemy (flip it upside down to indicate death)
            //Add item gained by player, potentially next to the player-sprite.

            string randomDescriptor = coolDescriptors[Random.Range(0, coolDescriptors.Length)];
            string randomKillingWord = killingWords[Random.Range(0, killingWords.Length)];

            List<EnemyModifier> mods = new List<EnemyModifier>();
            mods.Add(runtimeChoices.enemyModifiers[i - 1]);

            EndSceneVisuals instance = Instantiate(endSceneVisuals, transform);
            TextMeshProUGUI tmpText = instance.gameObject.GetComponent<TextMeshProUGUI>();
            tmpText.text = "The " + runtimeChoices.chosenHero.myName + ", " + randomDescriptor + " " +
            randomKillingWord + " " + runtimeChoices.enemies[i - 1].aOrAn + " " + runtimeChoices.enemies[i - 1].GenerateName(mods);


            instance.playerImages[0].sprite = playerSpr;


            instance.environment.sprite = runtimeChoices.chosenEnvironments[i - 1].environmentSprite;
            instance.tombstoneSpriteRenderer.material.SetTexture("_Texture2D", runtimeChoices.enemies[i - 1].sprite.texture);
            instance.itemImage.sprite = runtimeChoices.playerItems[i - 1].itemSprite;




        }

        VictoriousVisuals vicVis = Instantiate(victoriousVisuals,transform).GetComponent<VictoriousVisuals>();
        vicVis.hero.sprite = playerSpr;
        vicVis.hero.gameObject.SetActive(true);
        vicVis.textVictorious.text = "The hero won! \n AVE \n" + runtimeChoices.chosenHero.myName;
        foreach (Image img in vicVis.deities)
        {
            img.gameObject.SetActive(false);
        }

        //Instantiate(creditsText, transform); //Credits text..   

    }

    private void Update()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GameObject.Find("Main Menu button").GetComponent<Button>().Select();
        }

        float effectiveScrollSpeed = scrollSpeed;

        foreach (KeyCode keycode in keysThatCanSpeedUpScene)
        {
            if (Input.GetKey(keycode))
            {
                effectiveScrollSpeed += extraSpeedForScroll;
            }
        }

        transform.Translate(Vector3.up * effectiveScrollSpeed * Time.deltaTime);
        baseText.transform.Translate(Vector3.up * effectiveScrollSpeed * Time.deltaTime);
    }
}
