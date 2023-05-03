using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts;
using UnityEngine;

public class FartMover : MonoBehaviour
{
    private PostMover PostMover;
    // Start is called before the first frame update
    private float _moveSpeed;

    private void Awake()
    {
        PostMover = GameObject.FindGameObjectWithTag("Post").GetComponent<PostMover>();
        _moveSpeed = PostMover.moveSpeed;
    }

    private void Update()
    {
        var transformPosition = Vector3.left * _moveSpeed * Time.deltaTime;
        transform.position += transformPosition;
        if (transform.position.x < PostMover.despawnPosX)
        {
            Destroy(gameObject);
        }
    }
}
