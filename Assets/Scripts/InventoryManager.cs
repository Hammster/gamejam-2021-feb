using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<InventorySlot> inventorySlots;
  
    public void EnableItem(TileScriptableObject tileScriptableObject)
    {
        foreach (var inventorySlot in inventorySlots)
        {
            if (inventorySlot.slotType == tileScriptableObject)
            {
                inventorySlot.EnableItem();
            }
        }
    }
    
    public void DisableItem(TileScriptableObject tileScriptableObject)
    {
        foreach (var inventorySlot in inventorySlots)
        {
            if (inventorySlot.slotType == tileScriptableObject )
            {
                inventorySlot.DisableItem();
            }
        }
    }
}
