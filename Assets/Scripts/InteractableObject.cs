using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.Mouse0)
            && playerInRange
            && SelectionManager.Instance.onTarget
            && SelectionManager.Instance.selectedObject == gameObject
        )
        {
            // if the inventory is NOT full
            if (InventorySystem.Instance.CheckSlotsAvailable(0))
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);

                // Debug.Log("item added to inventory");
                InventorySystem.Instance.AddToInventory(ItemName);
                InventorySystem.Instance.itemsPickedUp.Add(gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
