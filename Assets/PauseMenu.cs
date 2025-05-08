using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject playerHUD;
    
    [SerializeField] private AudioSource tapeAudioSource;
    [SerializeField] private AudioClip tapeSound;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button rewindButton;
    [SerializeField] private Button configButton;
    [SerializeField] private Button ejectButton;

    private TimeSpan timeSpan;
    private InputManager inputManager;
    
    private bool paused;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        tapeAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        timeSpan = gm.GetTimePlayed();
        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,timeSpan.Milliseconds /10);

        if (inputManager.PlayerInput_Pause()) {
            if (paused) {
                OnResume();
            }
            else {
                OnPause();
            }
        }
    }
    
    
    

    private void OnPause() {
        if (tapeSound != null && tapeAudioSource != null)
        {
            tapeAudioSource.PlayOneShot(tapeSound);
        }
        paused = !paused;
        playerHUD.SetActive(false);
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void OnResume() {
        paused = !paused;
        playerHUD.SetActive(true);
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }


    public void PlayButton() {
        OnResume();
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
}
