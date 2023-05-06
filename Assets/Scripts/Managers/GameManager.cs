using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _lantern;

    public bool debugModeOn { get; private set; }

    private void Start()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectsWithTag("Player")[0];
            _lantern = GameObject.Find("Lantern");
            if (_player == null)
                Debug.LogError("WHERE IS THE PLAYER");
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            debugModeOn = !debugModeOn;

            if (debugModeOn)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            Debug.Log("Debug mode is on = " + debugModeOn);
        }
    }

    public void ToggleDebugMode() => debugModeOn = !debugModeOn;
    public void ToggleDebugMode(bool SetOn) => debugModeOn = SetOn;
    public GameObject GetPlayerGO() => _player;
    public Transform GetPlayerTransform() => _player.transform;
    public Vector2 GetLanternPosition() => _lantern.transform.position;
    public Vector2 GetPlayerPosition() => _player.GetComponent<CapsuleCollider2D>().transform.position;
}
