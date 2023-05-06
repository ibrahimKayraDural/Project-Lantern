using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] bool showCursor;

    void Start()
    {
        Refresh();
    }
    public void Refresh()
    {
        ShowOrHideCursor(showCursor);
    }


    public void ShowOrHideCursor(bool show)
    {
        showCursor = show;

        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
