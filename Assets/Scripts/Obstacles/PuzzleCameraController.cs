using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PuzzleCameraController : MonoBehaviour
{
    public static PuzzleCameraController Instance;

    [Header("Camera Setup")]
    public CinemachineCamera puzzleCam;

    [Header("Optional UI")]
    public GameObject puzzleUI;

    private bool isActive = false;

    public bool IsPuzzleActive => isActive; // Used by PuzzleInteraction

    private void Awake()
    {
        Instance = this;
    }

    public void EnterPuzzleMode()
    {
        isActive = true;

        puzzleCam.Priority = 20;

        if (puzzleUI != null)
            puzzleUI.SetActive(true);

        if (CoinBoardManager.Instance != null && CoinBoardManager.Instance.poemText != null)
            CoinBoardManager.Instance.poemText.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player movement + interaction
        if (Player.Instance != null)
        {
            Player.Instance.enabled = false;

            var interact = Player.Instance.GetComponent<PlayerInteract>();
            if (interact != null) interact.enabled = false;
        }
    }

    public void ExitPuzzleMode()
    {
        isActive = false;

        puzzleCam.Priority = 0;

        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (CoinBoardManager.Instance != null && CoinBoardManager.Instance.poemText != null)
            CoinBoardManager.Instance.poemText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable player movement + interaction
        if (Player.Instance != null)
        {
            Player.Instance.enabled = true;

            var interact = Player.Instance.GetComponent<PlayerInteract>();
            if (interact != null) interact.enabled = true;
        }
    }

    private void Update()
    {
        if (isActive && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitPuzzleMode();
        }
    }
}

