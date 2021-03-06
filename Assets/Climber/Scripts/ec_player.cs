﻿using UnityEngine;
using System.Collections;

public class ec_player : MonoBehaviour
{
	//Speeds
	public float flingSpeed = 2f;		// The speed the enemy moves at.
	public float flingHeight = 1f;		// The speed the enemy moves at.
	public float deathHop = 1000f;		// The speed the enemy moves at.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public float deathSpinMin = -100f;			// A value to give the minimum amount of Torque when dying
	public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying
	public float rotateSpeed = 1f;
	//Bools
	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private bool dead = false;			// Whether or not the enemy is dead.
	public bool onLeft;
	public bool onRight;
	public bool onGround = true;
	public bool waiting = true;

	//Mouse Inpput
	private bool tapping;
	private Vector3 tapPosition;
	private Vector3 lastTapPosition;
	private float tapTime = 0f;
	public float minSwipeDis = 1.3f;
	//Audio
	public AudioClip jumpSound;
	public AudioClip deathSound;
	public AudioClip impactSound;
	public GameObject death_part;
	public GameObject impact_part;
	public bool inAir = false;

	public GameObject ground_ref;
	private float deathCountDown = 2f;
	//Boost
	public float speedBoost = 0f;
	public float speedBoostCap = 1100f;
	public float speedBoostAmount = 1f;

	private Transform body;

	//AWAKE//
	void Awake()
	{
		// Setting up the references.
		body = transform.FindChild ("body").transform;
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
	}

	//UPDATE//
	void Update()
	{
		getInput(); //get player input

		//Assign inAir variable for effects and shit
		if (!onGround && !onLeft && !onRight)
			inAir = true;
		else
			inAir = false;

		//Rotate to direction the player is flying if in the air
		if(inAir && !dead)
		{
			//Handle rotation to trajectory
			float angle = Mathf.Atan (rigidbody2D.velocity.y/rigidbody2D.velocity.x); // Find angle in radians
			if((rigidbody2D.velocity.y != 0)||(rigidbody2D.velocity.x != 0))
			{
				angle *= Mathf.Rad2Deg; // Convert to degrees
				Debug.Log(angle);
				if(rigidbody2D.velocity.x==0)
					angle = 0f;
			}
			else
			{
				angle = 0f;
			}
			//FLIP UP AND DOWN WHEN RISING OR FALLING
			if(rigidbody2D.velocity.y>0)
			{
				Vector3 newScale = body.transform.localScale;
				newScale.y = 1.335934f;
				body.transform.localScale = newScale;
			}
			else if(rigidbody2D.velocity.y<0)
			{
				Vector3 newScale = body.transform.localScale;
				newScale.y = -1.335934f;
				body.transform.localScale = newScale;
			}
			// Assign rotation
			body.transform.eulerAngles = new Vector3(0f, 0f, -angle);
		}

		//OnGround
		if(onGround && !onLeft && !onRight)
		{
			body.transform.eulerAngles = new Vector3(0f, 0f, 0f); // Assign rotation to flat
		}

		//Turn off collider when under water
		if(dead)
		{
			deathCountDown-=Time.deltaTime;
			if(deathCountDown<-0f)
				brain.showBar();
				//Application.LoadLevel(0); //reset game
			//breaks because of wall colliders get turned off
			//GetComponent<BoxCollider2D> ().isTrigger=true;
		}
	}

	//DEATH//
	public void Death()
	{
		Camera.main.GetComponent<FollowPlayer> ().enabled = false;

		Debug.Log("DIE");
		// Find all of the sprite renderers on this object and it's children.
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}

		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		ren.enabled = true;
		ren.sprite = deadEnemy;

		// Set dead to true.
		dead = true;

		// Allow the enemy to rotate and spin it by adding a torque.
		rigidbody2D.fixedAngle = false;
		rigidbody2D.AddTorque(Random.Range(deathSpinMin,deathSpinMax));

		rigidbody2D.AddForce(new Vector2(0f, deathHop)); //bounce up

		transform.tag = "Dead";
		Camera.main.audio.Stop ();
		AudioSource.PlayClipAtPoint(deathSound, transform.position);

		GameObject.Instantiate (death_part, transform.position, Quaternion.identity);

