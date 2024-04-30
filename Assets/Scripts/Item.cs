using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public Sprite icon;

    public Item(int id, string name, Sprite icon)
    {
        this.id = id;
        this.name = name;
        this.icon = icon;
    }
}
