using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public string merchantName = "Peter";
    public List<Item> itemsToSell = new List<Item>();
    public InventoryUI inventoryUI;

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
            if (playerInv.AddItem(item)) {
                playerInv.money -= item.price;
                inventoryUI.UpdateInventoryDisplay();

                Debug.Log($"Bought {item.itemName}");
            }
        }
        else
        {
            Debug.Log($"Not enough money");
        }
    }

}
