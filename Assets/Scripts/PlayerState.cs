using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ------- Player Health ------- //

    public float currentHealth;
    public float maxHealth;

    // ------- Player Calories ------- //

    public float currentCalories;
    public float maxCalories;
    float distanceTraveled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;

    // ------- Player Hydration ------- //

    public float currentHydrationPercent;
    public float maxHydrationPercent;
    public bool isDehydrating = true;

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
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration()
    {
        while (isDehydrating)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTraveled >= 5)
        {
            distanceTraveled = 0;
            currentCalories -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth -= 5;
        }
    }
};
