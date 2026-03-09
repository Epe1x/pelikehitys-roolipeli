using System.Collections.Generic;
using UnityEngine;

public class Reppu
{
    public float maxWeight = 20f;
    public float maxVolume = 20f;

    private List<Tavara> items = new List<Tavara>();

    float currentWeight = 0;
    float currentVolume = 0;

    public bool AddItem(Tavara item)
    {
        if (currentWeight + item.weight > maxWeight)
            return false;

        if (currentVolume + item.volume > maxVolume)
            return false;

        items.Add(item);

        currentWeight += item.weight;
        currentVolume += item.volume;

        return true;
    }

    public List<Tavara> GetItems()
    {
        return items;
    }
}