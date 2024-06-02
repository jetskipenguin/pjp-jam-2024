using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFreezer : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Block")) { return; }
		Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
		//If the rigid body is on the parent object
		if (!rb) { 
			rb = collision.GetComponentInParent<Rigidbody2D>();
		}
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}
}
