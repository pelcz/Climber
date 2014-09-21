using UnityEngine;
using System.Collections;

public class brain : MonoBehaviour {

	//Tile list (types of tiles to be randomly chosen from)
	public GameObject[] tile_list_editor; //Tiles added through editor
	private static GameObject[] tile_list;
	//Last tile (tile that a new tile will be spawned above)
	public GameObject lastTile_editor; //First time should be set through editor for now
	private static GameObject lastTile;
	//Height to spawn next tile above lastTile
	private static float tile_height = 18f;
	public static int tileCount = 0;
	//Points
	public static int totalPoints = 0; //total points earned for all rounds
	public static int points = 0; //points per round
	public static int goal = 100;
	public int goalIncrease = 10;
	private static int coinDecrease = 10;
	private float pointCountTime = 0f;
	private float newRoundDelay = 0f;
	//UI
	public static GameObject Bar;
	public UISlider progressBar;
	public UILabel level_text;
	public static GameObject points_text;
	static int level = 1;
	//SOUNDS
	public AudioClip collectSound;
	public AudioClip levelUpSound;
	//SATES
	public enum gameState{starting,playing,paused,scoring};
	public static gameState currentState = gameState.starting;

	// Use this for initialization
	void Start () 
	{
		currentState = gameState.starting; //set initial gamestate
		Bar = GameObject.Find("bar");
		points_text = GameObject.Find("points");
		tile_list = tile_list_editor; //assign our static list to the editor list so that our static function can use it 
		lastTile = lastTile_editor; //assign our static lastTile to the editor one so that our static function can use it 
		coinDecrease = Mathf.RoundToInt(goal/10);
		tileCount = 0; //reset tile count on new round
		//Call initial tile spawn
		spawnTile();
	}
	
	// Update is called once per frame
	void Update () 
	{
		///////////////// STATES ///////////////
		switch (currentState) 
		{
			case gameState.starting: //**STARTING**//
				currentState = gameState.playing; //go to playing
			break;
			case gameState.playing: //**PLAYING**//
				//update point counter
				points_text.GetComponent<UILabel>().text = points.ToString ();
			break;
			case gameState.scoring: //**SCORING**//
				if(Bar.GetComponent<UISprite> ().alpha<1f)
				{
					Bar.GetComponent<UISprite> ().alpha += 0.1f; //make the bar visible
				}
				points_text.GetComponent<UILabel>().text = points.ToString ();//update point counter

				//reduce points and add to total points over time
				pointCountTime+=Time.deltaTime;
				if(pointCountTime>=0.02f && points>0)
				{
					points -= 1; //remove from points (counter)
					totalPoints += 1; //add to total points (bar)
					audio.PlayOneShot(collectSound);
					//bar_glow.glow("add");
					points_text.GetComponent<TweenScale> ().ResetToBeginning ();
					points_text.GetComponent<TweenScale>().PlayForward();
					pointCountTime = 0f; //reset counter
				}

				//Update progress bar with total points
				float pointsF = totalPoints;
				float goalF = goal;
				progressBar.value = (float)(pointsF / goalF);
				level_text.text = level.ToString();

				//IF WE HIT GOAL THEN LEVEL UP
				if (totalPoints >= goal)
				{
					levelUp();
				}

				//end round when done counting
				if(points==0)
				{
					newRoundDelay += Time.deltaTime;
					if(newRoundDelay>2f)
						newRound();
				}
			break;
			case gameState.paused: //**PAUSED**//
				//do nothing
			break;
		}
	}

	//Spawn a new random tile above the last one
	public static void spawnTile()
	{
		GameObject newTile;
		int ran = Random.Range (0, tile_list.Length);

		//only spawn 5 (arrow) after passing 10 tiles
		if (ran == 5 && tileCount<10)
			ran = 0;

		GameObject chosenTile = tile_list[ran]; //pick random tile from given list
		Vector3 newPos = lastTile.transform.position;
		newPos.y += tile_height; //Set new tiles position above the last tiles position

		//Spawn tile
		newTile = GameObject.Instantiate (chosenTile, newPos, Quaternion.identity) as GameObject; 
		lastTile = newTile; //Assign last tile to the new tile that was created
	}

	//Add Points
	public static void addPoints(int amount)
	{
		points += amount;//add to points counter
		points_text.GetComponent<TweenScale> ().ResetToBeginning ();
		points_text.GetComponent<TweenScale>().PlayForward();
		//cap points
//		if (points < 0)
//		{
//			points = 0;
//		}
//		if (points > goal)
//		{
//			points = goal;
//		}
	}

	//Remove Points (for dying)
	public static void died()
	{
		points -= coinDecrease;//remove points
		//cap points
		if (points < 0)
		{
			points = 0;
		}
		if (points > goal)
		{
			points = goal;
		}
	}

	//TURN ON BAR
	public static void showBar()
	{
		currentState = gameState.scoring;
	}

	//NEW ROUND
	void newRound()
	{
		Application.LoadLevel(0); //reset game
	}

	//LEVEL UP!!!
	void levelUp()
	{
		//reset and increase goal
		totalPoints = 0;
		goal += goalIncrease; //increase amount of coins needed to meet next goal (levelUp)
		coinDecrease = Mathf.RoundToInt(goal/10); //increase coin amount removed on death (10% of current goal)
		level+=1;
		level_glow.glow();
		AudioSource.PlayClipAtPoint(levelUpSound, transform.position); //play level up sound
	}
}
