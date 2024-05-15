using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer
{
    private Items fertilizableSeed;

    public Fertilizer(Items fertilizableSeed)
    {
        this.fertilizableSeed = fertilizableSeed;
    }

    public Items GetFertilizableSeed() { return fertilizableSeed; }
}
