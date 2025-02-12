using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsButton;

    //Craft Buttons
    Button craftAxeButton;

    //Requirement Text
    Text AxeReq1;
    Text AxeReq2;

    bool isOpen;

    //All Blueprints


    public static CraftingSystem Instance { get; set; }

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
        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = toolsScreenUI
            .transform.Find("Axe")
            .transform.Find("Craft Button")
            .GetComponent<Button>();
        craftAxeButton.onClick.AddListener(
            delegate
            {
                CraftAnyItem();
            }
        );
    }

    void CraftAnyItem()
    {
        //add item to inventory


        //remove resources from inventory


        //refresh list
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update() { }
}
