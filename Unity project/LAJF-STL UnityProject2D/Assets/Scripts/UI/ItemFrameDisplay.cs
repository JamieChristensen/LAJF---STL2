using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFrameDisplay : MonoBehaviour
{
    [SerializeField] private int _itemFrameNumber;
    public ChoiceCategory runTimeChoices;
    public GameObject itemSocketFilledBackground, itemSocketDefaultBackground;
    public SpriteRenderer spriteRenderer;

    public void OnHeroPickedItem()
    {
        if (runTimeChoices.runTimeLoopCount == _itemFrameNumber)
        {
            itemSocketFilledBackground.SetActive(true);
            itemSocketDefaultBackground.SetActive(false);
            spriteRenderer.sprite = runTimeChoices.playerItems[_itemFrameNumber - 1].itemSprite;
        }
    }
}
