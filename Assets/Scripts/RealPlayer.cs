using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class RealPlayer : MonoBehaviour
	{

		[HideInInspector]
		public bool facingRight = true;			// For determining which way the player is currently facing.
		public float moveForce = 2000f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
		public float maxReculeSpeed = 3f;				// The fastest the player can travel in the x axis.

		public float maxJumpSpeed = 500f;				// The fastest the player can travel in the x axis.

		private int nbJump = 0;
		private Transform groundCheck;			// A position marking where to check if the player is grounded.
		private bool grounded = false;			// Whether or not the player is grounded.
		private bool jump = false;			// true if we want to jump

		private bool jumpCollision = false;
		private bool jumpKeyDown = false;

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
			if (!jumpCollision)
			{
				// Cache the horizontal input.
				float h = Input.GetAxisRaw("Horizontal");
				bool recule = false;

				// If the input is moving the player right and the player is facing left...
				if (h > 0 && !facingRight)
					recule = true;
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (h < 0 && facingRight)
					recule = true;

				// The Speed animator parameter is set to the absolute value of the horizontal input.
				//anim.SetFloat("Speed", Mathf.Abs(h));

				// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
				if (h * GetComponent<Rigidbody2D>().velocity.x < (recule?maxReculeSpeed:maxSpeed))
					// ... add a force to the player.
					GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);


				// If the player's horizontal velocity is greater than the maxSpeed...
				if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > (recule ? maxReculeSpeed : maxSpeed))
					// ... set the player's velocity to the maxSpeed in the x axis.
					GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x)
						* (recule ? maxReculeSpeed : maxSpeed), GetComponent<Rigidbody2D>().velocity.y);

				if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) == 0 && h ==0)
					// ... set the player's velocity to the maxSpeed in the x axis.
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

				
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
					GetComponent<Rigidbody2D>().AddForce(Vector2.up *
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

			//update facing
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 velocity = (mousePos - transform.position).normalized;
			if (velocity.x < 0)
			{
				if (facingRight)
				{
					Flip();
				}
				facingRight = false;
			}
			else
			{
				if (!facingRight)
				{
					Flip();
				}
				facingRight = true;
			}

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

		void Flip()
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

		public void OnCollisionEnter2D(Collision2D collision)
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
	}
}
