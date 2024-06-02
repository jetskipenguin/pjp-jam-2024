using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingHazardController : MonoBehaviour
{
    [Header("Rising Hazard Settings")]
    [SerializeField] private float _riseSpeed = 1f;

    private GameObject gameOverUI;
	private void Awake()
	{
        gameOverUI = GameObject.FindGameObjectWithTag("GameOver");
        gameOverUI.SetActive(false);
	}
	private void FixedUpdate()
    {
        // Continuously move the hazard upwards
        transform.position += Vector3.up * _riseSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO: gameover here
            //Destroy(collision.gameObject);
            GameOver(collision.gameObject);
        }
    }

    private void GameOver(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Collider2D>().enabled = false;  
        gameOverUI.SetActive(true);
    }
}
