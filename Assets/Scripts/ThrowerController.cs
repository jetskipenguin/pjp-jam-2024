using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : MonoBehaviour
{
    [Header("Hand References")]
    [SerializeField] private GameObject animatedHands;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    
    [Header("MonoBehavior References")]
    [SerializeField, Tooltip("Reference to the TrajectoryLine script")]
    private TrajectoryLine _trajectoryLine;

    [Header("Thrower Settings")]
    [SerializeField, Tooltip("List of objects that can be thrown")]
    public List<GameObject> throwables = new List<GameObject>();

    [SerializeField, Tooltip("Speed of the throw")]
    public float throwSpeed = 10f;

    [SerializeField, Tooltip("Length of cooldown between throws (sec)")]
    public float throwCooldown = 0.5f;

    [SerializeField, Tooltip("Speed of rotation of the throwable")]
    public float rotationSpeed = 5f;

    private Collider2D _playerCollider;
    private bool _canThrow = true;
    private List<Collider2D> _throwableColliders = new List<Collider2D>();
    private GameObject _throwableInstance;
    private Rigidbody2D _throwableRigidbody;

    public void OnEnable()
    {
        _playerCollider = GetComponent<Collider2D>();
        initializeThrowable();
    }

    public void Update()
    {
        // If throwable is not thrown, follow player
        if (_throwableInstance != null && _canThrow)
        {
            _throwableInstance.transform.position = transform.position; // TODO: add offset for player hands
            RotateThrowableOnInput();
            _trajectoryLine.PlotTrajectoryLine(_throwableRigidbody, _throwableInstance.transform.position, throwSpeed);
        }

        // Throw the throwable
        if (Input.GetButtonDown("Throw") && _canThrow)
        {
            _canThrow = false;
            _trajectoryLine.ClearTrajectoryLine();

            // Throw the throwable in the direction of the players mouse
            _throwableRigidbody.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwSpeed;
            Debug.Log("Applying Velocity to Throwable: " + _throwableRigidbody.velocity);

            StartCoroutine(TurnOnColliderOnceOutsideOfThrower());
            StartCoroutine(ThrowCooldown());
        }

        updateHands();
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
        while (AreAnyCollidersIntersecting(_throwableColliders, _playerCollider))
        {
            yield return null;
        }

        // Once outside, enable the collider
        foreach (Collider2D collider in _throwableColliders)
        {
            collider.enabled = true;
        }
    }

    // Function to check if a collider is completely outside another collider's bounds
    private bool AreAnyCollidersIntersecting(List<Collider2D> colliders, Collider2D targetCollider)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.bounds.Intersects(targetCollider.bounds))
            {
                return true;
            }
        }
        return false;
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
        _throwableRigidbody = _throwableInstance.GetComponent<Rigidbody2D>();

        // Check if the throwable has a collider
        _throwableColliders = new List<Collider2D>();

        Collider2D parentCollider = _throwableInstance.GetComponent<Collider2D>();

        // check if parent collider exists
        if(parentCollider && !parentCollider.isTrigger)
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

        animatedHands.SetActive(false);
    }

    private void updateHands()
    {
        if (_throwableInstance == null || !_canThrow) {
            animatedHands.SetActive(true);
            leftHand.SetActive(false);
            rightHand.SetActive(false);
            return;
        }

		leftHand.SetActive(true);
		rightHand.SetActive(true);

		//Collider2D col = _throwableInstance.GetComponentInChildren<Collider2D>();
        SpriteRenderer sprite = _throwableInstance.GetComponent<SpriteRenderer>();

		rightHand.transform.position = new Vector2(sprite.bounds.min.x, sprite.bounds.center.y);
		leftHand.transform.position = new Vector2(sprite.bounds.max.x, sprite.bounds.center.y);

    }
}
