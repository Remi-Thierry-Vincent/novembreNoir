using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class TeamPlayer : Character
	{
				// update fixed at 50times per sec
//		protected new void FixedUpdate()
//		{
//			base.FixedUpdate ();
//		}

		protected override void updateMovementInputs ()
		{
			
			horizontalInput = Input.GetAxisRaw("Horizontal");
			verticalInput = Input.GetAxisRaw("Vertical");
			//jumpInput = Input.GetButton ("Jump");
			jumpInput = verticalInput > 0;
		}

		protected override void updateOrientation () {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 velocity = (mousePos - transform.position).normalized;
			if (((velocity.x < 0) && facingRight) || ((velocity.x > 0) && !facingRight))
				Flip();
		}

	}
}
