using UnityEngine;

public class EdgeManager : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        if (_camera == null) return;
        var bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0,0,_camera.nearClipPlane));
        var topRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth,_camera.pixelHeight,_camera.nearClipPlane));
        var colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var edge in colliders)
        {
            edge.size = new Vector2(topRight.x - bottomLeft.x, 1);
            var edgeTransform = edge.transform;
            edgeTransform.position = edge.name switch
            {
                "BottomEdge" => new Vector3(0, bottomLeft.y - 40),
                "TopEdge" => new Vector3(0, topRight.y + 40),
                _ => edgeTransform.position
            };
        }
    }
}
