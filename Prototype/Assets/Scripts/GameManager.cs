using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform gameOverScreen;
    public RectTransform winScreen;
    
    [Header("Other References")]
    public PlayerController playerController;
    public Npc[] npcs;

    private bool isGameResultDecided = false;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckVictoryCondition()
    {
        if (isGameResultDecided) return;

        foreach (Npc npc in npcs)
        {
            if (!npc.healed)
            {
                return;
            }
        }

        TriggerWin();
    }

    private void TriggerWin()
    {
        if (isGameResultDecided) return;
        
        isGameResultDecided = true;
        winScreen.gameObject.SetActive(true);

        if (playerController != null)
        {
            playerController.cantMove = true;
        }
    }

    public void GameOver()
    {
        if (isGameResultDecided) return;
        
        isGameResultDecided = true;
        gameOverScreen.gameObject.SetActive(true);

        if (playerController != null)
        {
            playerController.cantMove = true;
        }
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
