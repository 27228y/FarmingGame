using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Item itemData;
    public bool canBeCollected = true;
    
    private Renderer renderer;
    private static MaterialPropertyBlock propBlock;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        ApplyItemColor();
    }

    public void OnInteract(Inventory playerInventory)
    {
        if (!canBeCollected) return;
        
        if (playerInventory.AddItem(itemData))
        {
            Destroy(gameObject);
        }
    }

    public void ApplyItemColor()
    {
        if (itemData == null || renderer == null) return;
        
        if (propBlock == null) propBlock = new MaterialPropertyBlock();
        
        renderer.GetPropertyBlock(propBlock);
        
        propBlock.SetColor("_BaseColor", itemData.itemColor);
        renderer.SetPropertyBlock(propBlock);
    }
}
