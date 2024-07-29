using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    public FloatVariable deadZone;
    public FloatVariable playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.Value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < deadZone.Value)
        {
            playerHealth.Value = 0;
        }

        if(playerHealth.Value <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyKiller>() != null)
        {
            playerHealth.Value -= 50;
        }
        if(collision.gameObject.GetComponent<Spike>() != null)
        {
            playerHealth.Value = 0;
        }
    }

    public void Damage(int damage)
    {
        playerHealth.Value -= damage;
    }
}
