using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public string merchantName = "Peter";
    public List<Item> itemsToSell = new List<Item>();

    public void InteractWithMerchant(MerchantUIController uiController)
    {
        uiController.OpenMerchantWindow(this);
    }
    
    public void BuyItem(int itemIndex, Inventory playerInv)
    {
        if (itemIndex < 0 || itemIndex >= itemsToSell.Count) return;
        
        Item item = itemsToSell[itemIndex];

        if (playerInv.money >= item.price)
        {
            playerInv.AddItem(item);
            playerInv.money -= item.price;
            
            Debug.Log($"Bought {item.itemName}");
        }
        else
        {
            Debug.Log($"Not enough money");
        }
    }

    public void SellSelectedItem(Inventory playerInv)
    {
        if (playerInv.slots.Count == 0 || playerInv.selectedSlotIndex >= playerInv.slots.Count) return;

        int index = playerInv.selectedSlotIndex;
        Item item = playerInv.slots[index].itemData;
        
        playerInv.money += item.price;
        playerInv.RemoveSelectedItem();
        Debug.Log($"Sold {item.itemName}");
    }
}
