using System;
using Code.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public GameObject PauseScreen;
    
    public Slider sfxSlider;
    public Slider musicSlider;
    public bool IsPaused { get; set; }

    private InputActionMap _playerActionMap;
    private InputActionMap _UIActionMap;
    private InputAction _pauseAction;
    private InputAction _unpauseAction;

    private float prevTimeScale;
        
    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(v=> AudioManager.Instance.SfxVolumeChange(v, true));
        musicSlider.onValueChanged.AddListener(v => AudioManager.Instance.MusicVolumeChange(v, true));
    }

    private void OnEnable()
    {
        _UIActionMap = inputAsset.FindActionMap("UI");
        _playerActionMap = inputAsset.FindActionMap("Player");
        _pauseAction = _playerActionMap.FindAction("Pause");
        _unpauseAction = _UIActionMap.FindAction("Pause");
        _pauseAction.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void Update()
    {
        if (_pauseAction.triggered && !IsPaused)
        {
            PauseGame();
        }

        if (_unpauseAction.triggered && IsPaused)
        {
            Unpause();
        }
    }

    public void PauseGame()
    {
        Debug.Log("paused game");
        _UIActionMap.Enable();
        _unpauseAction.Enable();
        _playerActionMap.Disable();
        _pauseAction.Disable();
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        IsPaused = true;
        PauseScreen.SetActive(true);
        AudioManager.Instance.MusicVolumeChange(0.5f * PlayerPrefs.GetFloat("musicVolume", 1.0f), false);
    }

    public void Unpause()
    {
        Debug.Log("unpaused game");
        _UIActionMap.Disable();
        _unpauseAction.Disable();
        _playerActionMap.Enable();
        _pauseAction.Enable();
        Time.timeScale = prevTimeScale;
        IsPaused = false;
        PauseScreen.SetActive(false);
        AudioManager.Instance.MusicVolumeChange(PlayerPrefs.GetFloat("musicVolume", 1.0f), false);
    }

    public void Exit()
    {
        Debug.Log("Exit game");
        SceneManager.UnloadSceneAsync("Managers");
        Application.Quit();
    }
}