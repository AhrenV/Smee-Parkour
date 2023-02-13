using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enabler : MonoBehaviour
{
    public GameObject canvas;
    public GameObject partyManager;
    public GameObject UINETManager;
    void Start()
    {
        Invoke("GLITCH", 0.05f);
    }

    void GLITCH()
    {
        canvas.SetActive(true);
        partyManager.SetActive(true);
        UINETManager.SetActive(true);
    }
}
