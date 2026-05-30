using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSlotUI : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI priceText;

    public Button slotMainButton;

    private int myStoredIndex;
    private MerchantUIController uiController;

    public void SetupSlot(Item item, int index, MerchantUIController manager)
    {
        myStoredIndex = index;
        uiController = manager;

        if (itemIconImage != null && item.uiIcon != null)
        {
            itemIconImage.sprite = item.uiIcon;
            
            itemIconImage.gameObject.SetActive(true);
        }

        if (priceText != null)
        {
            priceText.text = $"{item.price}$";
        }

        if (slotMainButton == null)
        {
            slotMainButton = GetComponent<Button>();
        }

        if (slotMainButton != null)
        {
            slotMainButton.onClick.RemoveAllListeners();
            slotMainButton.onClick.AddListener(OnSlotClicked);
        }
    }

    private void OnSlotClicked()
    {
        if (uiController != null && uiController.activeMerchant != null)
        {
            uiController.activeMerchant.BuyItem(myStoredIndex, uiController.playerInventory);

            FindObjectOfType<InventoryUI>()?.UpdateInventoryDisplay();
        }
    }
}
