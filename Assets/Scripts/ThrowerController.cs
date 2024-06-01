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
    private List<Collider2D> _throwableColliders = new List<Collider2D>();
    private GameObject _throwableInstance;

    public void OnEnable()
    {
        initializeThrowable();
    }

    public void Update()
    {
        // If throwable is not thrown, follow player
        if (_throwableInstance != null && _canThrow)
        {
            _throwableInstance.transform.position = transform.position; // TODO: add offset for player hands
        }

        // Throw the throwable
        if (Input.GetButtonDown("Throw") && _canThrow)
        {
            _canThrow = false;
            Rigidbody2D rb = _throwableInstance.GetComponent<Rigidbody2D>();

            // Throw the throwable in the direction of the players mouse
            rb.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwSpeed;
            Debug.Log("Applying Velocity to Throwable: " + rb.velocity);

            StartCoroutine(TurnOnColliderOnceOutsideOfThrower());
            StartCoroutine(ThrowCooldown());
        }
    }

    private IEnumerator TurnOnColliderOnceOutsideOfThrower()
    {
        // Wait until the throwable is outside the thrower's transform to turn on collider
        while (Vector3.Distance(_throwableInstance.transform.position, transform.position) < 0.1f)
        {
            yield return null;
        }

        // Once outside, enable the collider
        foreach (Collider2D collider in _throwableColliders)
        {
            collider.enabled = true;
        }
    }

    private IEnumerator ThrowCooldown()
    {
        _canThrow = false;
        yield return new WaitForSeconds(throwCooldown);
        _canThrow = true;
        initializeThrowable();
    }

    private void initializeThrowable()
    {
        if(!_canThrow)
        {
            return;
        }

        // Random throwable
        GameObject selectedThrowable = throwables[Random.Range(0, throwables.Count)];

        // Instantiate the throwable
        _throwableInstance = Instantiate(selectedThrowable, transform.position, Quaternion.identity);
        _throwableColliders = new List<Collider2D> { _throwableInstance.GetComponent<Collider2D>() };

        if (!_throwableColliders[0])
        {
            var colliders = _throwableInstance.GetComponentsInChildren<Collider2D>();
            _throwableColliders = new List<Collider2D>(colliders);
        }
        
        foreach (Collider2D collider in _throwableColliders)
        {
            collider.enabled = false;
        }
    }
}
