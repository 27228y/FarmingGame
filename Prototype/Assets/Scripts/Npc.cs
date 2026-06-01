using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Npc : MonoBehaviour
{
    [Header("References")]
    public Canvas npcCanvas;
    public TextMeshProUGUI uiText;
    public PlayerController player;
    public RectTransform healthBar;
    
    [Header("Settings")]
    public int reward = 10;
    public string askPhrase;
    public string thanksPhrase;
    public Color neededColor;
    public float liveTime = 120f;
    public float maxHp = 100f;
    
    [Header("Movement Settings")]
    public Animator animator;
    public float walkRadius = 10f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    [Header("Stuck Detection")]
    public float stuckThresholdTime = 1.5f;
    private float stuckTimer = 0f;

    private NavMeshAgent agent;
    private float waitTimer;
    private bool isWaiting;
    private bool closeToPlayer = false;
    private Vector3 spawnPosition;
    private float hp = 100f;
    private Vector2 defaultScale;
    [HideInInspector] public bool healed = false;
    private bool isDead = false;
    
    void Start()
    {
        spawnPosition = transform.position;
        
        uiText.text = askPhrase;
        npcCanvas.enabled = false;
        agent = GetComponent<NavMeshAgent>();

        MoveToRandomPoint();

        defaultScale = healthBar.sizeDelta;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (player.interactionDistance * 1.2f >= distanceToPlayer)
        {
            npcCanvas.enabled = true;
            closeToPlayer = true;
            agent.isStopped = true;
            
            Vector3 lookDirection = player.transform.position - transform.position;
            lookDirection.y = 0;
            
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
            }
        }
        else
        {
            npcCanvas.enabled = false;
            
            if (closeToPlayer)
            {
                closeToPlayer = false;
                agent.isStopped = false;
                stuckTimer = 0f;
            }
        }

        if (!closeToPlayer)
        {
            HandleRandomWalk();
        }
        
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        changeHealth();
    }

    public bool HelpNpc(Item item)
    {
        Vector3 currentRgb = new Vector3(item.itemColor.r, item.itemColor.g, item.itemColor.b);
        Vector3 targetRgb = new Vector3(neededColor.r, neededColor.g, neededColor.b);
        
        float distance = Vector3.Distance(currentRgb, targetRgb);

        if (distance <= 0.25f && !healed)
        {
            uiText.text = thanksPhrase;
            healed = true;
            agent.isStopped = false;
            
            GameManager.Instance.CheckVictoryCondition();
            
            
            return true;
        }
        return false;
    }

    private void HandleRandomWalk()
    {
        bool isTryingToMove = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;

        if (isTryingToMove)
        {
            if (agent.velocity.sqrMagnitude < 0.1f)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= stuckThresholdTime)
                {
                    stuckTimer = 0f;
                    agent.ResetPath();
                    MoveToRandomPoint();
                    return;
                }
            }
            else
            {
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
            
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    isWaiting = false;
                    MoveToRandomPoint();
                }
            }
        }
    }

    private void MoveToRandomPoint()
    {
        int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts) {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += spawnPosition;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius * 0.5f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(hit.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
            
            attempts++;
        }
        
        agent.SetDestination(spawnPosition);
    }

    private void changeHealth()
    {
        if (healed || isDead) return;
        
        float damagePerSecond = maxHp / liveTime;
        hp -= damagePerSecond * Time.deltaTime;

        if (hp <= 0f)
        {
            GameManager.Instance.GameOver();
            agent.isStopped = true;
            isDead = true;
            hp = 0;
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.sizeDelta = new Vector2(defaultScale.x * hp / maxHp, defaultScale.y);
    }
}
