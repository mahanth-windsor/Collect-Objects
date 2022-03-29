using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;
	public TextMeshProUGUI gameInstructions;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private string[] INSTRUCTIONS = { "Orange", "Green", "Purple", "Blue" };
	private int count;
	private float groundDimension = 9.4f;
	private float groundScale = 2;

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";

		//set a random instruction color at the begining
		System.Random random = new System.Random();
		gameInstructions.text = INSTRUCTIONS[random.Next(0, INSTRUCTIONS.Length)];
		Debug.Log("Start : " );

		
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		rb.AddForce (movement * speed);

		if (count < -5)
		{

			winText.text = "Game Over!!!";

			Invoke("Restart", 2f);

		}

	}

	void Restart()
	{
		SceneManager.LoadScene(0);
	}



	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	IEnumerator OnTriggerEnter(Collider other) 
	{
		
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag(gameInstructions.text))
		{
			//changePositionDisappear(other);

			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive(false);

			// Add one to the score variable 'count'
			count++;

			return transfromObjectPosition(other);

		}

		else
		{
			other.gameObject.SetActive(false);

			count--;

			

			return transfromObjectPosition(other);
		}

	}

	IEnumerator transfromObjectPosition (Collider other)
	{
		// Run the 'SetCountText()' function (see below)
		SetCountText();

		float sideLength = groundDimension * groundScale;

		other.gameObject.transform.position = 
			new Vector3(Random.Range( -1 * sideLength, sideLength), (float)0.5 * groundScale, Random.Range(-1 * sideLength, sideLength));
		//Print the time of when the function is first called.
		Debug.Log("Started Coroutine at timestamp : " + Time.time);

		//yield on a new YieldInstruction that waits for 5 seconds.
		yield return new WaitForSeconds(Mathf.CeilToInt(Random.Range(1.0f, 4.0f)));

		//After we have waited 5 seconds print the time again.
		Debug.Log("Finished Coroutine at timestamp : " + Time.time);

		other.gameObject.SetActive(true);
	}

	void SetGameInstructions()
	{
		System.Random random = new System.Random();
		int value = random.Next(0, INSTRUCTIONS.Length);
		gameInstructions.text = INSTRUCTIONS[value];
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		if (count % 3 == 0)
		{
			SetGameInstructions();
		}

	}
}