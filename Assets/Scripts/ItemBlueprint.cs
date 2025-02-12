using UnityEngine;

public class ItemBlueprint
{
    public string itemName;

    public string Req1;
    public string Req2;
    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;

    public ItemBlueprint(string name, int reqNum, string r1, int r1Amount, string r2, int r2Amount)
    {
        itemName = name;

        numOfRequirements = reqNum;

        Req1 = r1;
        Req2 = r2;

        Req1Amount = r1Amount;
        Req2Amount = r2Amount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
