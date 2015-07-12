using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class TeamPlayer : MonoBehaviour
	{
		
		[HideInInspector]
		public bool facingRight = true;			// For determining which way the player is currently facing.
		public float moveForce = 2000f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
		
		public float maxJumpSpeed = 500f;				// The fastest the player can travel in the x axis.

		
		private int defaultGravity = 8; // the normal gravity when not climbing a ladder


		private int nbJump = 0;
		private Transform groundCheck;			// A position marking where to check if the player is grounded.
		//private bool grounded = false;			// Whether or not the player is grounded.
		private bool jump = false;			// true if we want to jump
		
		private bool jumpCollision = false;
		private bool jumpKeyDown = false;


		private bool canClimb = false;

		public GameObject currentGun;
		
		void Awake()
		{
			// Setting up references.
			groundCheck = transform.Find("groundCheck");
		}
		
		// Use this for initialization
		void Start()
		{
		}
		
		// update fixed at 50times per sec
		void FixedUpdate()
		{
			Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
		
			updateClimb (rigidbody);

			if (!jumpCollision)
			{
				updateHorizontalMove (rigidbody);
			}
			//
			if (jump)
			{
				//if (GetComponent<Rigidbody2D>().velocity.y < maxJumpSpeed)
				//{
				//	GetComponent<Rigidbody2D>().AddForce(Vector2.up *  moveForce * 20);
				//}
				//else
				{
					rigidbody.AddForce(Vector2.up *
					                                     (10000 + 2000 * nbJump - Math.Min(20, GetComponent<Rigidbody2D>().velocity.y) * 500));
					print("JUMP velocity : " + GetComponent<Rigidbody2D>().velocity.y + " => " +
					      (10000 + 2000 * nbJump  - Math.Min(20, GetComponent<Rigidbody2D>().velocity.y) * 500));
					jump = false;
				}
			}
		}
		
		// Update is called once per frame
		void Update()
		{
			//// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
			//print("grounded? " + grounded);
			// If the jump button is pressed and the player is grounded then the player should jump.
			//if (Input.GetButtonDown("Jump") && grounded)
			//	jump = true;



			if (nbJump < 2 && Input.GetButton("Jump") && !jumpKeyDown)
			{
				jump = true;
				nbJump++;
				jumpKeyDown = true;
			}
			else if (!Input.GetButton("Jump") && !jump)
			{
				jumpKeyDown = false;
			}
		}
		
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;
			
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
			
			//if (currentGun != null)
			//{
			//	Vector3 position = currentGun.transform.position;
			//	position.x = -position.x;
			//	currentGun.transform.position = position;
			//}
		}

		void OnTriggerEnter2D(Collider2D collider)  {

			if (collider.tag == "ladder")
			{
				Debug.Log("start climb");
				canClimb = true;
			}
		}

		void OnTriggerExit2D(Collider2D collider)  {
			
			if (collider.tag == "ladder")
			{
				Debug.Log("stop climb");
				canClimb = false;
			}
		}

		void OnCollisionEnter2D(Collision2D collision)
		{


			if (nbJump > 1)
			{
				jumpCollision = true;
			}
			//print("collision : " + collision.relativeVelocity + " " +
			//	(collision.collider.bounds.center - transform.position) + " ; " + collision.collider.offset);
			if (collision.relativeVelocity.y < 0 && (collision.collider.bounds.center - transform.position).y < 0)
			{
				nbJump = 0;
				jumpCollision = false;
			}


		}

		private void updateHorizontalMove (Rigidbody2D rigidbody) {
			// Cache the horizontal input.
			float horizontalInput = Input.GetAxisRaw("Horizontal");
			
			if ((facingRight != (horizontalInput > 0)) && (horizontalInput != 0))
			{
				Flip();
			}

			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if (horizontalInput * rigidbody.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody.AddForce(Vector2.right * horizontalInput * moveForce);
			
			
			// If the player's horizontal velocity is greater than the maxSpeed...
			// ... set the player's velocity to the maxSpeed in the x axis.
			if (Mathf.Abs (rigidbody.velocity.x) > maxSpeed)
				rigidbody.velocity = new Vector2(maxSpeed * Mathf.Sign(rigidbody.velocity.x),
				                                 rigidbody.velocity.y);
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(h));
			
			if (horizontalInput == 0)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
		}

		private void updateClimb (Rigidbody2D rigidbody)
		{
			if (canClimb) {
				rigidbody.gravityScale = 0;
				
				float veritacalInput = Input.GetAxisRaw("Vertical");
				if (veritacalInput * rigidbody.velocity.y < maxSpeed)
					rigidbody.AddForce(Vector2.up * veritacalInput * moveForce);

				if (Mathf.Abs (rigidbody.velocity.y) > maxSpeed)
					rigidbody.velocity = new Vector2(rigidbody.velocity.x,
					                                 maxSpeed * Mathf.Sign(rigidbody.velocity.y));
				
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				//anim.SetFloat("Speed", Mathf.Abs(h));
				
				if (veritacalInput == 0)
				    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);

			} else {
				rigidbody.gravityScale = defaultGravity;
			}
		}
	}
}
