using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PacMan : PathfindingUnit
{
    private Vector3 _direction;

    private void Update() 
    { 
        CheckInput();
        Move();
        UpdateOrientation(); 
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector3.down;
        }
    }

    private void Move()
    {
        if (_direction != Vector3.zero)
        {
            var target = new Vector3();

            if (_direction == Vector3.left)
            {
                target = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            }
            else if (_direction == Vector3.right)
            {
                target = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            }
            else if (_direction == Vector3.up)
            {
                target = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
            }
            else if (_direction == Vector3.down)
            {
                target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
            }
            else if(_direction == Vector3.zero)
            {
                target = transform.position; 
            }
        
            DetermineTarget(transform.position, target);
        }
    }

    private void UpdateOrientation()
    {
        if (_direction != Vector3.zero)
        {
            if (_direction == Vector3.left)
            {
                transform.rotation = quaternion.Euler(90, 0, -160);
            }
            else if (_direction == Vector3.right)
            {
                transform.rotation = quaternion.Euler(90, 0, 0);
            }
            else if (_direction == Vector3.up)
            {
                transform.rotation = quaternion.Euler(90, 0, 90);
            }
            else if (_direction == Vector3.down)
            {
                transform.rotation = quaternion.Euler(90, 0, -90);
            }
        }
    }
}
