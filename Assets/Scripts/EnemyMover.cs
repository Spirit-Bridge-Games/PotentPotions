using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class EnemyMover : MonoBehaviour
{
    public Vector3Variable playerPos;
    public FloatVariable speed;
    public float distanceFromPlayer;

    public bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, playerPos.Value) <= distanceFromPlayer)
        {
            Move();
        }
    }

    private void Move()
    {
        if (isFacingRight)
        {
            transform.Translate(transform.right * speed.Value * Time.deltaTime);
        }
        else
        {
            transform.Translate(-transform.right * speed.Value * Time.deltaTime);
        }
    }

    public void Rotate()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
    }
}
