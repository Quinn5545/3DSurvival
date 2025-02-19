using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler
// , IPointerEnterHandler, IPointerExitHandler
{
    public GameObject trashAlertUI;

    private TextMeshProUGUI textToModify;

    // public Sprite trash_closed;
    // public Sprite trash_opened;

    // private Image imageComponent;

    Button ConfirmBTN,
        CancelBTN;

    GameObject DraggedItem
    {
        get { return DragDrop.itemBeingDragged; }
    }

    GameObject itemToBeDeleted;

    public string ItemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    void Start()
    {
        // imageComponent = transform.Find("Trash").GetComponent<Image>();

        textToModify = trashAlertUI.transform.Find("Question").GetComponent<TextMeshProUGUI>();

        ConfirmBTN = trashAlertUI.transform.Find("Confirm").GetComponent<Button>();
        ConfirmBTN.onClick.AddListener(
            delegate
            {
                DeleteItem();
            }
        );

        CancelBTN = trashAlertUI.transform.Find("Cancel").GetComponent<Button>();
        CancelBTN.onClick.AddListener(
            delegate
            {
                CancelDeletion();
            }
        );
    }

    public void OnDrop(PointerEventData eventData)
    {
        //itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
        if (DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = DraggedItem.gameObject;

            StartCoroutine(NotifyBeforeDeletion());
        }
    }

    IEnumerator NotifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = "Throw away this " + ItemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void CancelDeletion()
    {
        // imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    private void DeleteItem()
    {
        // imageComponent.sprite = trash_closed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (DraggedItem != null && DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
    //     {
    //         imageComponent.sprite = trash_opened;
    //     }
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (DraggedItem != null && DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
    //     {
    //         imageComponent.sprite = trash_closed;
    //     }
    // }
}
