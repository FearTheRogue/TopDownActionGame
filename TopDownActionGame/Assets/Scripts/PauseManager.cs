using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PauseManager : MonoBehaviour
{
    public static bool Paused {  get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    public bool IsPaused {  get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.PauseMenu.performed += OnPausePressed;

        SetPaused(false);
    }

    private void OnDestroy()
    {
        inputActions.Player.PauseMenu.performed -= OnPausePressed;
        inputActions.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void RestartScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Paused = paused;

        if (pausePanel != null)
            pausePanel.SetActive(paused);

        Time.timeScale = paused ? 0f : 1f;

        // Cursor Behaviour
        Cursor.visible = paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
