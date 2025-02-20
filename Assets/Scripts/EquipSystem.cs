using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // ----- UI ----- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    // public List<string> itemList = new List<string>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PopulateSlotsList();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                HandleKeyInput(i);
                break;
            }
        }
    }

    private void PopulateSlotsList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // adding item to list
        // itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter++;
            }
        }

        if (counter == 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleKeyInput(int keyNumber)
    {
        switch (keyNumber)
        {
            case 1:
                Debug.Log("key pressed: " + keyNumber);
                SelectQuickSlot(1);
                break;
            case 2:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(2);
                break;
            case 3:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(3);
                break;
            case 4:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(4);
                break;
            case 5:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(5);
                break;
            case 6:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(6);
                break;
            case 7:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(7);
                break;
            case 8:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(8);
                break;
            case 9:
                Debug.Log("key pressed: " + keyNumber);

                SelectQuickSlot(9);
                break;
            default:
                Debug.Log("Unknown key");
                break;
        }
    }

    private void SelectQuickSlot(int number)
    {
        // Debug.Log($"Selected slot: {number}");
        selectedNumber = number;

        // Deselect previous item if it exists
        if (selectedItem != null)
        {
            selectedItem.GetComponent<InventoryItem>().isSelected = false;
            selectedItem = null;
        }

        selectedItem = GetSelectedItem(number);

        // Only select if the slot isn't empty
        if (selectedItem != null)
        {
            selectedItem.GetComponent<InventoryItem>().isSelected = true;
        }

        // Update UI for all slots
        foreach (Transform child in numbersHolder.transform)
        {
            var textComponent = child.Find("text")?.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
                textComponent.color = Color.gray;
        }

        // Update the selected slot's color
        Transform numberTransform = numbersHolder.transform.Find("number" + number);
        if (numberTransform != null)
        {
            var textComponent = numberTransform.Find("text")?.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.color = Color.white;
            }
        }
    }

    private GameObject GetSelectedItem(int slotNumber)
    {
        Transform slotTransform = quickSlotsList[slotNumber - 1].transform;

        if (slotTransform.childCount > 0)
        {
            return slotTransform.GetChild(0).gameObject;
        }
        else
        {
            return null;
        }
    }
}
