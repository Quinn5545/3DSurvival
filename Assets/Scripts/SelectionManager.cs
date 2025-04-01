using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject interaction_Info_UI;
    public GameObject selectedObject;
    TMP_Text interaction_text;
    public Image centerDotIcon;
    public Image handIcon;
    public bool handIsVisible;
    public GameObject selectedTree;
    public GameObject chopHolder;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }

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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // just for brevity
            InteractableObject interactableObj =
                selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            NPC npc = selectionTransform.GetComponent<NPC>();
            if (npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && !npc.isTalkingWithPlayer)
                {
                    npc.StartConversation();
                }
                if (DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            Animal animal = selectionTransform.GetComponent<Animal>();
            if (animal && animal.playerInRange)
            {
                interaction_text.text = animal.animalName;
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(
                        DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage())
                    );
                }
            }
            else
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            if (interactableObj && interactableObj.playerInRange)
            {
                onTarget = true;
                selectedObject = interactableObj.gameObject;
                interaction_text.text = interactableObj.GetItemName();
                interaction_Info_UI.SetActive(true);

                //set hand icon
                if (interactableObj.CompareTag("pickable"))
                {
                    handIsVisible = true;
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    handIsVisible = false;
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }
            }
            else // if there is a hit, but without interactable script
            {
                handIsVisible = false;
                onTarget = false;
                // interaction_Info_UI.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
        }
        else // if there is no hit at all
        {
            handIsVisible = false;
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIsVisible = false;
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIsVisible = true;
        handIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}
