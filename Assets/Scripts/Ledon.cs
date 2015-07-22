using UnityEngine;
using System.Collections;
using System;

namespace decembreNoir
{
	public class Ledon : Character
	{

		
		public int life = 4;

		
		protected static int minTempoJump = 10;
		protected static int maxTempoJump = 100;
		protected int tempoNextJump = maxTempoJump;

		protected override void updateMovementInputs ()
		{
			horizontalInput = -1;
			verticalInput = 0;

			tempoNextJump--;
			if (tempoNextJump < 0) {
				jumpInput = true;
				tempoNextJump = UnityEngine.Random.Range(minTempoJump, maxTempoJump);
			} else {
				jumpInput = false;
			}
		}
		
		
		public void shoot(int puissance)
		{
			life -= puissance;
			if (life <= 0)
			{
				Destroy(this.gameObject);
			}
		}
		
	}
}