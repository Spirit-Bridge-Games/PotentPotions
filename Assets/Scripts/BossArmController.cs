using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    public float offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if (isGoingUp)
        //{
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, max), speed);
        //}
        //else
        //{
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, min), speed);
        //}

        transform.eulerAngles = new Vector3(0,0, Mathf.PingPong(Time.time * speed, 30) - offset);

    }
}
