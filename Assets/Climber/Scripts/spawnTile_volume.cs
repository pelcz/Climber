using UnityEngine;
using System.Collections;

public class spawnTile_volume : MonoBehaviour {

	private ec_player player_ref;
	private bool triggered = false;
	
	void Start()
	{
		player_ref =  GameObject.FindGameObjectWithTag("Player").GetComponent<ec_player>();
	}
	
	//ENTER - Spawn new tile
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Player") //if player is alive
		{
			if(triggered==false)
			{
				brain.spawnTile(); //spawn a new tile
				triggered = true;
			}
		}
	}

	//EXIT - Destroy this tile
	void OnTriggerExit2D (Collider2D col)
	{
		if(col.tag == "Player") //if player is alive
		{
			if(player_ref.transform.position.y>transform.position.y) //if the players height is higher than ours
			{
				Destroy(transform.gameObject); //destroy this tile
			}
		}
	}
}
