using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionRemover : MonoBehaviour
{
    public FloatVariable deadzone;
    public FloatVariable playerHealth;
    public FloatVariable bossHealth;
    public Color potionColor;
    public bool isBossPotion;
    public GameObject potionSplash;

    private void Update()
    {
        if(transform.position.y < deadzone.Value)
        {
            DestroyAndCreateSplash();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (isBossPotion)
            {
                playerHealth.Value -= 10;
                DestroyAndCreateSplash();
            }
        }
        else if (collision.gameObject.GetComponent<EnemyKiller>() != null)
        {
            collision.gameObject.GetComponent<EnemyKiller>().OnKill(potionColor);
            DestroyAndCreateSplash();
        }
        else if (collision.gameObject.GetComponent<BossHand>() != null)
        {
            collision.gameObject.GetComponent<BossHand>().OnHit(potionColor);
            DestroyAndCreateSplash();
        }
        else if(collision.gameObject.layer == 6)
        {
            DestroyAndCreateSplash();
        }
        else
        {
            DestroyAndCreateSplash();
        }
    }

    private void DestroyAndCreateSplash()
    {
        var obj = Instantiate(potionSplash, transform.position, Quaternion.identity);
        obj.GetComponent<ParticleSystemRenderer>().material.color = potionColor;
        obj.GetComponent<ParticleSystem>().Play();

        Destroy(gameObject);
    }
}
