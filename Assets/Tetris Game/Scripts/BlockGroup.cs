using UnityEngine;
using System.Collections.Generic;

/**
    Class Block group represents a collection of linear block rows
    and logic to add new blocks
*/
public class BlockGroup : MonoBehaviour
{
    public Block blockPrefab;
    public BlockRow rowPrefab;
    private List<BlockRow> rows;

    void Awake()
    {
        rows = new List<BlockRow>();

        // Create bottom row with 1/1 block
        Block block = Instantiate(blockPrefab);
        this.AddBlock(block);
    }

    /**
        Add block to block groups on collision of a new blocks
    */
    void OnCollisionEnter(Collision collision)
    {
        Block block = collision.gameObject.GetComponent<Block>();
        this.AddBlock(block);
    }

    /**
        Add block to an available block row. If no block row is present (or all is full), then create a new row
    */
    void AddBlock(Block block)
    {
        bool insertedBlock = false;

        // iterate through each row from bottom, adding block if it can fit
        foreach (BlockRow row in rows)
        {
            if (row.HasBlock(block)) return;

            if (row.CanFitBlock(block))
            {
                row.AddBlock(block);
                insertedBlock = true;
                break;
            }
        }

        if (!insertedBlock)
        {
            // If no existing Row could fit block, create new Row
            float yOffset = 0;
            
            // Position new row above the last previous row.
            if (rows.Count != 0)
            {
                BlockRow finalRow = rows[rows.Count - 1];
                yOffset = finalRow.transform.localPosition.y + 0.1f;
            }
            Vector3 vector3 = new Vector3(0, yOffset, 0);

            // Create row and add block to it. 
            BlockRow row = Instantiate(rowPrefab, vector3, Quaternion.identity);
            row.AddBlock(block);

            row.transform.SetParent(this.transform, false);
            this.rows.Add(row);
        }
    }
}