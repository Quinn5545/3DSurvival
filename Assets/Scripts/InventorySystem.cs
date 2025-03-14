using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject ItemInfoUI;
    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isOpen;

    // public bool isFull;


    //Pickup Popup
    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public Image pickupImage;
    public List<string> itemsPickedUp;

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

    void Start()
    {
        isOpen = false;
        // isFull = false;

        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.E)
            && !isOpen
            && !ConstructionManager.Instance.inConstructionMode
        )
        {
            // Debug.Log("e is pressed");
            inventoryScreenUI.SetActive(true);
            CraftingSystem.Instance.craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (
            Input.GetKeyDown(KeyCode.E)
            && isOpen
            && !ConstructionManager.Instance.inConstructionMode
        )
        {
            inventoryScreenUI.SetActive(false);
            CraftingSystem.Instance.craftingScreenUI.SetActive(false);
            CraftingSystem.Instance.toolsScreenUI.SetActive(false);
            CraftingSystem.Instance.survivalScreenUI.SetActive(false);
            CraftingSystem.Instance.refineScreenUI.SetActive(false);
            CraftingSystem.Instance.constructionScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();
        itemToAdd = Instantiate(
            Resources.Load<GameObject>(itemName),
            whatSlotToEquip.transform.position,
            whatSlotToEquip.transform.rotation
        );
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);
        itemList.Add(itemName);

        Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

        TriggerPickupPopup(itemName, sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    void TriggerPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
        Invoke("HidePickupAlert", 2f);
    }

    void HidePickupAlert()
    {
        pickupAlert.SetActive(false);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                emptySlot += 1;
            }
        }
        if (emptySlot >= emptyNeeded)
        {
            // Debug.Log(emptySlot);
            return true;
        }
        else
        {
            // Debug.Log(emptySlot);
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (
                    slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)"
                    && counter != 0
                )
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);

                    counter--;
                }
            }
        }
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; //Stone(Clone)
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }
}
