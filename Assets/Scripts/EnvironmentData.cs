using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public List<string> pickedUpItems;

    public EnvironmentData(List<string> _pickedUpItems)
    {
        pickedUpItems = _pickedUpItems;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
