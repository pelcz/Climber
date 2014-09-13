using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public float minHeight = 0.9f;

	public Vector3 offset;			// The offset at which the Health Bar follows the player.
	
	private Transform player;		// Reference to the player.

	private Vector3 newPos;

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update ()
	{
		newPos = player.position;
		newPos.x = 0;
		if(newPos.y < minHeight) //limit how low the camera can go
		{
			newPos.y = minHeight;
		}
		// Set the position to the player's position with the offset.
		transform.position = newPos + offset;
	}
}
