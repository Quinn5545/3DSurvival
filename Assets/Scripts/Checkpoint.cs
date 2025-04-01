using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Checkpoint", order = 1)]
public class Checkpoint : ScriptableObject
{
    public string name;
    public bool isCompleted;
}
