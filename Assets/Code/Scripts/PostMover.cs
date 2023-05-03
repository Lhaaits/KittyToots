using UnityEngine;

namespace Code.Scripts
{
    public class PostMover : MonoBehaviour
    {
        public float moveSpeed = 10;
        [HideInInspector] public float despawnPosX;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            if (_camera == null) return;
            var screenToWorldPoint = _camera.ScreenToWorldPoint(new Vector3(0,0,_camera.nearClipPlane));
            despawnPosX = screenToWorldPoint.x - 20;
        }

        // Update is called once per frame
        private void Update()
        {
            var transformPosition = Vector3.left * moveSpeed * Time.deltaTime;
            transform.position += transformPosition;
            if (transform.position.x < despawnPosX)
            {
                Destroy(gameObject);
            }
        }
    }
}
