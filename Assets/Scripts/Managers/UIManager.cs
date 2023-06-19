using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Options")]
    [SerializeField] bool showCursor;
    [SerializeField] bool pausable = true;
    [Header("Reference")]
    [SerializeField] GameObject PauseCanvas;

    bool gameIsPaused;

    void Start()
    {
        Refresh();
    }
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            TogglePauseGame();
        }
    }

    public void Refresh()
    {
        ShowOrHideCursor(showCursor);
    }

    public void PauseGame()
    {
        if (pausable == false) return;

        gameIsPaused = true;

        showCursor = true;
        PauseCanvas.SetActive(true);

        Refresh();
    }
    public void UnpauseGame()
    {
        gameIsPaused = false;

        showCursor = false;
        PauseCanvas.SetActive(false);

        Refresh();
    }
    public void TogglePauseGame()
    {
        if (gameIsPaused) UnpauseGame();
        else PauseGame();

        Debug.Log(gameIsPaused);
    }

    public void ShowOrHideCursor(bool show)
    {
        showCursor = show;

        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
