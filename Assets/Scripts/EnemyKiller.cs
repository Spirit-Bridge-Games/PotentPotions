using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{
    public Color spriteColor;
    public List<Color> spriteColors;

    // Start is called before the first frame update
    void Start()
    {
        spriteColor = spriteColors[Random.Range(0,spriteColors.Count)];

        GetComponent<SpriteRenderer>().color = spriteColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKill(Color color)
    {
        if(spriteColor == color)
        {
            Destroy(gameObject);
        }
    }
}
