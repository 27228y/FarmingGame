using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RectTransform gameOverScreen;
    public RectTransform winScreen;
    public PlayerController playerController;
    public Npc[] npcs;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        foreach (Npc npc in npcs)
        {
            if (!npc.healed)
            {
                return;
            }
        }
        
        winScreen.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        playerController.cantMove = true;
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
