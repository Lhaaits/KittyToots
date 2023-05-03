using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class ManagePosts : MonoBehaviour
    {
        public GameObject posts;
    
        public float spawnInterval = 2;
        private float _timer;
        public float heightOffset = 10;
    
        // Start is called before the first frame update
        private void Start()
        {
            SpawnPosts();
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
                SpawnPosts();
                _timer = 0;
            }
        }

        private void SpawnPosts()
        {
            var transform1 = transform;
            var position = transform1.position;
            var lowestPoint = position.y - heightOffset;
            var highestPoint = position.y + heightOffset;
            Instantiate(posts, new Vector3(position.x, Random.Range(lowestPoint, highestPoint),0), transform1.rotation);
        }
    }
}
