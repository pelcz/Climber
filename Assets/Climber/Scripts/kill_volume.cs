using UnityEngine;
using System.Collections;

public class kill_volume : MonoBehaviour {

	private ec_player player_ref;
	
	void Start()
	{
		player_ref =  GameObject.FindGameObjectWithTag("Player").GetComponent<ec_player>();
	}
	
	//ENTER
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Player")
		{
			player_ref.Death();
		}
	}
}
