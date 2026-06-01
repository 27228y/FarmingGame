using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Item itemData;
    public bool canBeCollected = true;
    
    private Renderer myRenderer;
    private static MaterialPropertyBlock propBlock;
    private static int baseColorShaderId;

    private void Start()
    {
        myRenderer = GetComponentInChildren<Renderer>();
        
        if (baseColorShaderId == 0)
        {
            baseColorShaderId = Shader.PropertyToID("_BaseColor");
        }
        
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
        if (itemData == null || myRenderer == null) return;
        
        if (propBlock == null) propBlock = new MaterialPropertyBlock();
        
        myRenderer.GetPropertyBlock(propBlock);
        
        propBlock.SetColor(baseColorShaderId, itemData.itemColor);
        myRenderer.SetPropertyBlock(propBlock);
    }
}
