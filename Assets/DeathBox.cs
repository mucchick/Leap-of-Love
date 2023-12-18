using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform RespawnAnchor;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.position = RespawnAnchor.position;
        }
    }
}
