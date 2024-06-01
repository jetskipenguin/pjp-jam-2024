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

    [SerializeField, Tooltip("Speed of rotation of the throwable")]
    public float rotationSpeed = 5f;

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
            RotateThrowableOnInput();
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

    private void RotateThrowableOnInput()
    {
        // Rotate throwable
        float scrollInput = Input.GetAxis("Mouse ScrollWheel"); 
        if (scrollInput != 0f)
        {
            // Adjust rotation based on scroll wheel input
            float rotationAmount = scrollInput * rotationSpeed; // Adjust rotation speed as needed
            _throwableInstance.transform.Rotate(Vector3.forward, rotationAmount);
        }

        // Use Q and E as alternative input
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _throwableInstance.transform.Rotate(Vector3.forward, 45);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            _throwableInstance.transform.Rotate(Vector3.forward, -45);
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

        // Check if the throwable has a collider
        _throwableColliders = new List<Collider2D>();

        Collider2D parentCollider = _throwableInstance.GetComponent<Collider2D>();

        // check if parent collider exists
        if(parentCollider)
        {
            _throwableColliders.Add(parentCollider);
        }
        // If not, get colliders from children
        else
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
