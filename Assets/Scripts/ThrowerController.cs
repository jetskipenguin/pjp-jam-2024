using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : MonoBehaviour
{
    [SerializeField, Tooltip("List of objects that can be thrown")]
    public List<GameObject> throwables = new List<GameObject>();

    [SerializeField, Tooltip("Speed of the throw")]
    public float throwSpeed = 10f;

    [SerializeField, Tooltip("Length of cooldown between throws (sec)")]
    public float throwCooldown = 0.5f;

    private bool _canThrow = true;

    public void Update()
    {
        // Random throwable
        GameObject throwable = throwables[Random.Range(0, throwables.Count)];

        if (Input.GetButtonDown("Throw") && _canThrow)
        {
            // Instantiate the throwable
            GameObject throwableInstance = Instantiate(throwable, transform.position, Quaternion.identity);
            Rigidbody2D rb = throwableInstance.GetComponent<Rigidbody2D>();

            // Throw the throwable in the direction of the players mouse
            rb.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwSpeed;
            StartCoroutine(ThrowCooldown());
        }
    }

    private IEnumerator ThrowCooldown()
    {
        _canThrow = false;
        yield return new WaitForSeconds(throwCooldown);
        _canThrow = true;
    }
}
