using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject mainUI;
    public GameObject settingsUI;
    public GameObject controlsUI;
    public GameObject hintUI;
    public GameObject resetUI;

    private List<GameObject> uiList;
    void Start()
    {
        uiList = new List<GameObject>()
        {
            mainUI, settingsUI, controlsUI, hintUI, resetUI,
        };
    }

    void Update()
    {
        
    }
    private void ClearUI()
    {
        foreach (GameObject ui in uiList)
        {
            ui.SetActive(false);
        }
    }
    public void OpenMain()
    {
        ClearUI();
        mainUI.SetActive(true);
    }
    public void OpenSettings()
    {
        ClearUI();
        settingsUI.SetActive(true);
    }
    public void OpenControls()
    {
        ClearUI();
        controlsUI.SetActive(true);
    }
    public void OpenHint()
    {
        ClearUI();
        hintUI.SetActive(true);
    }
    public void OpenReset()
    {
        ClearUI();
        resetUI.SetActive(true);
    }
}
