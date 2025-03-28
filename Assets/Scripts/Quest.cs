using UnityEngine;

[System.Serializable]
public class Quest
{
    public string QuestName;
    public string QuestGiver;
    public string QuestDescription;

    [Header("Bools")]
    public bool accepted;
    public bool declined;
    public bool initialDialogCompleted;
    public bool isCompleted;
    public bool isComplicated;

    public bool hasNoRequirements;

    [Header("Quest Info")]
    public QuestInfo info;
}
