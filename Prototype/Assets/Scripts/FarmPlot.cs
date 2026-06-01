using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{

    public enum PlotState { Empty, Growing, Ready }
    public PlotState currentState = PlotState.Empty;
    public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    
    public AudioClip grownClip;
    private Item seedData;
    private float timer;
    private GameObject currentPlant;
    private float initialScale;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (currentState == PlotState.Growing && currentPlant != null)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / seedData.growthTime);
            float scale = Mathf.Lerp(0f, initialScale, progress);
            currentPlant.transform.localScale = Vector3.one * scale;

            if (progress >= 1f)
            {
                FinishGrowth();

                audioSource.PlayOneShot(grownClip);
            }
        }
    }
    
    public void Plant(Item seed)
    {
        if (currentState != PlotState.Empty) return;
        
        seedData = seed;
        timer = 0f;
        currentState = PlotState.Growing;
        currentPlant = Instantiate(seedData.grownPlantPrefab, transform.position + spawnOffset, Quaternion.identity);
        initialScale = currentPlant.transform.localScale.x;
        currentPlant.transform.localScale = Vector3.one * 0.01f;
        if (currentPlant.TryGetComponent(out Collectible collectible)) collectible.canBeCollected = false;
    }

    void FinishGrowth()
    {
        currentState = PlotState.Ready;
        if (currentPlant.TryGetComponent(out Collectible collectible)) collectible.canBeCollected = true;
        currentPlant.layer = LayerMask.NameToLayer("Interactable");
        currentPlant.transform.SetParent(null);
        
        // TODO: should not be able to plant before previous plant is removed
        currentState = PlotState.Empty;
    }
} 