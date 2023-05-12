using Code.Scripts;
using UnityEngine;

public class FartMover : MonoBehaviour
{
    private LogicScript _logicScript;
    // Start is called before the first frame update


    private void Awake()
    {
        _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    private void Update()
    {
        var transformPosition = Vector3.left * _logicScript.moveSpeed * Time.deltaTime;
        transform.position += transformPosition;
        if (transform.position.x < _logicScript.despawnPosX)
        {
            Destroy(gameObject);
        }
    }
}
