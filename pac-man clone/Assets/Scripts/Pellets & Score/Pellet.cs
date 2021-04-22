using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int scoreValue = 1;
    public float collisionDistance = 0.2f;

    private Score _score;
    private Transform _pacMan;
    private Vector3 _pacManPosition;

    private void Start()
    {
        _score = FindObjectOfType<Score>();
        _pacMan = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update() { UpdatePlayerPosition(); }

    private void UpdatePlayerPosition()
    {
        _pacManPosition = _pacMan.transform.position;
        
        var distance = Vector3.Distance(_pacManPosition, transform.position);

        if (distance <= collisionDistance)
        {
            GatherPellet();
        }
    }

    private void GatherPellet()
    {
        _score.AddScore(scoreValue);
        Destroy(gameObject);
    }
}
