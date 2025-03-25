using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; set; }

    public TextMeshProUGUI dialogText;

    public Button option1;
    public Button option2;

    public Canvas dialogUI;

    public bool dialogUIActive;

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
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUIActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        dialogUIActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
