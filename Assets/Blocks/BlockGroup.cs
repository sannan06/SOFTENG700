using UnityEngine;
using System.Collections.Generic;

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

    void OnCollisionEnter(Collision collision)
    {
        Block block = collision.gameObject.GetComponent<Block>();
        this.AddBlock(block);
    }

    void AddBlock(Block block)
    {
        bool insertedBlock = false;
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
            if (rows.Count != 0)
            {
                BlockRow finalRow = rows[rows.Count - 1];
                yOffset = finalRow.transform.localPosition.y + 0.1f;
            }
            Vector3 vector3 = new Vector3(0, yOffset, 0);
            BlockRow row = Instantiate(rowPrefab, vector3, Quaternion.identity);
            row.AddBlock(block);

            row.transform.SetParent(this.transform, false);
            this.rows.Add(row);
        }
    }
}