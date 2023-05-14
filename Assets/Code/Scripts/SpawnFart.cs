using UnityEngine;

namespace Code.Scripts
{
    public class SpawnFart : MonoBehaviour
    {
        [SerializeField] private GameObject fart;
        [SerializeField] private Transform butt;
        private LogicScript _logicScript;

        private void Awake()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }

        public void HandleFart()
        {
            if (_logicScript.IsGameOver) return;
            var position = butt.position;
            Instantiate(fart, new Vector3(position.x, position.y, 0), butt.rotation);
        }
    }
}