		//temp removal of ground collision to fix bug where dying at the beggining will make the player stuck
		ground_ref.GetComponent<BoxCollider2D> ().enabled = false;
		//brain.died (); //remove coins
		//bar_glow.glow ("sub"); //play red glow
	}

	//IMPACT//
	public void impact()
	{
		AudioSource.PlayClipAtPoint(impactSound, transform.position);
		//should play smoke puff
		//GameObject.Instantiate(impact_part, transform.position, Quaternion.identity);
	}

	//FLIP//
	public void Flip(string side)
	{
		//play tween
		body.GetComponent<TweenScale> ().ResetToBeginning ();
		body.GetComponent<TweenScale>().PlayForward();
		if(side == "right")
		{
			// Multiply the x component of localScale by -1.
			//Vector3 newScale = transform.FindChild("body").transform.localScale;
			//newScale.x = -2.5f;
			//transform.FindChild("body").transform.localScale = newScale;
			body.transform.eulerAngles = new Vector3(0f, 0f, 90f); // Assign rotation
		}
		if(side == "left")
		{
			// Multiply the x component of localScale by -1.
			//Vector3 newScale = transform.FindChild("body").transform.localScale;
			//newScale.x = 2.5f;
			//transform.FindChild("body").transform.localScale = newScale;
			body.transform.eulerAngles = new Vector3(0f, 0f, 270f); // Assign rotation
		}
	}

	//GET INPUT FROM PLAYER//
	public void getInput()
	{
		if (Input.GetMouseButtonDown(0)) // && GetComponent<game_flow>().currentState!=game_flow.States.watching
		{
			tapping = true; //start tapping
			tapPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if(tapTime<0.9f && !dead) //time check for valid swipe and not dead
			{
				float dis = Mathf.Abs(Vector3.Distance(lastTapPosition, tapPosition));
				if(dis>minSwipeDis) //GOT VALID SWIPE
				{
					float xdis = tapPosition.x-lastTapPosition.x;
					//float ydis = tapPosition.y-lastTapPosition.y;
					//float dir = Vector3.Angle(tapPosition, lastTapPosition);

					if(onRight || onGround) //FLING LEFT
					{
						if(xdis>0)
						{
							waiting = false;
							//Debug.Log("fling left");
							//reset wall bools
							onLeft = false;
							onRight = false;
							onGround = false;

							//Apply Fling!
							speedBoost = rigidbody2D.velocity.y*speedBoostAmount;
							if(speedBoostAmount>speedBoostCap)
							{
								speedBoostAmount = speedBoostCap;
							}
							rigidbody2D.velocity = new Vector2(0f,0f); //null out velocity before applying force
							rigidbody2D.AddForce(new Vector2(-1f * flingSpeed, 1 * (flingHeight+speedBoost)), ForceMode2D.Force);
							AudioSource.PlayClipAtPoint(jumpSound, transform.position, 0.8f);
							if(Camera.main.audio.isPlaying==false)
								Camera.main.audio.Play();
						}
					}
					if(onLeft || onGround) //FLING RIGHT
					{
						if(xdis<0)
						{
							waiting = false;
							//Debug.Log("fling right");
							//reset wall bools
							onLeft = false;
							onRight = false;
							onGround = false;
							
							//Apply Fling!
							speedBoost = rigidbody2D.velocity.y*speedBoostAmount;
							if(speedBoostAmount>speedBoostCap)
							{
								speedBoostAmount = speedBoostCap;
							}
							rigidbody2D.velocity = new Vector2(0f,0f); //null out velocity before applying force
							rigidbody2D.AddForce(new Vector2(1f * flingSpeed, 1 * (flingHeight+speedBoost)), ForceMode2D.Force);
							AudioSource.PlayClipAtPoint(jumpSound, transform.position, 0.8f);
							if(Camera.main.audio.isPlaying==false)
								Camera.main.audio.Play();
						}
					}
					if(inAir) //Flail to left or right if in air
					{
						if(xdis>0)//Left
						{	
							//Apply Fling!
							rigidbody2D.AddForce(new Vector2(-1f * flingSpeed/6, 45f));
						}
						if(xdis<0)//Right
						{	
							//Apply Fling!
							rigidbody2D.AddForce(new Vector2(1f * flingSpeed/6, 45f));
						}
					}
				}
			}
			
			tapping = false; //stop tapping
			tapTime = 0f;
		}
		
		if(tapping) //if finger is on screen, count how long it's been pressing
		{
			tapTime+=Time.deltaTime;
			lastTapPosition = Input.mousePosition;
		}

		//KEYBOARD CONTROLS//
		if(!dead)
		{
			if(onRight || onGround) //FLING LEFT
			{
				if(Input.GetKeyDown(KeyCode.LeftArrow))
				{
					waiting = false;
					//Debug.Log("fling left");
					//reset wall bools
					onLeft = false;
					onRight = false;
					onGround = false;

					//Apply Fling!
					speedBoost = rigidbody2D.velocity.y*speedBoostAmount;
					if(speedBoostAmount>speedBoostCap)
					{
						speedBoostAmount = speedBoostCap;
					}
					rigidbody2D.velocity = new Vector2(0f,0f); //null out velocity before applying force
					rigidbody2D.AddForce(new Vector2(-1f * flingSpeed, 1 * (flingHeight+speedBoost)), ForceMode2D.Force);
					AudioSource.PlayClipAtPoint(jumpSound, transform.position, 0.8f);
					if(Camera.main.audio.isPlaying==false)
						Camera.main.audio.Play();
				}
			}
			if(onLeft || onGround) //FLING RIGHT
			{
				if(Input.GetKeyDown(KeyCode.RightArrow))
				{
					waiting = false;
					//Debug.Log("fling right");
					//reset wall bools
					onLeft = false;
					onRight = false;
					onGround = false;

					//Apply Fling!
					speedBoost = rigidbody2D.velocity.y*speedBoostAmount;
					if(speedBoostAmount>speedBoostCap)
					{
						speedBoostAmount = speedBoostCap;
					}
					rigidbody2D.velocity = new Vector2(0f,0f); //null out velocity before applying force
					rigidbody2D.AddForce(new Vector2(1f * flingSpeed, 1 * (flingHeight+speedBoost)), ForceMode2D.Force);
					AudioSource.PlayClipAtPoint(jumpSound, transform.position, 0.8f);
					if(Camera.main.audio.isPlaying==false)
						Camera.main.audio.Play();
				}
			}
			if(inAir) //Flail to left or right if in air
			{
				if(Input.GetKeyDown(KeyCode.LeftArrow))//Left
				{	
					//Debug.Log("fling left flailing");
					//Apply Fling!
					rigidbody2D.AddForce(new Vector2(-1f * flingSpeed/6, 45f));
				}
				if(Input.GetKeyDown(KeyCode.RightArrow))//Right
				{	
					//Debug.Log("fling right flailing");
					//Apply Fling!
					rigidbody2D.AddForce(new Vector2(1f * flingSpeed/6, 45f));
				}
			}
		}
	}
}
