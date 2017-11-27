using UnityEngine;
using System.Collections;

public class LinesScript : MonoBehaviour {

	private float lineMovement;
	public volatile bool hasNext;

	// Use this for initialization
	void Start () {
		lineMovement = 0;
		hasNext = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (transform.position.y < 0 && !hasNext) 
		{
			hasNext = true;
			
			BackgroundScript[] bgScript = FindObjectsOfType<BackgroundScript> ();
			if(bgScript[0])
			{
				bgScript[0].CreateNextLines(transform.position.y);
			}
			
		}
		
		lineMovement = -CarScript.movement.y;
		if (CarScript.isDead)
			lineMovement = 0;
		
		if (transform.position.y < BackgroundScript.stage_bounds.min.y-10)
			Destroy (gameObject, 0.2f);
	}

	void FixedUpdate()
	{
		
		if (CarScript.isDead || CarScript.onlyCarMoves)
			rigidbody2D.velocity = Vector2.zero;
		else {
			Vector2 tmpMovement = new Vector2 (0, lineMovement * Time.deltaTime);
			rigidbody2D.velocity = tmpMovement;
		}
		//transform.Rotate(0, 0, rotation);
	}
}
