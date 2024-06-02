using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeleter : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Block")) { return; }
		if(transform.parent != null)
		{
			Destroy(collision.transform.parent.gameObject);
		}
		else
		{
			Destroy(collision.gameObject);
		}
	}
}
