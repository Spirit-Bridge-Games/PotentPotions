using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSplashDestroyer : MonoBehaviour
{
    public float destroyDuration;

    private float destroyTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;

        if(destroyTimer > destroyDuration )
        {
            Destroy(gameObject);
        }
    }

    
}
