using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPotionShooter : MonoBehaviour
{
    public GameObject potion;
    public ColorVariable handColor;
    public FloatVariable throwStrength;
    public Transform cannon;
    public float rayLength;


    private RaycastHit2D hit2D;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(cannon.position, -transform.right * rayLength);
    }

    private void FirePotion()
    {
        GameObject obj = Instantiate(potion, transform.position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().color = handColor.Value;
        obj.GetComponent<PotionRemover>().potionColor = handColor.Value;
        obj.GetComponent<PotionRemover>().isBossPotion = true;

        obj.GetComponent<Rigidbody2D>().AddForce(-transform.right * throwStrength.Value, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FirePotion();
        }
    }
}
