using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetColorSet : MonoBehaviour
{
    public ColorListVariable colorListVariable;

    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        colorListVariable.RemoveAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (colorListVariable.Items.Count > 0)
        {
            foreach (Color color in colorListVariable.Items)
            {
                image.color += new Color(color.r,color.g,color.b);
            }
        }
        else
        {
            image.color = new Color(0,0,0,0.5f);
        }
    }
}
