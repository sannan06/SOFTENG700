using UnityEngine;
using System.Collections.Generic;

public class BlockGroup : MonoBehaviour
{
    private List<BlockRow> rows;

    void Awake()
    {
        rows = new List<BlockRow>();

        // Create 1 row with 1/1 block
    }

    void AddBlock(Block block)
    {
        bool insertedBlock = false;
        foreach (BlockRow row in rows)
        {
            if (!row.IsFull())
            {
                if (row.CanFitBlock(block))
                {
                    // Get max position of row
                    // Add block to that position
                    // Add block as child of that row
                }
            }
        }

        if (!insertedBlock)
        {
            // If no existing Row could fit block, create new Row
        }
    }
}