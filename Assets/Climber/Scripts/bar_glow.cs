using UnityEngine;
using System.Collections;

public class bar_glow : MonoBehaviour {

	public float fadeSpeed = 0.05f;
	public Color addColour;
	public Color subtractColour;
	static Color addColourS;
	static Color subtractColourS;
	static private UISprite sprite_ref;

	// Use this for initialization
	void Start () 
	{
		sprite_ref = GetComponent<UISprite>();
		addColourS = addColour;
		subtractColourS = subtractColour;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Fade out glow effect
		if (sprite_ref.alpha > 0f)
			sprite_ref.alpha -= fadeSpeed;
	}

	static public void glow(string type)
	{
		//set a type/colour
		if(type=="add")
		{
			sprite_ref.color = addColourS;
		}
		else if(type=="sub")
		{
			sprite_ref.color = subtractColourS;
		}
		sprite_ref.alpha = 1f; //turn on glow
	}
}
