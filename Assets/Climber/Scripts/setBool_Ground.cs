using UnityEngine;
using System.Collections;

public class setBool_Ground : MonoBehaviour {

	private ec_player player_ref;
	
	void Start()
	{
		player_ref = transform.parent.GetComponent<ec_player> ();
	}
	
	//ENTER
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "ground" && player_ref.tag == "Player")
		{
			player_ref.onGround = true;
			//player_ref.impact();
		}
	}
	//STAY
	void OnTriggerStay2D (Collider2D col)
	{
		if(col.tag == "ground" && player_ref.tag == "Player")
		{
			player_ref.onGround = true;
		}
	}
	//EXIT
	void OnTriggerExit2D (Collider2D col)
	{
		if(col.tag == "ground" && player_ref.tag == "Player")
		{
			player_ref.onGround = false;
		}
	}
}
