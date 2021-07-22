using System.Collections.Generic;

public class BlockGroup : MonoBehavior
{
    private List<BlockRow> rows;

    void Awake()
    {
        rows = new List<BlockRow>();

        // Create 1 row with 1/1 block
    }

    void AddBlock()
    {
        // For Row row : rows
            // If row is not full
                // If row.currentTotal + block.value < 1
                    // Get max position of row
                    // Add block to that position
                // End if
            // End if
        // End for

        // If no existing Row could fit block, create new Row
    }
}