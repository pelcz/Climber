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
	//Points
	public static int points = 0;
	public static int goal = 100;
	public int goalIncrease = 10;
	private static int coinDecrease = 10;
	//UI
	public UISlider progressBar;
	public UILabel level_text;
	static int level = 1;
	//SOUNDS
	public AudioClip collectSound;
	public AudioClip levelUpSound;

	// Use this for initialization
	void Start () 
	{
		tile_list = tile_list_editor; //assign our static list to the editor list so that our static function can use it 
		lastTile = lastTile_editor; //assign our static lastTile to the editor one so that our static function can use it 
		coinDecrease = Mathf.RoundToInt(goal/10);
		//Call initial tile spawn
		spawnTile();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Update progress bar with points
		float pointsF = points;
		float goalF = goal;
		progressBar.value = (float)(pointsF / goalF);
		level_text.text = level.ToString();
		if (points >= goal) //HIT GOAL /LEVEL UP!!
		{
			levelUp();
		}
	}

	//Spawn a new random tile above the last one
	public static void spawnTile()
	{
		GameObject newTile;
		GameObject chosenTile = tile_list[Random.Range(0,tile_list.Length)]; //pick random tile from given list
		Vector3 newPos = lastTile.transform.position;
		newPos.y += tile_height; //Set new tiles position above the last tiles position

		//Spawn tile
		newTile = GameObject.Instantiate (chosenTile, newPos, Quaternion.identity) as GameObject; 
		lastTile = newTile; //Assign last tile to the new tile that was created
	}

	//Add Points
	public static void addPoints(int amount)
	{
		points += amount;//add points
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

	//LEVEL UP!!!
	void levelUp()
	{
		//reset and increase goal
		points = 0;
		goal += goalIncrease; //increase amount of coins needed to meet next goal (levelUp)
		coinDecrease = Mathf.RoundToInt(goal/10); //increase coin amount removed on death (10% of current goal)
		level+=1;
		level_glow.glow();
		AudioSource.PlayClipAtPoint(levelUpSound, transform.position); //play level up sound
	}
}
