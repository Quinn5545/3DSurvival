using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{
    public static EquippableItem Instance { get; set; }

    public Animator animator;

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
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.handIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
            && !EquipSystem.Instance.selectedItem.CompareTag("ConstructionItem")
        )
        {
            // Debug.Log("test");
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
        }
        if (
            Input.GetMouseButtonDown(1)
            && EquipSystem.Instance.selectedItem.CompareTag("ConstructionItem")
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.handIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
        )
        {
            // Debug.Log("test RC");
            // Debug.Log(EquipSystem.Instance.selectedItem.name);
            switch (EquipSystem.Instance.selectedItem.name)
            {
                case "Foundation(Clone)":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                    break;
                case "Foundation":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel"); // just for testing purposes
                    break;
                case "Wall(Clone)":
                    ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                    break;
                case "Wall":
                    ConstructionManager.Instance.ActivateConstructionPlacement("WallModel"); // just for testing purposes
                    break;
                default:
                    // do nothing
                    break;
            }
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }
}
