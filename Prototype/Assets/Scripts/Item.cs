using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    public string itemName;

    [Header("Trading")]
    public int price;
    
    [Header("UI Visuals")]
    public Sprite uiIcon;
    
    [Header("Settings")]
    public GameObject prefab;
    public bool isStackable;
    public int maxStackSize = 10;
    
    public Color itemColor = Color.white;

    public bool isSeed = false;
    public GameObject grownPlantPrefab;
    public float growthTime;
}
