using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MerchantUIController : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject buyPanelWindow;
    public Transform slotContainer;
    public GameObject merchantSlotPrefab;
    
    public Merchant activeMerchant;
    private List<GameObject> spawnedSlots = new List<GameObject>();

    public void OpenMerchantWindow(Merchant merchantTarget)
    {
        activeMerchant = merchantTarget;
        
        buyPanelWindow.SetActive(true);
        ClearOldSlots();

        for (int i = 0; i < merchantTarget.itemsToSell.Count; i++)
        {
            GameObject newSlot = Instantiate(merchantSlotPrefab, slotContainer);
            spawnedSlots.Add(newSlot);
            
            MerchantSlotUI slotUI = newSlot.GetComponent<MerchantSlotUI>();
            if (slotUI != null)
            {
                slotUI.SetupSlot(merchantTarget.itemsToSell[i], i, this);
            }
        }
    }

    public void CloseMerchantWindow()
    {
        buyPanelWindow.SetActive(false);
        ClearOldSlots();
        activeMerchant = null;
    }

    public void ClearOldSlots()
    {
        foreach (var slot in spawnedSlots) Destroy( slot );
        {
            spawnedSlots.Clear();
        }
    }
}
