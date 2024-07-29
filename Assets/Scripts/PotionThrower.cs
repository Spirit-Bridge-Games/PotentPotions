using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PotionThrower : MonoBehaviour
{
    public Transform cannonTransform;
    public FloatVariable throwStrength;
    public ColorListVariable colorListVariable;

    public GameObject potion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnThrow(CallbackContext context)
    {
        if (context.started)
        {
            //GameObject foundObj = GameObject.FindGameObjectWithTag("Potion");
            //if (foundObj == null)
            //{
                if (colorListVariable.GetColor() != Color.black)
                {
                    GameObject obj = Instantiate(potion, cannonTransform.position, Quaternion.identity);
                    obj.GetComponent<SpriteRenderer>().color = colorListVariable.GetColor();
                    obj.GetComponent<PotionRemover>().potionColor = colorListVariable.GetColor();
                    obj.GetComponent<PotionRemover>().isBossPotion = false;

                    if (transform.localScale.x > 0)
                        obj.GetComponent<Rigidbody2D>().AddForce(transform.right * throwStrength.Value, ForceMode2D.Impulse);

                    colorListVariable.RemoveAll();
                }
            //}
        }
    }
}
