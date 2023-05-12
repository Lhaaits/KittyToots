using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class ManagePosts : MonoBehaviour
    {
        public GameObject posts;
    
        private float _timer;
        private float _lastYPosition = 0;
        private float _maxHeightOffset = 9.5f;

        private LogicScript _logicScript;

        private void Awake()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            SpawnPosts();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_timer < _logicScript.spawnInterval)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                SpawnPosts();
                _timer = 0;
            }
        }

        private void SpawnPosts()
        {
            var position = transform.position;
            var lowestPoint = position.y - _maxHeightOffset;
            var highestPoint = position.y + _maxHeightOffset;
            var maxUp = Mathf.Min(highestPoint - _lastYPosition, _logicScript.maxHeightChange);
            var maxDown = Mathf.Max(lowestPoint - _lastYPosition, -_logicScript.maxHeightChange);
            
            // TODO if plus , then more minus, and vice versa
            var newYPosition = _lastYPosition + Random.Range(maxDown, maxUp);
            Instantiate(posts, new Vector3(position.x, newYPosition,0), transform.rotation);
            _lastYPosition = newYPosition;
        }
    }
}
