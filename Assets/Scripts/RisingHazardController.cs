using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingHazardController : MonoBehaviour
{
    [Header("Rising Hazard Settings")]
    [SerializeField] private float _riseSpeed = 1f;

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
            Destroy(collision.gameObject);
        }
    }
}
