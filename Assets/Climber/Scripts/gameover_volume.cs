using UnityEngine;
using System.Collections;

public class gameover_volume : MonoBehaviour {

	//ENTER
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Dead")
		{
			Application.LoadLevel(0); //reset game
		}
	}
}
