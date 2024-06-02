using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingHazardController : MonoBehaviour
{
    [Header("Rising Hazard Settings")]
    [SerializeField] private float _riseSpeed = 1f;

    [Header("Asset References")]
    [SerializeField] private GameOverChannelSO _gameOver;

	private void FixedUpdate()
    {
        // Continuously move the hazard upwards
        transform.position += Vector3.up * _riseSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameOver(collision.gameObject);
        }
    }

    private void GameOver(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Collider2D>().enabled = false;  
        _gameOver.RaiseEvent();
    }
}
