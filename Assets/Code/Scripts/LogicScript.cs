using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

namespace Code.Scripts
{
    public class LogicScript : MonoBehaviour
    {
        public int score;
        public TMP_Text scoreText;
        public GameObject highScoreScreen;
        public TMP_Text highScoreText;
        public TMP_Text newHighScoreText;
        public TMP_Text startText;
        public GameObject gameOverScreen;
        public float slowMotionFactor = 0.1f;
        public float moveSpeed = 15;
        public float spawnInterval = 3;
        public float maxHeightChange = 9.5f;
        public float difficultyIncreaseFactor = 1.02f;
        
        [HideInInspector] public float despawnPosX;
        private Camera _camera;

        public bool IsGameOver { get; set; }

        public bool IsPaused { get; set; } = true;
        private string _instructionText = "";
        public InputActionAsset inputAsset;

        private InputActionMap _inputActionMap;
        private InputAction _fartAction;

        private void OnEnable()
        {
            _inputActionMap = inputAsset.FindActionMap("Player");
            _fartAction = _inputActionMap.FindAction("Fart");
            _fartAction.Enable();
        }

        private void Awake()
        {
            _camera = Camera.main;
            if (_camera == null) return;
            var screenToWorldPoint = _camera.ScreenToWorldPoint(new Vector3(0,0,_camera.nearClipPlane));
            despawnPosX = screenToWorldPoint.x - 20;
        }

        private void Start()
        {
            var currentHighScore = PlayerPrefs.GetInt(Constants.HighScore.ToString());
            if (currentHighScore > 0)
            {
                highScoreText.text = currentHighScore.ToString();
                highScoreScreen.SetActive(true);
            }

            Debug.Log("awakened, timeSinceStartup: " + Time.realtimeSinceStartup);
            _instructionText = startText.text;
            startText.text = "";
            Time.timeScale = 1;
            _fartAction.Enable();
        }

        private void Update()
        {
            if (IsPaused && Time.timeSinceLevelLoad > 1f) // Wait a moment before pausing, for effect
            {
                Time.timeScale = 0;
                if (Time.realtimeSinceStartup * 3 % 2 < 1)
                {
                    startText.text = _instructionText;
                }
                else
                {
                    startText.text = "";
                }
            }

            if (_fartAction.triggered)
            {
                if (!IsPaused) return;
                Debug.Log("pressed fart to start");
                startText.text = "";
                IsPaused = false;
                Time.timeScale = 1;
            }
        }

        public void IncreaseScore()
        {
            score++;
            scoreText.text = score.ToString();
            if (score % 5 == 0)
            {
                moveSpeed *= difficultyIncreaseFactor;
                spawnInterval /=  difficultyIncreaseFactor;
                maxHeightChange /= difficultyIncreaseFactor;
            }
        }

        public void Restart()
        {
            Debug.Log("restarting game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            IsGameOver = false;
        }

        public void GameOver()
        {
            if (IsGameOver) return; // If already game over, don't do this again
            IsGameOver = true;
            UpdateHighScore();
            Debug.Log("slow down time");
            gameOverScreen.SetActive(true);
            Time.timeScale = slowMotionFactor;
            inputAsset.FindActionMap("UI").Enable();
            _fartAction.Disable();
        }
        // TODO exit game menu

        // TODO nice "new high score" animations
        private void UpdateHighScore()
        {
            var currentHighScore = PlayerPrefs.GetInt(Constants.HighScore.ToString());
            if (score > currentHighScore)
            {
                newHighScoreText.text = "New high score! " + score;
                highScoreText.text = score.ToString();
                PlayerPrefs.SetInt(Constants.HighScore.ToString(), score);
            }
        }

        enum Constants
        {
            HighScore
        }

        private void OnDisable()
        {
            _fartAction.Disable();
        }
    }
}