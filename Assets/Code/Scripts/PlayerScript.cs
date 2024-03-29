using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class PlayerScript : MonoBehaviour
    {
        /** TODO background parallax
     * start screen
     * add touch control
     * build for mobile
         * 
    */
        public new Rigidbody2D rigidbody;

        [Tooltip("How much up do you want?")] public float upFactor = 10;

        private LogicScript _logicScript;
        private int _balloonCount = 3;
        private List<string> _balloonTags = new();

        public InputActionAsset inputAsset;
        private InputActionMap _inputActionMap;
        private InputAction _fartAction;

        private AudioManager _audioManager;
        private void OnEnable()
        {
            _inputActionMap = inputAsset.FindActionMap("Player");
            _fartAction = _inputActionMap.FindAction("Fart");
            _fartAction.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            _fartAction.Disable();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            _logicScript.IsGameOver = false;
            _balloonTags.Clear();
            _balloonTags.AddRange(new List<string> { "Left", "Middle", "Right" });
            _audioManager = AudioManager.Instance;
        }

        public void HandleFart()
        {
            if (_logicScript.IsGameOver || _logicScript.IsPausedForEffect) return;
            Debug.Log("fartaction triggered");
            rigidbody.velocity = Vector2.up * upFactor;
            _audioManager.PlaySoundEffect(SoundEffect.Fart);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_logicScript.IsGameOver) return;
            if (col.collider.name.Contains("Edge"))
            {
                _logicScript.GameOver();
                return;
            }

            // col is read-only, but if you get its parent gameObject and then the collider, it's writable
            col.gameObject.GetComponent<Collider2D>().enabled = false;

            var playerCollider = col.otherCollider;
            Debug.Log($"{col.collider.name} collided with {playerCollider.name}");

            GameObject balloon;
            string balloonTag;
            if (playerCollider.name.StartsWith("Balloon"))
            {
                balloon = playerCollider.gameObject;
                balloonTag = balloon.tag;
            }
            else // Cat collided, get rightmost balloon
            {
                balloonTag = _balloonTags[^1];
                balloon = GameObject.FindGameObjectWithTag(balloonTag);
            }

            if (_balloonCount > 1)
            {
                StartCoroutine(FlickerPlayer());
            }

            Debug.Log(balloonTag);
            Destroy(balloon);

            _audioManager.PlaySoundEffect(SoundEffect.Hit);

            _balloonCount--;
            _balloonTags.Remove(balloonTag);
            RepositionBalloons();

            if (_balloonCount == 0)
            {
                Debug.Log("collided, game over " + col.collider.name);
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                _logicScript.GameOver();
            }
        }

        private void RepositionBalloons()
        {
            switch (_balloonTags.Count)
            {
                case 2:
                {
                    var leftBalloon = GameObject.FindGameObjectWithTag(_balloonTags[0]);
                    leftBalloon.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, 12f);
                    var rightBalloon = GameObject.FindGameObjectWithTag(_balloonTags[1]);
                    rightBalloon.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, -12f);
                    rigidbody.gravityScale = 5;
                    upFactor = 20;
                    break;
                }
                case 1:
                {
                    var middleBalloon = GameObject.FindGameObjectWithTag(_balloonTags[0]);
                    middleBalloon.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, 0f);
                    rigidbody.gravityScale = 7;
                    upFactor = 23;
                    break;
                }
            }
        }

        private IEnumerator FlickerPlayer()
        {
            var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            Debug.Log("starting flicker coroutine");
            var delay = new WaitForSecondsRealtime(0.1f);
            int flickers = 0;
            while (flickers < 10)
            {
                flickers++;
                foreach (var spriteRenderer in spriteRenderers)
                {
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.enabled = !spriteRenderer.enabled;
                    }
                }

                yield return delay;
            }

            Debug.Log("set renderers back to enabled");
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                }
            }
        }
    }
}