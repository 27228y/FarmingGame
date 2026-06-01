using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Header("Settings")]
    public Color waterColor;
    
    [Header("References")]
    public Sprite defaultPotionIcon;
    public Renderer liquidRenderer;
    public ParticleSystem splashParticleSystem;
    public Transform spawnPoint;
    public GameObject potionPrefab;
    
    private List<Item> ingredients = new List<Item>();
    private MaterialPropertyBlock propBlock;
    private int baseColorShaderId;

    void Start()
    {
        propBlock = new MaterialPropertyBlock();
        baseColorShaderId = Shader.PropertyToID("_BaseColor");
        
        UpdateLiquidColor(waterColor);
    }

    public void AddIngredient(Item item)
    {
        if (item == null) return;
        
        ingredients.Add(item);
        
        if (liquidRenderer == null) return;

        UpdateLiquidColor(CalculateAverageColor());
    }

    public void FinishCooking()
    {
        if (ingredients.Count == 0) return;

        float r = 0, g = 0, b = 0;
        foreach (var ing in ingredients)
        {
            r += ing.itemColor.r;
            g += ing.itemColor.g;
            b += ing.itemColor.b;
        }
        
        Color finalColor = new Color(r / ingredients.Count, g / ingredients.Count, b / ingredients.Count);
        
        GameObject potion = Instantiate(potionPrefab, spawnPoint.position, Quaternion.identity);
        
        potion.layer = LayerMask.NameToLayer("Interactable");
        
        Collectible col = potion.GetComponent<Collectible>();
        if (col != null)
        {
            Item mixedItem = ScriptableObject.CreateInstance<Item>();
            mixedItem.itemName = "Potion";
            mixedItem.itemColor = finalColor;

            mixedItem.uiIcon = defaultPotionIcon;
            mixedItem.itemColor = finalColor;
            col.itemData = mixedItem;
        }
        
        ingredients.Clear();
        
        UpdateLiquidColor(waterColor);
    }

    private void UpdateLiquidColor(Color targetColor)
    {
        if (liquidRenderer == null) return;
        
        liquidRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(baseColorShaderId, targetColor);
        liquidRenderer.SetPropertyBlock(propBlock);
        
        if (splashParticleSystem == null) return;
        
        var mainModule = splashParticleSystem.main;
        mainModule.startColor = targetColor;
    }

    private Color CalculateAverageColor()
    {
        if (ingredients.Count == 0) return waterColor;

        float r = 0, g = 0, b = 0;
        foreach (Item i in ingredients)
        {
            r += i.itemColor.r;
            g += i.itemColor.g;
            b += i.itemColor.b;
        }
        
        return new Color(r / ingredients.Count, g / ingredients.Count, b / ingredients.Count, 1.0f);
    }
}
