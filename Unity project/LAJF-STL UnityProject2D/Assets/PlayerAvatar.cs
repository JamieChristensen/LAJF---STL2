using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour
{
    public ChoiceCategory runtimeChoices;
    public Image playerAvatarImage;

    void Start()
    {
        playerAvatarImage = GetComponent<Image>();
        playerAvatarImage.sprite = FindObjectOfType<P1Controller>().playerSprite;
        gameObject.SetActive(true);
    }


}
