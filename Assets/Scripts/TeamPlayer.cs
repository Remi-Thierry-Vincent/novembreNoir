using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class TeamPlayer : Character
	{

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
