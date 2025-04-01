using System;
using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField]
    int currentHealth;

    [SerializeField]
    int maxHealth;

    [Header("Sounds")]
    [SerializeField]
    AudioSource soundChannel;

    [SerializeField]
    AudioClip rabbitHitAndScream;

    [SerializeField]
    AudioClip rabbitHitAndDie;

    [SerializeField]
    ParticleSystem bloodSplashParticles;

    [SerializeField]
    GameObject bloodPuddle;

    private Animator animator;
    public bool isDead;

    enum AnimalType
    {
        Rabbit,
        OtherAnimals,
    }

    [SerializeField]
    AnimalType thisAnimalType;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;

            bloodSplashParticles.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("Die");
                GetComponent<AI_Movement>().enabled = false;
                StartCoroutine(AddBloodPuddle());

                isDead = true;
            }
            else
            {
                PlayHitSound();
            }
        }
    }

    IEnumerator AddBloodPuddle()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndDie);
                break;

            default:
                break;
        }
    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndScream);
                break;

            default:
                break;
        }
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
