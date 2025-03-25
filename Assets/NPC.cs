using System;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool playerInRange;

    public bool isTalkingWithPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

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

    public void StartConversation()
    {
        isTalkingWithPlayer = true;
        // print("conversation starter");

        DialogSystem.Instance.OpenDialogUI();
        DialogSystem.Instance.dialogText.text = "Hello There!";
        DialogSystem
            .Instance.option1.transform.Find("Text (TMP)")
            .GetComponent<TextMeshProUGUI>()
            .text = "Bye";
        DialogSystem.Instance.option1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
    }
}
