using System.Collections.Generic;
using UnityEngine;

public class BlockRow : MonoBehaviour
{
    private List<Block> blocks;

    void Awake()
    {
        this.blocks = new List<Block>();
    }

    public bool IsFull()
    {
        return this.Total() == 1;
    }

    public bool CanFitBlock(Block block)
    {
        return this.Total() + block.fraction.ToDecimal() < 1;
    }

    double Total()
    {
        double total = 0;
        foreach (Block block in blocks)
        {
            total += block.fraction.ToDecimal();
        }

        return total;
    }
}