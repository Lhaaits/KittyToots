using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

namespace Code.Scripts
{
    public class LogicScript : MonoBehaviour
    {
        private const string Highscore = "Hi: ";
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

        public bool IsGameOver { get; set; }
        public bool IsPausedForEffect { get; set; } = true;

        private Camera _camera;
        private string _instructionText = "";

        private InputActionMap _inputActionMap;

        private void Awake()
        {
            _camera = Camera.main;
            if (_camera == null) return;
            var screenToWorldPoint = _camera.ScreenToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            despawnPosX = screenToWorldPoint.x - 20;
        }

        private void Start()
        {
            var currentHighScore = PlayerPrefs.GetInt(Constants.HighScore.ToString());
            if (currentHighScore > 0)
            {
                highScoreText.text = Highscore + currentHighScore;
                highScoreScreen.SetActive(true);
            }

            Debug.Log("awakened, timeSinceStartup: " + Time.realtimeSinceStartup);
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1
            _instructionText = "Touch screen to fart";
#else
            _instructionText = startText.text;
#endif
            startText.text = "";
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (!IsPausedForEffect || !(Time.timeSinceLevelLoad > 1f)) return;
            Time.timeScale = 0;
            // flicker text
            startText.text = Time.realtimeSinceStartup * 3 % 2 < 1 ? _instructionText : "";
        }

        public void HandleFart()
        {
            if (IsGameOver)
            {
                Restart();
                return;
            }

            if (!IsPausedForEffect) return;
            Debug.Log("pressed fart to start");
            startText.text = "";
            IsPausedForEffect = false;
            Time.timeScale = 1;
        }

        public void IncreaseScore()
        {
            score++;
            scoreText.text = score.ToString();
            if (score % 5 != 0) return;
            moveSpeed *= difficultyIncreaseFactor;
            spawnInterval /= difficultyIncreaseFactor;
            maxHeightChange /= difficultyIncreaseFactor;
        }

        public void Restart()
        {
            Debug.Log("restarting game");
            SceneInstantiate.ReloadScene();
            AudioManager.Instance.ChangeMusicPitch(1.0f);
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
            // inputAsset.FindActionMap("UI").Enable();
            AudioManager.Instance.ChangeMusicPitch();
        }

        // TODO recognize phone orientation
        // TODO nice "new high score" animations
        private void UpdateHighScore()
        {
            var currentHighScore = PlayerPrefs.GetInt(Constants.HighScore.ToString());
            if (score <= currentHighScore) return;
            newHighScoreText.text = "New high score! " + score;
            highScoreText.text = Highscore + score;
            PlayerPrefs.SetInt(Constants.HighScore.ToString(), score);
        }

        private void OnDisable()
        {
            Touch.EnhancedTouchSupport.Disable();
        }

        enum Constants
        {
            HighScore
        }
    }
}