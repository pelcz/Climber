using UnityEngine;
using System.Collections;

public class pickup : MonoBehaviour {

	private Transform player_ref;
	private brain brain_ref;
	public GameObject impact_part;
	private float speed = 20f;
	private float collectDis = 0.4f; //dis to actually pick up/destroy coin
	private bool shouldFollow;
	private float pickUpDis = 1.75f; //radius of trigger
	private AudioClip collectSound;

	void Start()
	{
		player_ref =  GameObject.FindGameObjectWithTag("Player").transform;
		brain_ref =  Camera.main.GetComponent<brain>();
		collectSound = brain_ref.collectSound;
		GetComponent<CircleCollider2D> ().radius = pickUpDis;
	}

	//ENTER
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Player") //if player is alive
		{
			shouldFollow = true;
		}
	}

	void FixedUpdate()
	{
		if(shouldFollow)
			follow ();
	}

	void follow()
	{
		//speed = speed + 5f * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, player_ref.transform.position, speed * Time.fixedDeltaTime);
		//transform.position = Vector2.Lerp (transform.position, player_ref.transform.position, speed * Time.fixedDeltaTime);
		if (Vector2.Distance (transform.position, player_ref.transform.position) < collectDis)
			collect ();
	}

	void collect()
	{
		AudioSource.PlayClipAtPoint(collectSound, transform.position);
		brain.addPoints(1); //add 1 point
		GameObject.Instantiate(impact_part, transform.position, Quaternion.identity);
		//bar_glow.glow ("add");
		Destroy(transform.gameObject); //destroy this
	}
}
