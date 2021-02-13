using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryManager : MonoBehaviour
{
    private static readonly InventoryManager instance = new InventoryManager();    
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

    public static InventoryManager GetInstance() {
        return instance;
    }
    static InventoryManager() {
    }
    private InventoryManager() {
    }
}
