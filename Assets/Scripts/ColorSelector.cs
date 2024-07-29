using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public ColorListVariable colorListVariable;

    public void OnRedSelected()
    {
        if(Time.timeScale > 0)
            colorListVariable.Add(Color.red);
    }
    public void OnGreenSelected()
    {
        if (Time.timeScale > 0)
            colorListVariable.Add(Color.green);
    }
    public void OnBlueSelected()
    {
        if (Time.timeScale > 0)
            colorListVariable.Add(Color.blue);
    }

    public void OnClearPotion()
    {
        if (Time.timeScale > 0)
            colorListVariable.RemoveAll();
    }
}
