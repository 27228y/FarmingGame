using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item itemData;
    public int count;
    
    public InventorySlot(Item itemData, int count)
    {
        this.itemData = itemData;
        this.count = count;
    }
}

public class Inventory : MonoBehaviour
{
    public int money = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 5;
    public int selectedSlotIndex = 0;
    public InventoryUI inventoryUI;

    public bool AddItem(Item newItem)
    {
        if (newItem == null) return false;
        
        if (newItem.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.itemData == newItem && slot.count < newItem.maxStackSize)
                {
                    slot.count++;
                    
                    if (inventoryUI != null) inventoryUI.UpdateInventoryDisplay();
                    return true;
                }
            }
        }

        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(newItem, 1));
            
            if (inventoryUI != null) inventoryUI.UpdateInventoryDisplay();
            return true;
        }
        
        return false;
    }

    public void ChangeSelection(int direction)
    {
        selectedSlotIndex += direction;
        
        if (selectedSlotIndex < 0) selectedSlotIndex = maxSlots - 1;
        if (selectedSlotIndex >= maxSlots) selectedSlotIndex = 0;
    }

    public void RemoveSelectedItem()
    {
        if (slots.Count > 0 && selectedSlotIndex < slots.Count)
        {
            slots[selectedSlotIndex].count--;

            if (slots[selectedSlotIndex].count <= 0)
            {
                slots.RemoveAt(selectedSlotIndex);
                
                if (selectedSlotIndex >= slots.Count && slots.Count > 0)
                {
                    selectedSlotIndex = slots.Count - 1;
                }
            }
        }
    }
}
