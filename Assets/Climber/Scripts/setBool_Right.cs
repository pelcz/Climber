using UnityEngine;
using System.Collections;

public class setBool_Right : MonoBehaviour {

	private ec_player player_ref;

	void Start()
	{
		player_ref = transform.parent.GetComponent<ec_player> ();
	}

	//ENTER
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Wall" && player_ref.tag == "Player")
		{
			player_ref.onRight = true;
			player_ref.Flip("right");
			player_ref.impact();
		}
	}
	//STAY
	void OnTriggerStay2D (Collider2D col)
	{
		if(col.tag == "Wall" && player_ref.tag == "Player")
		{
			player_ref.onRight = true;
		}
	}
	//EXIT
	void OnTriggerExit2D (Collider2D col)
	{
		if(col.tag == "Wall" && player_ref.tag == "Player")
		{
			player_ref.onRight = false;
		}
	}
}
