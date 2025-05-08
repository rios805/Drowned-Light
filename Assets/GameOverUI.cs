using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject playerHUD;
    
    [SerializeField] private AudioSource tapeAudioSource;
    [SerializeField] private AudioClip tapeSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TimeSpan timeSpan;
    private InputManager inputManager;
    private void Start()
    {
        tapeAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void ReloadButton() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnDeath() {
        playerHUD.SetActive(false);
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        timeSpan = gm.GetTimePlayed();
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
