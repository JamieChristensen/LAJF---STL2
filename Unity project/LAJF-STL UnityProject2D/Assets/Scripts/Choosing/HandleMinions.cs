using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleMinions : MonoBehaviour
{
    //public Image sprite1;
    //public Image sprite2;
    //public Image sprite3;

    public List<Image> Sprites;
    public List<TextMeshProUGUI> names;
    public List<Enemy> EnemyPool;

    //public Enemy enemy1;
    //public Enemy enemy2;
    //public Enemy enemy3;

    private const int DRAFT_CHOICES = 3;
    // Start is called before the first frame update
    void Start()
    {
        // we might want to access the pool of unused creatures
        Debug.Log("AM I running??");
        ShuffleList(EnemyPool);
        for(int i = 0; i < DRAFT_CHOICES; i++)
        {
            Enemy enemy = EnemyPool[i];
            Debug.Log(enemy);
            Sprites[i].sprite = enemy.sprite;
            names[i].text = enemy.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShuffleList(List<Enemy> ts)
    {
        for (int i = 0; i < ts.Count; i++)
        {
            Enemy temp = ts[i];
            int randomIndex = Random.Range(i, ts.Count);
            ts[i] = ts[randomIndex];
            ts[randomIndex] = temp;
        }
    }
}
