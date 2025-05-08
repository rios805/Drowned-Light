using UnityEngine;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance { get; private set; }
    

    [SerializeField] private GameObject notePanel;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private TextMeshProUGUI noteTitle;
    [SerializeField] private NoteSO noteSO;
    [SerializeField] private AudioClip noteSound;
    private AudioSource audioSource;
    private Player player;
    private bool isShowing = false;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        player = Player.Instance;
    }
    

    private void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.Space))
        {
            HideNote();
        }
    }

    public void ShowNote(NoteSO noteSO) {
        if (noteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(noteSound);
        }
        
        playerHUD.SetActive(false);
        noteTitle.text = noteSO.noteTitle;
        noteText.text = noteSO.noteText;
        notePanel.SetActive(true);
        isShowing = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void HideNote()
    {
        playerHUD.SetActive(true);
        notePanel.SetActive(false);
        isShowing = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public bool IsShowing() {
        return isShowing;
    }
}
