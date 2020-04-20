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

    public void OnHeroPickedItem()
    {
        if (runTimeChoices.runTimeLoopCount == _itemFrameNumber)
        {
            itemSocketFilledBackground.SetActive(true);
            itemSocketDefaultBackground.SetActive(false);
            itemImage.gameObject.SetActive(true);
            if (runTimeChoices.playerItems.Count != 0)
            {
                itemImage.sprite = cage;
                //itemImage.sprite = runTimeChoices.playerItems[_itemFrameNumber - 1].itemSprite;
            }
            else
            {
                itemImage.sprite = cage;
            }
        }
    }
}
