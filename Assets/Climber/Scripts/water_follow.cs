using UnityEngine;
using System.Collections;

public class water_follow : MonoBehaviour {

	Vector3 last_pos;
	float orig_dis;
	private ec_player player_ref;

	// Use this for initialization
	void Start () {
		orig_dis = Vector3.Distance(transform.position, Camera.main.transform.position);
		last_pos = transform.position;
		player_ref =  GameObject.FindGameObjectWithTag("Player").GetComponent<ec_player>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.y>last_pos.y)
		{
			last_pos = transform.position;
		}

		if(Vector3.Distance(transform.position, Camera.main.transform.position)<orig_dis && player_ref.transform.rigidbody2D.velocity.y>0f && player_ref.transform.tag=="Player")
		{
			last_pos.y-=0.025f;
		}


		transform.position = last_pos;
	}
}
