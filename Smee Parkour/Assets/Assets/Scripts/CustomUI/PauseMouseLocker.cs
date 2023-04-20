using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMouseLocker : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuManager;
    public void LockMouse()
    {
        if (Cursor.lockState == CursorLockMode.Locked && pauseMenuManager.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        return;
    }

    private void Start()
    {
        pauseMenuManager.SetActive(true); // Some weird glitch to do with it needing to be set active during runtime (probably because script does something when property is changed)
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            LockMouse();
        }
    }
}
