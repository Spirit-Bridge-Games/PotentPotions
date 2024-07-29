using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorListVariable : ScriptableObject
{
    public List<Color> Items = new();

    public void Add(Color color)
    {
        if (!Items.Contains(color))
        {
            Items.Add(color);
        }
    }

    public void Remove(Color color)
    {
        if(Items.Contains(color))
        {
            Items.Remove(color);
        }
    }

    public void RemoveAll()
    {
        Items.Clear();
    }

    public Color GetColor()
    {
        Color tempColor = Color.black;
        foreach(var item in Items)
        {
            tempColor.r += item.r;
            tempColor.g += item.g;
            tempColor.b += item.b;
        }
        return tempColor;
    }
}

