using UnityEngine;

namespace Code.Scripts
{
    public class ManageClouds : MonoBehaviour
    {
        public GameObject cloud;

        private float _timer;
        private float _maxHeightOffset = 83;
        public float spawnInterval = 0.8f;

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
            for (float i = -positionX; i < positionX; i += 8)
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

            var newYPosition = Random.Range(lowestPoint, highestPoint);

            var minZ = 12;
            var positionZ = Random.Range(minZ, 48) * 4;
            var instantiatedCloud = Instantiate(cloud, new Vector3(xPos, newYPosition, positionZ), transform.rotation);
            instantiatedCloud.GetComponent<SpriteRenderer>().color = new Color(1f - (positionZ - minZ)/(255f*4f),1,1f);
        }
    }
}