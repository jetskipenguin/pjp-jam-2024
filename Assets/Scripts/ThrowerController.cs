using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : MonoBehaviour
{
    [SerializeField, Tooltip("List of objects that can be thrown")]
    List<GameObject> throwables = new List<GameObject>();

    [SerializeField, Tooltip("Speed of the throw")]
    float throwSpeed = 10f;

    void Update()
    {
        // Random throwable
        GameObject throwable = throwables[Random.Range(0, throwables.Count)];

        if (Input.GetButtonDown("Throw"))
        {
            // Instantiate the throwable
            GameObject throwableInstance = Instantiate(throwable, transform.position, Quaternion.identity);
            Rigidbody2D rb = throwableInstance.GetComponent<Rigidbody2D>();

            // Throw the throwable in the direction of the players mouse
            rb.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwSpeed;
        }
    }
}
