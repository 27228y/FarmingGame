using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject slotPrefab;
    public Transform slotParent;
    public TextMeshProUGUI moneyText;

    private List<InventorySlotUI> visualSlots = new List<InventorySlotUI>();

    void Start()
    {
        for (int i = 0; i < playerInventory.maxSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotRef = newSlot.GetComponent<InventorySlotUI>();
            visualSlots.Add(slotRef);
        }
    }

    public void UpdateInventoryDisplay()
    {
        if (playerInventory == null) return;
        
        moneyText.text = $"{playerInventory.money} $";
        
        for (int i = 0; i < visualSlots.Count; i++)
        {
            InventorySlotUI slot = visualSlots[i];
            
            if (slot == null) continue;

            if (slot.selectionFrame != null)
            {
                slot.selectionFrame.gameObject.SetActive(i == playerInventory.selectedSlotIndex);
            }

            if (i < playerInventory.slots.Count)
            {
                Item data = playerInventory.slots[i].itemData;

                if (slot.itemIconImage != null && data != null && data.uiIcon != null)
                {
                    slot.itemIconImage.sprite = data.uiIcon;
                    
                    slot.itemIconImage.color = (data.itemName == "Potion") ? data.itemColor : Color.white;
                    slot.itemIconImage.gameObject.SetActive(true);
                }

                int amount = playerInventory.slots[i].count;
                if (slot.countText != null)
                {
                    slot.countText.text = amount > 1 ? amount.ToString() : "";
                }
            }
            else
            {
                if (slot.itemIconImage != null)
                {
                    slot.itemIconImage.sprite = null;
                    slot.itemIconImage.gameObject.SetActive(false);
                }

                if (slot.countText != null)
                {
                    slot.countText.text = "";
                }
            }

        }
    }
}