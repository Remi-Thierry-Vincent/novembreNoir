using UnityEngine;
using System.Collections;

namespace decembreNoir
{
	public class Monster : MonoBehaviour
	{


		private float maxX = -19999999;
		private float maxXNbSaut = 0;
		public int directionMult = -1;

		public int life = 4;

		public LevelBehaviour currentlevel = null;

		private Pathfing ia;
		private int tempo = 10;
		private float lastYvelocity = 0;
		private bool gotoDoor = true;

		// Use this for initialization
		void Start()
		{
			//??? shouldn't be null
			//if (currentlevel == null)
			//{
			//	currentlevel = LevelBehaviour.allLevels["mainlevel"];
			//}
			if (currentlevel.doors.Count > 0)
			{
				ia = new Pathfing();
				ia.objectif = currentlevel.doors[new System.Random().Next(currentlevel.doors.Count)];
				ia.parent = this;
				ia.finish = arriveToTarget;
				ia.OnStart();
			}
		}

		public void arriveToTarget()
		{
			print("ARRIVE A LA FIN");
			gotoDoor = false;
		}

		// Update is called once per frame
		void Update()
		{

		}

		void FixedUpdate()
		{
			if (gotoDoor)
			{
				tempo--;

				if (lastYvelocity > 10 && GetComponent<Rigidbody2D>().velocity.y <= 0.01)
				{
					print("HITCEILING");
					ia.hitCeiling();
				}
				lastYvelocity = GetComponent<Rigidbody2D>().velocity.y;

				if (tempo < 0 && GetComponent<Rigidbody2D>().velocity.magnitude <= 0 && GetComponent<Rigidbody2D>().IsTouchingLayers())
				{
					tempo = 10;
					ia.nextJump();

					//System.Random rand = new System.Random();
					//float currentPos = GetComponent<Rigidbody2D>().position.x;
					//if (currentPos * directionMult > maxX * directionMult)
					//{
					//	maxX = currentPos;
					//	maxXNbSaut = 0;
					//}
					//else
					//{
					//	maxXNbSaut++;
					//}
					//if (maxXNbSaut > 1 + rand.NextDouble() * 4)
					//{
					//	directionMult = -directionMult;
					//}
					//GetComponent<Rigidbody2D>().AddForce(
					//	new Vector2(directionMult * (float)(100 + rand.NextDouble() * 50),
					//		(float)(200 + rand.NextDouble() * 1000)));

				}
				//else if (GetComponent<Rigidbody2D>().velocity.y <= 0)
				//{
				//	GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				//}
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