using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI,
        survivalScreenUI,
        refineScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsButton,
        survivalButton,
        refineButton,
        backButton;

    //Craft Buttons
    Button craftAxeButton,
        craftPlankButton;

    //Requirement Text
    TextMeshProUGUI AxeReq1;
    TextMeshProUGUI AxeReq2;
    TextMeshProUGUI PlankReq1;

    public bool isOpen;

    //All Blueprints

    public ItemBlueprint AxeBlueprint = new("Axe", 1, 2, "Stone", 3, "Stick", 2);
    public ItemBlueprint PlankBlueprint = new("Plank", 2, 1, "Log", 1, "", 0);

    public void Awake()
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
        isOpen = false;

        toolsButton = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(
            delegate
            {
                OpenToolsCategory();
            }
        );
        survivalButton = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalButton.onClick.AddListener(
            delegate
            {
                OpenSurvivalCategory();
            }
        );
        refineButton = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineButton.onClick.AddListener(
            delegate
            {
                OpenRefineCategory();
            }
        );

        backButton = toolsScreenUI.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(
            delegate
            {
                BackToCrafting();
            }
        );
        backButton = survivalScreenUI.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(
            delegate
            {
                BackToCrafting();
            }
        );
        backButton = refineScreenUI.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(
            delegate
            {
                BackToCrafting();
            }
        );

        //Axe
        AxeReq1 = toolsScreenUI
            .transform.Find("Axe")
            .transform.Find("req1")
            .GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolsScreenUI
            .transform.Find("Axe")
            .transform.Find("req2")
            .GetComponent<TextMeshProUGUI>();

        craftAxeButton = toolsScreenUI
            .transform.Find("Axe")
            .transform.Find("CraftButton")
            .GetComponent<Button>();
        craftAxeButton.onClick.AddListener(
            delegate
            {
                CraftAnyItem(AxeBlueprint);
            }
        );

        //Plank
        PlankReq1 = refineScreenUI
            .transform.Find("Plank")
            .transform.Find("req1")
            .GetComponent<TextMeshProUGUI>();

        craftPlankButton = refineScreenUI
            .transform.Find("Plank")
            .transform.Find("CraftButton")
            .GetComponent<Button>();
        craftPlankButton.onClick.AddListener(
            delegate
            {
                CraftAnyItem(PlankBlueprint);
            }
        );
    }

    void CraftAnyItem(ItemBlueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        // Produce the number of items according to the blueprint
        for (var i = 0; i < blueprintToCraft.numOfItemsToProduce; i++)
        {
            //add item to inventory
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }

        //remove resources from inventory

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }
        if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        //refresh list
        StartCoroutine(calculate());

        // RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count++;
                    break;
                case "Stick":
                    stick_count++;
                    break;
                case "Log":
                    log_count++;
                    break;
            }
        }

        // ------ Axe ------ //

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "2 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        // ------ Plank ------ //

        AxeReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankButton.gameObject.SetActive(true);
        }
        else
        {
            craftPlankButton.gameObject.SetActive(false);
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }

    void BackToCrafting()
    {
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        craftingScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // RefreshNeededItems();

        isOpen = false;
    }
}
