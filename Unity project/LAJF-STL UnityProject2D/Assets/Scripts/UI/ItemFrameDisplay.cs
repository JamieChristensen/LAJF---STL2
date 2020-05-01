using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrameDisplay : MonoBehaviour
{
    [SerializeField] private int _itemFrameNumber;
    public ChoiceCategory runTimeChoices;
    public GameObject itemSocketFilledBackground, itemSocketDefaultBackground;
    public Image itemImage;
    public Sprite cage;
    private void Start()
    {
        if (_itemFrameNumber == 0)
        {
            itemSocketFilledBackground.SetActive(true);
            itemSocketDefaultBackground.SetActive(false);
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = runTimeChoices.baselineItem.itemSprite;
        }
    }

    public void OnHeroPickedItem()
    {

        if (runTimeChoices.runTimeLoopCount == _itemFrameNumber)
        {
            itemSocketFilledBackground.SetActive(true);
            itemSocketDefaultBackground.SetActive(false);
            itemImage.gameObject.SetActive(true);

            if (runTimeChoices.playerItems.Count != 0 && runTimeChoices.playerItems != null)
            {
                Debug.Log("ItemFrame runtime playerItemsList: ");
                foreach (PlayerItems item in runTimeChoices.playerItems)
                {
                    Debug.Log(item.ToString());
                }
                itemImage.sprite = cage;

                int clampedSpriteIndex = Mathf.Clamp(_itemFrameNumber - 1, 0, runTimeChoices.playerItems.Count - 1);
                itemImage.sprite = runTimeChoices.playerItems[clampedSpriteIndex].itemSprite;
                return;
            }
            else
            {
                itemImage.sprite = cage;
                return;
            }
        }


    }




}
