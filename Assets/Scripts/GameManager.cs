using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public bool isGameOver;

    public AudioManager audioManager;
    public UIManager uiManager;

    public GameObject boxNormalPrefab;

    public PlayerController playerController;
    public BlasterController blasterController;
    public GeneratorController generatorController;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI levelCountText;
    public TextMeshProUGUI hintText;

    public GameObject shadow;
    public GameObject pausedUI;
    public TextMeshProUGUI graphicsQualityText;

    public int levelCount;

    private string graphicsQuality = "medium";

    void Start()
    {
        StartCoroutine(LoadPlayerPrefs());
    }

    void Update()
    {
        if ((Input.GetButtonDown("Restart") || playerController.transform.position.y < -25) && !generatorController.isPlaced)
        {
            SceneManager.UnloadSceneAsync("Level " + levelCount);
            LoadLevel();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (isGameActive)
            {
                isGameActive = false;
                shadow.SetActive(true);
                pausedUI.SetActive(true);
                playerController.GetComponent<Animator>().enabled = false;
                Time.timeScale = 0;
                uiManager.OpenMain();
            }
            else if (uiManager.mainUI.activeSelf)
            {
                Unpause();
            }
            else
            {
                uiManager.OpenMain();
            }
        }
        if (Input.GetButtonDown("Skip Level"))
        {
            if (isGameActive)
            {
                StartNextLevel();
            }
        }
    }
    public void Unpause()
    {
        isGameActive = true;
        shadow.SetActive(false);
        pausedUI.SetActive(false);
        playerController.GetComponent<Animator>().enabled = true;
        Time.timeScale = 1;
    }
    public void StartNextLevel()
    {
        SceneManager.UnloadSceneAsync("Level " + levelCount);
        levelCount++;
        LoadLevel();
    }
    private void LoadLevel()
    {
        if (DoesSceneExist("Level " + levelCount))
        {
            SceneManager.LoadScene("Level " + levelCount, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("End Level", LoadSceneMode.Additive);
            isGameOver = true;
        }

        generatorController.spriteRenderer.enabled = false;
        generatorController.isPlaced = false;
        generatorController.ResetCircle();
        generatorController.circleGrowSpeed = 0.9f;

        playerController.gameObject.transform.position = new Vector2(0, 6);
        playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerController.isFrozen = false;
        playerController.hasCanister = false;
        playerController.canisterParticle.SetActive(false);
        playerController.isGrounded = false;

        levelCountText.text = levelCount.ToString();

        tutorialText.text = "";
        if (isGameOver)
        {
            blasterController.SetAmmoCount(100000);
            hintText.text = "Thanks for playing!";
            levelCountText.text = "";
        }
        else if (levelCount == 1)
        {
            blasterController.SetAmmoCount(100);
            tutorialText.text = "Move - A and D\r\nJump - W or Space\r\nShoot - Left Click\r\nShow/Hide generator - Q\r\n" +
                "Place generator - Right Click\r\nRestart - R\r\nEscape - Pause/Unpause\r\nSkip level - P";
            hintText.text = "Press \"q,\" then right-click in the center of the room.";
        }
        else if (levelCount == 2)
        {
            tutorialText.text = "Try shooting the wooden box to make enough space for your generator.";
            hintText.text = "Left-click when your blaster is pointed at the wooden box, then place your generator.";
        }
        else if (levelCount == 3)
        {
            blasterController.SetAmmoCount(1);
            tutorialText.text = "You have limited ammunition so use it wisely!";
            hintText.text = "Shoot a wooden box, then push the others to form a platform to place your generator on.";
        }
        else if (levelCount == 4)
        {
            blasterController.SetAmmoCount(1);
            hintText.text = "Push the wooden boxes to the left, then shoot the floating box (it moves in the direction it was shot).";
        }
        else if (levelCount == 5)
        {
            blasterController.SetAmmoCount(3);
            hintText.text = "Shoot one of the wooden boxes, jump on the left floating box, and shoot at your feet.";
        }
        else if (levelCount == 6)
        {
            blasterController.SetAmmoCount(4);
            hintText.text = "Push the wooden box on the left onto the lower floating box, go to the right side of the room, and bring the boxes over.";
        }
        else if (levelCount == 7)
        {
            blasterController.SetAmmoCount(4);
            hintText.text = "Move the right-most floating box to the right side until it touches the metal boxes. " +
                "Walk towards the right edge of the floating box until you almost fall off.";
            tutorialText.text = "Your generator needs to be grounded at three points: The center, left, and right.";
        }
        else if (levelCount == 8)
        {
            blasterController.SetAmmoCount(1);
            tutorialText.text = "Shoot the anti-gravity box to activate it.";
            hintText.text = "Stand on the anti-gravity box and jump off onto the metal boxes on the right side.";
        }
        else if (levelCount == 9)
        {
            blasterController.SetAmmoCount(2);
            hintText.text = "Acivate the left anti-gravity box, activate the right anti-gravity box while standing on it, jump on the floating box," +
                " and push the floating box out of the way.";
        }
        else if (levelCount == 10)
        {
            blasterController.SetAmmoCount(10);
            tutorialText.text = "You can shoot an activated anti-gravity box again to deactivate it.";
            hintText.text = "Use the floating box to push the right anti-gravity box.";
        }
        else if (levelCount == 11)
        {
            blasterController.SetAmmoCount(7);
            hintText.text = "Pick up the canister, shoot the top-most metal box, and bring the floating box to the right side.";
        }
        else if (levelCount == 12)
        {
            blasterController.SetAmmoCount(2);
            hintText.text = "Destroy the metal box next to the floating box and push the floating box over. " +
                "Alternatively, push the anti-gravity box to the right as you fall off, then deactivate it and push it next to the floating box.";
        }
        else if (levelCount == 13)
        {
            blasterController.SetAmmoCount(2);
            hintText.text = "Push the left floating box over a little, jump the gap to the right side, and push the right floating box over a little.";
        }
        else if (levelCount == 14)
        {
            blasterController.SetAmmoCount(9);
            hintText.text = "Use the canisters to destroy the two top-most metal boxes on the right side, and deactivate the anti-gravity box as needed.";
        }
        else if (levelCount == 15)
        {
            blasterController.SetAmmoCount(4);
            hintText.text = "Ride the anti-gravity box up to the floating box then shoot twice to bring yourself near the canister. " +
                "Jump off, and the moment you reach the canister mid-air, start moving left.";
        }
        else if (levelCount == 16)
        {
            blasterController.SetAmmoCount(4);
            hintText.text = "You only need one canister. Use it to destroy the metal box next to where the left anti-gravity box started. " +
                "Then, deactivate the left anti-gravity box.";
        }
        else if (levelCount == 17)
        {
            blasterController.SetAmmoCount(4);
            hintText.text = "Deactivate the anti-gravity box, push it to the left side, and activate it as it falls. " +
                "Destroy the metal box three spaces left and one space up from the floating box, then deactivate the anti-gravity box and jump " +
                "to the right side.";
        }
        else if (levelCount == 18)
        {
            blasterController.SetAmmoCount(1);
            hintText.text = "Activate the anti-gravity box in the middle and use it to jump to the left anti-gravity box. " +
                "Push the left anti-gravity box into the gap between the left pile of metal boxes and the middle pile of metal boxes.";
        }
        else if (levelCount == 19)
        {
            blasterController.SetAmmoCount(2);
            hintText.text = "Restart the level, and as you fall, move to the left so you land on the metal boxes. " +
                "Use the left canister to destroy a metal box on the second (from the left) pile. Then stand on the edge of either the " +
                "second or third pile.";
        }
        else if (levelCount == 20)
        {
            blasterController.SetAmmoCount(10);
            hintText.text = "Take a leap of faith (on the left side of the room).";
            generatorController.circleGrowSpeed = 3;
        }
        Save();
    }
    private bool DoesSceneExist(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }
    public void Reset()
    {
        if (!isGameOver)
        {
            SceneManager.UnloadSceneAsync("Level " + levelCount);
        }
        else
        {
            SceneManager.UnloadSceneAsync("End Level");
            isGameOver = false;
        }
        levelCount = 1;
        LoadLevel();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void UpdateGraphics()
    {
        if (graphicsQuality == "low")
        {
            QualitySettings.SetQualityLevel(1);
            graphicsQuality = "medium";
            graphicsQualityText.text = "GRAPHICS:MEDIUM";
        }
        else if (graphicsQuality == "medium")
        {
            QualitySettings.SetQualityLevel(2);
            graphicsQuality = "high";
            graphicsQualityText.text = "GRAPHICS:HIGH";
        }
        else if (graphicsQuality == "high")
        {
            QualitySettings.SetQualityLevel(0);
            graphicsQuality = "low";
            graphicsQualityText.text = "GRAPHICS:LOW";
        }
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("bgmVolume", audioManager.bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume", audioManager.sfxVolume);
        PlayerPrefs.SetString("graphicsQuality", graphicsQuality);

        PlayerPrefs.SetInt("levelCount", levelCount);
        PlayerPrefs.Save();
    }
    private void Load()
    {
        audioManager.UpdateBGMVolume(PlayerPrefs.GetFloat("bgmVolume"));
        audioManager.UpdateSFXVolume(PlayerPrefs.GetFloat("sfxVolume"));
        graphicsQuality = PlayerPrefs.GetString("graphicsQuality");
        if (graphicsQuality == "low")
        {
            QualitySettings.SetQualityLevel(0);
            graphicsQualityText.text = "GRAPHICS:LOW";
        }
        else if (graphicsQuality == "medium")
        {
            QualitySettings.SetQualityLevel(1);
            graphicsQualityText.text = "GRAPHICS:MEDIUM";
        }
        else if (graphicsQuality == "high")
        {
            QualitySettings.SetQualityLevel(2);
            graphicsQualityText.text = "GRAPHICS:HIGH";
        }

        levelCount = PlayerPrefs.GetInt("levelCount");
        LoadLevel();
    }
    private IEnumerator LoadPlayerPrefs()
    {
        yield return new WaitForSeconds(0.01f);
        if (!PlayerPrefs.HasKey("bgmVolume"))
        {
            audioManager.UpdateBGMVolume(0.5f);
            audioManager.UpdateSFXVolume(0.5f);
            Save();
        }
        Load();
        yield break;
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
