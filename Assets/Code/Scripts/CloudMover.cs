using UnityEngine;

namespace Code.Scripts
{
    public class CloudMover : MonoBehaviour
    {
        private LogicScript _logicScript;
        

        private void Awake()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }

        // Update is called once per frame
        private void Update()
        {
            var transformPosition = Vector3.left * _logicScript.moveSpeed * Time.deltaTime;
            transform.position += transformPosition;
            if (transform.position.x < _logicScript.despawnPosX - 100)
            {
                Destroy(gameObject);
            }
        }
    }
}
