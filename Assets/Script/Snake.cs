using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segmentsList; // This is for move the body parts
    public GameObject segmentPrefab;
    public int initialSize = 2;

    void Start()
    {
        _segmentsList = new List<Transform>();
        _segmentsList.Add(this.transform);

        for (int i = 0; i < initialSize; i++)
        {
            Grow();
        }
    }

    private void Update() // Do not use this, use inputMehods
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
    }

    void FixedUpdate() // Let's try to change to other method to move
    {
        for (int i = _segmentsList.Count - 1; i > 0; i--) // The locomotion of the body
        {
            _segmentsList[i].position = _segmentsList[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab).transform;
        segment.position = _segmentsList[_segmentsList.Count - 1].position;

        _segmentsList.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segmentsList.Count; i++)
        {
            Destroy(_segmentsList[i].gameObject);
        }

        _segmentsList.Clear();
        _segmentsList.Add(this.transform);

        for (int i = 0; i < initialSize; i++)
        {
            Grow();
        }

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
        else if (other.CompareTag("Player"))
        {
            ResetState();
        }
    }
}
