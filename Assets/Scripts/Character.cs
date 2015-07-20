using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class Character : MonoBehaviour
	{
		
		[HideInInspector]
		public bool facingRight = true;			// For determining which way the player is currently facing.
		public float moveForce = 2000f;			// Amount of force added to move the player left and right.
		public float maxForwardSpeed = 10f;				// The fastest the player can travel in the x axis.
		public float maxBackwardSpeed = 3f;				// The fastest the player can travel in the x axis.
		public float maxJumpSpeed = 500f;				// The fastest the player can travel in the x axis.


		protected float verticalInput = 0;
		protected float horizontalInput = 0;
		protected bool jumpInput = false;
		
		protected int defaultGravity = 8; // the normal gravity when not climbing a ladder


		protected int nbJump = 0;
		protected int maxNbJump = 2;
		protected Transform groundCheck;			// A position marking where to check if the player is grounded.
		//private bool grounded = false;			// Whether or not the player is grounded.
		protected bool jumpWanted = false;			// true if we want to jump
		
		protected bool jumpCollision = false;
		protected bool jumpKeyDown = false;


		protected bool canClimb = false;

		protected Rigidbody2D rigidBody;

		public GameObject currentGun;
		
		protected void Awake()
		{
			// Setting up references.
			groundCheck = transform.Find("groundCheck");
		}
		
		// Use this for initialization
		protected void Start()
		{
			rigidBody = GetComponent<Rigidbody2D>();
		}
		
		// update fixed at 50times per sec
		protected void FixedUpdate()
		{
			// climb speed to beupdated
			// asociation with jump to be managed (climb = pas sauter)
			updateClimb (maxForwardSpeed);

			if (!jumpCollision)
			{
				updateHorizontalMove ();
			}
			//
			if (jumpWanted)
			{
				//if (GetComponent<Rigidbody2D>().velocity.y < maxJumpSpeed)
				//{
				//	GetComponent<Rigidbody2D>().AddForce(Vector2.up *  moveForce * 20);
				//}
				//else
				{
					rigidBody.AddForce(Vector2.up *
						(10000 + 2000 * nbJump - Math.Min(20, GetComponent<Rigidbody2D>().velocity.y) * 500));
					jumpWanted = false;
				}
			}
		}
		
		// Update is called once per frame
		protected void Update()
		{
			//// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
			//print("grounded? " + grounded);
			// If the jump button is pressed and the player is grounded then the player should jump.
			//if (Input.GetButtonDown("Jump") && grounded)
			//	jumpWanted = true;

			updateMovementInputs ();

			if (nbJump < maxNbJump && jumpInput && !jumpKeyDown)
			{
				jumpWanted = true;
				nbJump++;
				jumpKeyDown = true;
			}
			else if (!jumpInput && !jumpWanted)
			{
				jumpKeyDown = false;
			}
		}
		
		protected void Flip()
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

		protected void OnTriggerEnter2D(Collider2D collider)
		{

			if (collider.tag == "ladder")
			{
				Debug.Log("start climb");
				canClimb = true;
			}
		}

		protected void OnTriggerExit2D(Collider2D collider)
		{
			
			if (collider.tag == "ladder")
			{
				Debug.Log("stop climb");
				canClimb = false;
			}
		}

		protected void OnCollisionEnter2D(Collision2D collision)
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

		protected virtual void updateMovementInputs ()
		{
		}

		protected virtual void updateOrientation()
		{
			if ((facingRight != (horizontalInput > 0)) && (horizontalInput != 0)) {
				Flip ();
			}
		}

		protected void updateHorizontalMove ()
		{
			updateOrientation ();
			updateHorizontalSpeed (maxForwardSpeed);
		}


		protected void updateHorizontalSpeed (float maxSpeed)
		{
			if (horizontalInput * rigidBody.velocity.x < maxSpeed)
				rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);

			if (Mathf.Abs (rigidBody.velocity.x) > maxSpeed)
				rigidBody.velocity = new Vector2(maxSpeed * Mathf.Sign(rigidBody.velocity.x),
				                                 rigidBody.velocity.y);
			
			if (horizontalInput == 0)
				rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
		}

		protected void updateClimb (float maxSpeed)
		{
			if (canClimb) {
				rigidBody.gravityScale = 0;

				if (verticalInput * rigidBody.velocity.y < maxSpeed)
					rigidBody.AddForce(Vector2.up * verticalInput * moveForce);

				if (Mathf.Abs (rigidBody.velocity.y) > maxSpeed)
					rigidBody.velocity = new Vector2(rigidBody.velocity.x,
					                                 maxSpeed * Mathf.Sign(rigidBody.velocity.y));

				if (verticalInput == 0)
				    rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

			} else {
				rigidBody.gravityScale = defaultGravity;
			}
		}
	}
}
