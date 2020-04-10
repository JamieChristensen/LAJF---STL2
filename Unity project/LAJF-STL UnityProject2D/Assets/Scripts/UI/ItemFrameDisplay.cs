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

    public void OnHeroPickedItem()
    {
        if (runTimeChoices.runTimeLoopCount == _itemFrameNumber)
        {
            itemSocketFilledBackground.SetActive(true);
            itemSocketDefaultBackground.SetActive(false);
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = runTimeChoices.playerItems[_itemFrameNumber - 1].itemSprite;
        }
    }
}
