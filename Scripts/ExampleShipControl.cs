using UnityEngine;
using System.Collections;

public class ExampleShipControl : MonoBehaviour {

	public float acceleration_amount;
	public float rotation_speed;

	PlayerFire playerFire;
	PlayerHealth playerHealth;

	public bool bulletFire=true;
	public bool missileFire=false;

	private void Awake()
	{

	
		playerFire = FindObjectOfType<PlayerFire>();
		playerHealth = FindObjectOfType<PlayerHealth>();

		if (!PlayerPrefs.HasKey(gameObject.name + "speed"))
		{

			PlayerPrefs.SetFloat(gameObject.name + "speed", acceleration_amount);

		}

		else
		{

			acceleration_amount = PlayerPrefs.GetFloat(gameObject.name + "speed");
			print("speed="+acceleration_amount);
		}

	}

	void Update() {


		if (playerHealth.health > 0)
		{

			if (Input.GetKey(KeyCode.UpArrow))
			{
				GetComponent<Rigidbody2D>().AddForce(transform.up * acceleration_amount * Time.deltaTime);

			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				GetComponent<Rigidbody2D>().AddForce((-transform.up) * acceleration_amount * Time.deltaTime);

			}

			if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftShift))
			{
				GetComponent<Rigidbody2D>().AddTorque(-rotation_speed * Time.deltaTime);

			}
			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftShift))
			{
				GetComponent<Rigidbody2D>().AddTorque(rotation_speed * Time.deltaTime);

			}
			if (Input.GetKey(KeyCode.C))
			{
				GetComponent<Rigidbody2D>().angularVelocity = Mathf.Lerp(GetComponent<Rigidbody2D>().angularVelocity, 0, rotation_speed * 0.06f * Time.deltaTime);
				GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(GetComponent<Rigidbody2D>().velocity, Vector2.zero, acceleration_amount * 0.06f * Time.deltaTime);
			}


			if (Input.GetKeyDown(KeyCode.F))
			{
		
               playerFire.isFiring = true;
         
			}

			if (Input.GetKeyUp(KeyCode.F))
			{

				playerFire.isFiring = false;

			}

		}
    }
}
