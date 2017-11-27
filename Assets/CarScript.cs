using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rotation = 0;
		prevDirection = 0;
		direction = 0;
		inputX = 0;
		turnRight = false;
		turnLeft = false;
		prevX = -10000;
		isDead = false;
		quaRotation = Quaternion.identity;
		carDefaultYPos = -3.4f;
		onlyCarMoves = false;
		speed = 0;
		max_speed = 200;
		score = 0;
		stopped = true;
		crashAnimationTimer = 0;
		finished = false;
		rotationSpeed = 210f;

	}

	public float direction;
	public float prevDirection;
	public float prevX;

	public float rotation;
	public Quaternion quaRotation;

	static public bool isDead;
	static public bool stopped;
	static public bool finished;

	static public bool onlyCarMoves;

	static public bool turnRight;
	static public bool turnLeft;

	static public int score;

	private float inputX;
	// 2 - Store the movement
	static public Vector2 movement;

	public List<float> rotationList; // For drifting
	float delayTimer;

	public float speed;
	static private float max_speed;
	static private float rotationSpeed;

	public static float getMaxSpeed() {return max_speed;}

	private float carDefaultYPos;

	private float crashAnimationTimer;

	public static void setMaxSpeed(float newMax)
	{
		if (SelectedLevelScript.selectedCar == 2) 
		{
			newMax *= 0.85f;
		}
		max_speed = newMax;
		rotationSpeed = 210 * newMax / 250;

	}
	// Update is called once per frame
	void Update () {

		if (rigidbody2D.position.x < BackgroundScript.stage_bounds.min.x || rigidbody2D.position.x > BackgroundScript.stage_bounds.max.x
						|| rigidbody2D.position.y < BackgroundScript.stage_bounds.min.y || rigidbody2D.position.y > BackgroundScript.stage_bounds.max.y)
			handleCrash ();

		if (isDead || stopped)
		{
			crashAnimation();
			movement = new Vector2(0,0);
			rotation = 0;
			direction = 0;
			return;
		}

		float turnAmount = 0.05f;
		//if(inputX!=0)
		//	turnAmount = 0.020f;
		if(turnRight)
			inputX += turnAmount;
		else if(turnLeft)
			inputX -= turnAmount;
		else
			inputX = 0;

		float maxTurn = 1f;
		if (inputX < -maxTurn)
			inputX = -maxTurn;
		if (inputX > maxTurn)
			inputX = maxTurn;

		const float deg2rad = 0.0174532925f;

		if (Math.Abs (inputX) > 0.001f) {
			rotation = -(inputX * rotationSpeed * Time.deltaTime);

			direction += rotation;
		} else 
		{
			rotation = 0;
		}
		if (rotation > 3 || rotation <-3) {
			if (SelectedLevelScript.selectedCar == 1) 
			{
				speed -= 100*Time.deltaTime;
				if(max_speed > 400)
				{
					if (speed < max_speed - 50)
						speed = max_speed - 50;
				}
				else if(max_speed >300)
				{
					if (speed < max_speed - 40)
						speed = max_speed - 40;
				}
				else
				{
					if (speed < max_speed - 30)
						speed = max_speed - 30;
				}
			}
			else if (SelectedLevelScript.selectedCar == 2) 
			{
				speed -= 100*Time.deltaTime;
				if(max_speed > 400)
				{
					if (speed < max_speed - 80)
						speed = max_speed - 80;
				}
				else if(max_speed >300)
				{
					if (speed < max_speed - 60)
						speed = max_speed - 60;
				}
				else
				{
					if (speed < max_speed - 50)
						speed = max_speed - 50;
				}
			}

		} else 
		{
			if (speed < max_speed)
				accelerate ();
		}

		if (direction < -180)
						direction += 360;
		if (direction > 180)
						direction -= 360;

		if (rigidbody2D.position.y < carDefaultYPos || direction<-90 || direction >90)
			onlyCarMoves = true;
		else
			onlyCarMoves = false;

		if (SelectedLevelScript.selectedCar == 1) 
		{
						// 4 - Movement per direction
			movement = new Vector2 (
				speed * (float)System.Math.Sin (deg2rad * -direction), speed * (float)System.Math.Cos (deg2rad * -direction));
		}
		else if (SelectedLevelScript.selectedCar == 2) 
		{
			rotationList.Add(direction);

			float currentRotationDirection = direction;
			float delayVal = 0.4f*250/max_speed;
			if(delayTimer < delayVal)
			{
				// Do not turn yet
				delayTimer += Time.deltaTime;
				
			}
			else
			{
				if(rotationList.Count > 0)
				{
					float rotItem = rotationList[0];
					if(rotItem-direction>120)
					{
						if(rotItem<0) rotItem += 360;
						else if(rotItem>0) rotItem -= 360;
					}
					else if(rotItem-direction<-120)
					{
						if(rotItem<0) rotItem += 360;
						else if(rotItem>0) rotItem -= 360;
					}

					currentRotationDirection = (2*rotItem+direction)/3.0f;
					rotationList.RemoveAt(0);
				}


			}
			// 4 - Movement per direction
			movement = new Vector2 (
				speed * (float)System.Math.Sin (deg2rad * -currentRotationDirection), speed * (float)System.Math.Cos (deg2rad * -currentRotationDirection));
			if (rigidbody2D.position.y < carDefaultYPos || currentRotationDirection<-90 || currentRotationDirection >90)
				onlyCarMoves = true;
			else
				onlyCarMoves = false;
		}
	}

	void crashAnimation()
	{
		if (!isDead)
			return;
		crashAnimationTimer += Time.deltaTime;
		float scaling = 10 * Time.deltaTime;

		transform.localScale = new Vector3(transform.localScale.x + scaling,transform.localScale.y+scaling,transform.localScale.z);
		transform.Rotate(0, 0, 60*scaling);

		if (crashAnimationTimer > 0.5) 
		{
			finished = true;
			Destroy(gameObject, 0.2f);
		}
	}
	void FixedUpdate()
	{
		if (isDead)
				rigidbody2D.velocity = Vector2.zero;
		else {
			if(onlyCarMoves)
			{
				Vector2 tmpMovement = new Vector2 (movement.x * Time.deltaTime, movement.y * Time.deltaTime);
				rigidbody2D.velocity = tmpMovement;
			}
			else
			{
				Vector2 tmpMovement = new Vector2 (movement.x * Time.deltaTime, 0);
				rigidbody2D.velocity = tmpMovement;
			}
			//transform.Rotate(0, 0, rotation);
			quaRotation.eulerAngles = new Vector3 (0, 0, direction);
			transform.rotation = quaRotation;

			checkScore();
		}

	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		handleCrash();
	}

	private void accelerate()
	{
		if (SelectedLevelScript.selectedCar == 1) 
		{
			speed += 200 * Time.deltaTime;
		}
		else if (SelectedLevelScript.selectedCar == 2) 
		{
			speed += 150 * Time.deltaTime;
		}
	}

	private void handleCrash()
	{
		isDead = true;
	}

	private void checkScore()
	{
		ObstacleScript[] obScripts = FindObjectsOfType<ObstacleScript> ();
		foreach (ObstacleScript oScript in obScripts) 
		{
			if(oScript.transform.position.y < gameObject.transform.position.y && oScript.id+1 > score)
				score = oScript.id+1;
		}
	}
}
