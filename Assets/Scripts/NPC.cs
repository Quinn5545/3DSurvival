using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;

    public bool isTalkingWithPlayer;

    TextMeshProUGUI npcDialogText;

    Button optionButton1;
    TextMeshProUGUI optionButton1Text;
    Button optionButton2;
    TextMeshProUGUI optionButton2Text;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;

        optionButton1 = DialogSystem.Instance.option1;
        optionButton1Text = DialogSystem
            .Instance.option1.transform.Find("Text (TMP)")
            .GetComponent<TextMeshProUGUI>();

        optionButton2 = DialogSystem.Instance.option2;
        optionButton2Text = DialogSystem
            .Instance.option2.transform.Find("Text (TMP)")
            .GetComponent<TextMeshProUGUI>();
    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;

        LookAtPlayer();

        // Interacting with the NPC for the first time
        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex]; // 0 at start
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else // Interacting with the NPC after the first time
        {
            // If we return after declining the quest
            if (currentActiveQuest.declined)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;
                SoundManager.Instance.PlayVoiceOvers(
                    currentActiveQuest.info.comebackAfterDeclineClip
                );

                SetAcceptAndDeclineOptions();
            }

            // If we return while the quest is still in progress
            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirementsCompleted())
                {
                    SubmitRequiredItems();

                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;
                    SoundManager.Instance.PlayVoiceOvers(
                        currentActiveQuest.info.comebackCompletedClip
                    );

                    optionButton1Text.text = "[Take Reward]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() =>
                    {
                        ReceiveRewardAndCompleteQuest();
                    });
                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;
                    SoundManager.Instance.PlayVoiceOvers(
                        currentActiveQuest.info.comebackInProgressClip
                    );

                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() =>
                    {
                        DialogSystem.Instance.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.finalWords;
                SoundManager.Instance.PlayVoiceOvers(currentActiveQuest.info.finalWordsClip);

                optionButton1Text.text = "[Close]";
                optionButton1.onClick.RemoveAllListeners();
                optionButton1.onClick.AddListener(() =>
                {
                    DialogSystem.Instance.CloseDialogUI();
                    isTalkingWithPlayer = false;
                });
            }

            // If there is another quest available
            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }
        }
    }

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            AcceptedQuest();
        });

        optionButton2.gameObject.SetActive(true);
        optionButton2Text.text = currentActiveQuest.info.declineOption;
        optionButton2.onClick.RemoveAllListeners();
        optionButton2.onClick.AddListener(() =>
        {
            DeclinedQuest();
        });
    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondRequiredItem, secondRequiredAmount);
        }
    }

    private bool AreQuestRequirementsCompleted()
    {
        print("Checking Requirements");

        // First Item Requirement

        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCounter++;
            }
        }

        // Second Item Requirement -- If we dont have a second item, just set it to 0

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        SetQuestHasCheckpoints(currentActiveQuest);

        bool allCheckpointsCompleted = false;

        if (currentActiveQuest.info.hasCheckpoints)
        {
            foreach (Checkpoint cp in currentActiveQuest.info.checkpoints)
            {
                if (cp.isCompleted == false)
                {
                    allCheckpointsCompleted = false; // if at least one is false, then return false
                    break;
                }

                allCheckpointsCompleted = true;
            }
        }

        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            if (currentActiveQuest.info.hasCheckpoints)
            {
                if (allCheckpointsCompleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    private void SetQuestHasCheckpoints(Quest activeQuest)
    {
        if (activeQuest.info.checkpoints.Count > 0)
        {
            activeQuest.info.hasCheckpoints = true;
        }
        else
        {
            activeQuest.info.hasCheckpoints = false;
        }
    }

    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        SoundManager.Instance.PlayVoiceOvers(
            currentActiveQuest.info.initialDialogClips[currentDialog]
        );
        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1) // If its the last dialog
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOvers(
                currentActiveQuest.info.initialDialogClips[currentDialog]
            );

            currentActiveQuest.initialDialogCompleted = true;

            SetAcceptAndDeclineOptions();
        }
        else // If there are more dialogs
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOvers(
                currentActiveQuest.info.initialDialogClips[currentDialog]
            );

            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void AcceptedQuest()
    {
        // add quest to quest list
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);
        //

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            SoundManager.Instance.PlayVoiceOvers(currentActiveQuest.info.comebackCompletedClip);
            optionButton1Text.text = "[Take Reward]";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                ReceiveRewardAndCompleteQuest();
            });
            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            SoundManager.Instance.PlayVoiceOvers(currentActiveQuest.info.acceptAnswerClip);
            CloseDialogUI();
        }
    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        // add quest to completed quest list
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);
        //

        currentActiveQuest.isCompleted = true;

        var coinsReceived = currentActiveQuest.info.coinReward;
        print("You received " + coinsReceived + " gold coins");

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        }

        activeQuestIndex++;

        // Start Next Quest
        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
        else
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
            print("No more quests");
        }
    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        SoundManager.Instance.PlayVoiceOvers(currentActiveQuest.info.declineAnswerClip);
        CloseDialogUI();
    }

    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
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
