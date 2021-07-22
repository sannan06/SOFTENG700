using System.Collections.Generic;
using UnityEngine;

public class BlockRow : MonoBehaviour
{
    private List<Block> blocks;

    void Awake()
    {
        this.blocks = new List<Block>();
    }

    public bool CanFitBlock(Block block)
    {
        Fraction sum = this.FractionalTotal() + block.fraction;
        return sum.numerator <= sum.denominator;
    }

    public void AddBlock(Block block)
    {
        // Add Block as child of BlockRow
        block.transform.SetParent(this.transform, false);

        Destroy(block.GetComponent<Rigidbody>());

        // Get x position for new block
        float rowTotal = this.FractionalTotal().ToDecimal();
        float xOffset = rowTotal / 2;

        // Add block to that position
        block.transform.localPosition = new Vector3(xOffset, 0, 0);

        this.blocks.Add(block);
    }

    public bool HasBlock(Block block)
    {
        return this.blocks.Contains(block);
    }

    Fraction FractionalTotal()
    {
        Fraction total = "0/1";
        foreach (Block block in blocks)
        {
            total += block.fraction;
        }

        return total;
    }
}