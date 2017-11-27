using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	
	private float obstacleMovement;

	public volatile bool hasNext;

	public int id;

	// Use this for initialization
	void Start () {
	
		obstacleMovement = 0;
		hasNext = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (transform.position.y < BackgroundScript.stage_bounds.max.y && !hasNext) 
		{
			hasNext = true;
			
			BackgroundScript[] bgScript = FindObjectsOfType<BackgroundScript> ();
			if(bgScript[0])
			{
				bgScript[0].CreateNextObstacle(transform.position.y);
			}
			
		}

		obstacleMovement = -CarScript.movement.y;
		if (CarScript.isDead)
			obstacleMovement = 0;

		if (transform.position.y < BackgroundScript.stage_bounds.min.y)
			Destroy (gameObject, 0.2f);
	}

	void FixedUpdate()
	{

		if (CarScript.isDead || CarScript.onlyCarMoves)
				rigidbody2D.velocity = Vector2.zero;
		else {
				Vector2 tmpMovement = new Vector2 (0, obstacleMovement * Time.deltaTime);
				rigidbody2D.velocity = tmpMovement;
		}
		//transform.Rotate(0, 0, rotation);
	}
}
