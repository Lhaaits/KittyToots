using UnityEngine;

namespace Code.Scripts
{
    public class ManageClouds : MonoBehaviour
    {
        public GameObject cloud;

        private float _timer;
        private float _lastYPosition = 0;
        private float _maxHeightOffset = 83;
        public float spawnInterval = 0.5f;

        private LogicScript _logicScript;

        private void Awake()
        {
            _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            SpawnClouds();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_timer < spawnInterval)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                SpawnCloud();
                _timer = 0;
            }
        }

        private void SpawnClouds()
        {
            var positionX = transform.position.x;
            for (float i = -positionX; i < positionX; i += 5)
            {
                SpawnCloud(i);
            }
        }

        private void SpawnCloud()
        {
            SpawnCloud(transform.position.x);
        }

        private void SpawnCloud(float xPos)
        {
            var position = transform.position;
            var lowestPoint = position.y - _maxHeightOffset;
            var highestPoint = position.y + _maxHeightOffset;
            var maxUp = Mathf.Min(highestPoint - _lastYPosition, _logicScript.maxHeightChange);
            var maxDown = Mathf.Max(lowestPoint - _lastYPosition, -_logicScript.maxHeightChange);

            var newYPosition = Random.Range(lowestPoint, highestPoint);

            var positionZ = Random.Range(25, 50) * 2;
            var instantiatedCloud = Instantiate(cloud, new Vector3(xPos, newYPosition, positionZ), transform.rotation);
            instantiatedCloud.GetComponent<SpriteRenderer>().color =
                new Color(255 - (2 * positionZ), 255 - (2 * positionZ), 255);
            _lastYPosition = newYPosition;
        }
    }
}