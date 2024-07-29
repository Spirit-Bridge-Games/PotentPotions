using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    public ColorVariable handColor;
    public float colorChangeDuration;
    public ColorListVariable potionColor;
    public FloatVariable bossHealth;
    public float handHealth;

    private SpriteRenderer sprite;
    private float colorChangeTimer;

    [SerializeField] private List<Color> colors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        ChangeColor();
        bossHealth.Value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Timers();

        if(colorChangeTimer >= colorChangeDuration)
        {
            colorChangeTimer = 0;
            ChangeColor();
        }

        if(handHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ChangeColor()
    {
        int rand = Random.Range(0, colors.Count);

        handColor.Value = colors[rand];
        sprite.color = handColor.Value;
    }

    private void Timers()
    {
        colorChangeTimer += Time.deltaTime;
    }

    public void OnHit(Color color)
    {
        if(handColor.Value == color)
        {
            bossHealth.Value -= 5;
            handHealth -= 1;
        }
    }
}
