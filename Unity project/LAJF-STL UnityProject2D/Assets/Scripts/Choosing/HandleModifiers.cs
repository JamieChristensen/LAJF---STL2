using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleModifiers : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Image> Sprites;
    public List<TextMeshProUGUI> names;
    public List<EnemyModifier> ModifierPool;

    //public Enemy enemy1;
    //public Enemy enemy2;
    //public Enemy enemy3;

    private const int DRAFT_CHOICES = 3;
    // Start is called before the first frame update
    void Start()
    {
        // we might want to access the pool of unused creatures
        ShuffleList(ModifierPool);
        for (int i = 0; i < DRAFT_CHOICES; i++)
        {
            EnemyModifier modifier = ModifierPool[i];
            Sprites[i].sprite = modifier.sprite;
            names[i].text = modifier.name;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShuffleList(List<EnemyModifier> ts)
    {
        for (int i = 0; i < ts.Count; i++)
        {
            EnemyModifier temp = ts[i];
            int randomIndex = Random.Range(i, ts.Count);
            ts[i] = ts[randomIndex];
            ts[randomIndex] = temp;
        }
    }
}

