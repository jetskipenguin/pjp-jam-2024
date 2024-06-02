using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player") || collision.gameObject == this.gameObject)
        {
			return;	
        }
		// creates joint
		FixedJoint2D joint = collision.gameObject.AddComponent<FixedJoint2D>();

		// sets joint position to point of contact
		joint.anchor = collision.contacts[0].point;
		// conects the joint to the other object
		joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInChildren<Rigidbody2D>();
		// Stops objects from continuing to collide and creating more joints
		//joint.enableCollision = false;

	}
}
