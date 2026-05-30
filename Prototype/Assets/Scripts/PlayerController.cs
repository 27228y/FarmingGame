using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactableLayer;
    public Animator animator;

    [Header("UI Elements")]
    public MerchantUIController merchantUIWindow;
    
    [Header("Sounds")]
    public AudioClip interactSound;
    public AudioClip completeSound;
    
    private Inventory myInventory;
    private GameObject currentTarget;
    [HideInInspector] public bool cantMove = false;
    private Transform model;
    private Camera cam;
    private AudioSource audioSource;

    void Start()
    {
        myInventory = GetComponent<Inventory>();
        model = transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (cantMove && Input.GetKeyDown(interactKey))
        {
            CloseShop();
            return;
        }
        
        if (cantMove) return;
        
        HandleMovement();
        HandleSelection();
        FindTarget();

        if (Input.GetKeyDown(interactKey))
        {
            HandleInteraction();
        }
    }
    
    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 move = (transform.right * h + transform.forward * v).normalized;
        
        float currentSpeed = move.magnitude;
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
        
        transform.position += move * (speed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * speed * 3);
        }
    }
    
    void FindTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayer);
        GameObject closest = null;
        float shortestDistance = float.MaxValue;

        foreach (var col in cols)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = col.gameObject;
            }
        }

        if (closest != currentTarget)
        {
            if (currentTarget != null) ToggleHighlight(currentTarget, false);
            currentTarget = closest;
            if (currentTarget != null) ToggleHighlight(currentTarget, true);
        }
    }
    
    void ToggleHighlight(GameObject obj, bool state)
    {
        if (obj == null) return;

        if (obj.TryGetComponent(out Outline outline))
        {
            outline.enabled = state;
        }
    }

    void HandleInteraction()
    {
        // Sound effect
        audioSource.PlayOneShot(interactSound);
        
        // Logic
        if (currentTarget == null)
        {
            return;
        }

        if (currentTarget.TryGetComponent(out BouncyObject obj))
        {
            obj.PlayBounceAnimation();
        }

        if (currentTarget.TryGetComponent(out Collectible item))
        {
            item.OnInteract(myInventory);
        }
        else if (currentTarget.TryGetComponent(out Cauldron pot))
        {
            var slots = myInventory.slots;
            int index = myInventory.selectedSlotIndex;
            
            bool holdingItem = index < slots.Count && slots[index].count > 0;

            if (holdingItem)
            {
                pot.AddIngredient(slots[index].itemData);
                myInventory.RemoveSelectedItem();
            }
            else
            {
                pot.FinishCooking();
            }
        }
        else if (currentTarget.TryGetComponent(out FarmPlot plot))
        {
            if (plot.currentState == FarmPlot.PlotState.Empty)
            {
                int index = myInventory.selectedSlotIndex;
                bool hasItem = myInventory.slots.Count > 0 && index < myInventory.slots.Count;

                if (hasItem)
                {
                    Item selected = myInventory.slots[index].itemData;

                    if (selected != null && selected.isSeed)
                    {
                        plot.Plant(selected);
                        myInventory.RemoveSelectedItem();
                    }
                }
            }
        }
        else if (currentTarget.TryGetComponent(out Merchant merchant))
        {
            if (merchantUIWindow != null)
            {
                merchant.InteractWithMerchant(merchantUIWindow);
                
                cantMove = true;
            }
        }
        else if (currentTarget.TryGetComponent(out Npc npc))
        {
            int index = myInventory.selectedSlotIndex;
            bool hasItem = myInventory.slots.Count > 0 && index < myInventory.slots.Count;

            if (hasItem)
            {
                Item selected = myInventory.slots[index].itemData;

                if (npc.HelpNpc(selected))
                {
                    myInventory.RemoveSelectedItem();
                    myInventory.money += npc.reward;
                    
                    audioSource.PlayOneShot(completeSound);
                }
            }
        }
    }

    public void CloseShop()
    {
        if (merchantUIWindow != null)
        {
            merchantUIWindow.CloseMerchantWindow();
        }
        
        cantMove = false;
    }

    void HandleSelection()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) myInventory.ChangeSelection(-1);
        if (scroll < 0f) myInventory.ChangeSelection(1);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
