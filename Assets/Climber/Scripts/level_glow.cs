using UnityEngine;
using System.Collections;

public class level_glow : MonoBehaviour {

	public float fadeSpeed = 0.05f;
	public Color levelUpColour;
	static Color levelUpColourS;
	static private UISprite sprite_ref;

	// Use this for initialization
	void Start () 
	{
		sprite_ref = GetComponent<UISprite>();
		levelUpColourS = levelUpColour;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Fade out glow effect
		if (sprite_ref.alpha > 0f)
			sprite_ref.alpha -= fadeSpeed;
	}

	static public void glow(string type = "levelup")
	{
		//set a type/colour
		if(type=="levelup")
		{
			sprite_ref.color = levelUpColourS;
		}
		sprite_ref.alpha = 1f; //turn on glow
	}
}
