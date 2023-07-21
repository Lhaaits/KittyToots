using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Scripts
{
    public class Pause : MonoBehaviour
    {
        public InputActionAsset inputAsset;
        [FormerlySerializedAs("PauseScreen")] public GameObject pauseScreen;

        public Slider sfxSlider;
        public Slider musicSlider;
        public bool IsPaused { get; set; }

        private InputActionMap _playerActionMap;
        private InputActionMap _uiActionMap;
        private InputAction _pauseAction;
        private InputAction _unpauseAction;

        private float _prevTimeScale;

        private void Start()
        {
            sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SfxVolumeKey, 1.0f);
            sfxSlider.onValueChanged.AddListener(v => AudioManager.Instance.SfxVolumeChange(v, true));
            musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MusicVolumeKey, 1.0f);
            musicSlider.onValueChanged.AddListener(v => AudioManager.Instance.MusicVolumeChange(v, true));
        }

        private void OnEnable()
        {
            _uiActionMap = inputAsset.FindActionMap("UI");
            _playerActionMap = inputAsset.FindActionMap("Player");
            _pauseAction = _playerActionMap.FindAction("Pause");
            _unpauseAction = _uiActionMap.FindAction("Pause");
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1
            _uiActionMap.Enable();
#endif
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
            _uiActionMap.Enable();
            _unpauseAction.Enable();
            _playerActionMap.Disable();
            _pauseAction.Disable();
            _prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
            IsPaused = true;
            pauseScreen.SetActive(true);
        }

        public void Unpause()
        {
            Debug.Log("unpaused game");
#if !(UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1)
        _uiActionMap.Disable();
#endif
            _unpauseAction.Disable();
            _playerActionMap.Enable();
            _pauseAction.Enable();
            Time.timeScale = _prevTimeScale;
            IsPaused = false;
            pauseScreen.SetActive(false);
        }

        public void Exit()
        {
            Debug.Log("Exit game");
            SceneManager.UnloadSceneAsync("Managers");
            Application.Quit();
        }
    }
}