using UnityEngine;

namespace Code.Scripts
{
    public class TriggerScript : MonoBehaviour
    { 
        private LogicScript _logicScript;
        private bool _wasAlreadyTriggered;
    
        // Start is called before the first frame update
        private void Start()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_wasAlreadyTriggered || _logicScript.IsGameOver) return;
            _wasAlreadyTriggered = true;
            _logicScript.IncreaseScore();
        }
    }
}
