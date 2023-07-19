using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public float timeStart;
    public float timeEnd;
    public int amount;
    public int idSpawn;
    public Item(Item d)
    {
        timeStart = d.timeStart;
        timeEnd = d.timeEnd;
        amount = d.amount;
        idSpawn = d.idSpawn;
    }
}
