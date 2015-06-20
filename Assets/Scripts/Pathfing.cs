using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace decembreNoir
{
	class Pathfing
	{
		public Monster parent;
		public Waypoint from = null;
		public Waypoint to = null;
		public Waypoint objectif = null;
		public Action finish;

		List<Waypoint> notTo = new List<Waypoint>();

		private Vector3 lastPos;
		private int directionMult = 1;
		private System.Random rand = new System.Random();
		private int nbJump = 0;
		private int nbCeilinghit = 0;
		private int maxJump = 7;

		//to detect fall
		private float minY = 1230;

		public void OnStart () {
			
			minY = parent.transform.position.y - 1;

			//trouver le waypont le plus proche ( pas trop loin) sinon se promener.
			//aller au waypoint exactemnt  sinon discard et se promener jusqu'a un autre

			//on est à un endroit connu! joie!

			//choper la liste des waypoints suivant pour aller où on désire

			//s'y diriger


		}

		public void hitCeiling()
		{
			nbJump += 4;
			nbCeilinghit++; ;
		}

		public bool chooseNextWaypoint()
		{
			nbJump = 0;
			nbCeilinghit = 0;
			//Monster.print("chooseNextWaypoint " + to);
			if (to == null)
			{
				//choose the first one 
				List<Waypoint> allWaypoints = new List<Waypoint>();
				allWaypoints.AddRange(parent.currentlevel.waypoints);
				allWaypoints.RemoveAll(w => notTo.Contains(w));
				if (allWaypoints.Count == 0)
				{
					Monster.print("Error: no waypoint available for " + parent);
					//TODO: promenade
					to = parent.currentlevel.waypoints[0];
					notTo.Clear();
					return false;
				}
				to = allWaypoints.OrderBy(w => Vector3.Distance(parent.transform.position, w.transform.position)).First();
				//Monster.print("chooseNextWaypoint nearest " + to);
			}
			else if (to == objectif)
			{
				notTo.Clear();
				finish();
				return true;
			}
			else
			{
				notTo.Clear();
				//go to next
				if (to == null)
				{
					Monster.print("Error: to is null " + parent);
					return false;
				}
				from = to;
				//TODO: trouver ce qui mène à l'objectif
				to = plusCourtChemin(to, objectif);
				Monster.print("PLUSCOURTCHEMIN: from "
					+ (from == null ? "null" : from.name) + " =>" + to.name + " =>=> " + objectif.name);
				if (to == null)
				{
					Monster.print("Error: to.to is null " + parent);
					return false;
				}
				directionMult = to.transform.position.x < parent.transform.position.x ? -1 : 1;
			}
			minY = Math.Min(parent.transform.position.y, to.transform.position.y) - 2;
			return false;
		}

		public void nextJump()
		{
			Monster.print(parent.transform.position.y + " <? " + minY);
			if (nbJump > maxJump || parent.transform.position.y < minY)
			{
				Monster.print("RESET : "+(nbJump>maxJump)+" or "+(parent.transform.position.y < minY)
					+ " " );
				notTo.Add(to);
				to = null;
			}
			//Monster.print("[P]nextJump " + parent + " " +(to == null?"null": Vector3.Distance(parent.transform.position, to.transform.position).ToString()));
			//Arrivé?
			if (to == null || Vector3.Distance(parent.transform.position, to.transform.position) < 2)
			{
				if (chooseNextWaypoint())
				{
					return;
				}
			}

			//find x direction
			int directionMultNow = to.transform.position.x < parent.transform.position.x ? -1 : 1;
			Waypoint jumpTo = to;
			// no way
			//if (directionMultNow != directionMult && from != null)
			//{
			//	Monster.print("return to previous =  " + jumpTo);
			//	maxJump++;
			//	//return to previous waypoint
			//	jumpTo = from;
			//}
			float xDist = Math.Abs(jumpTo.transform.position.x - parent.transform.position.x);
			float yDist = Math.Abs(jumpTo.transform.position.y - parent.transform.position.y);
			bool up = jumpTo.transform.position.y > parent.transform.position.y;

			//double forcex = 150 + rand.NextDouble() * 200;
			//double forcey = 100 + (up?100 * yDist:100) + rand.NextDouble() * ((isCeilinghit && yDist < 4) ? 400 : 600);
			//if (xDist < 3)
			//{
			//	Monster.print("[P]nextJump precise");
			//	//TODO : + si haut, - si profond
			//	forcex = 100 + rand.NextDouble() * 60;
			//	forcey = 200 + rand.NextDouble() * 150;
			//	if (yDist > 2)
			//	{
			//		forcex = up ? 100 + yDist * 40 : 50;
			//		forcey = up ? 200 + yDist * 200 : 500;
			//	}
			//}
			double forcex = 50*Math.Min(xDist,Math.Max(yDist,2)) + rand.NextDouble() * 100;
			double forcey = 100 + (up ? 200 * Math.Max(yDist, 0.5) : 100);
			//Monster.print("forcey =  " + forcey + " (" + yDist+")");
			forcey += rand.NextDouble() * Math.Max(300, (800 - yDist * 80));
			if (nbCeilinghit>0)
			{
				forcey = forcey / (nbCeilinghit+1);
			}
			if (jumpTo.hard && xDist > yDist * 2)
			{
				forcey *= 0.9;
				forcex *= 2.3;
			}
			if (jumpTo.hard && yDist > xDist * 2)
			{
				forcex *= 1.2;
				forcey *= 1.2;
			}
			if (Vector3.Distance(parent.transform.position, jumpTo.transform.position) < 4)
			{
				Monster.print("Near");
				forcex *= 0.4;
				forcey *= 0.8;
			}

			//Monster.print("[P]nextJump " + to.name + " :: " + xDist + "," + yDist + "," + up + " : " + forcex + "," + forcey + "," + directionMultNow);

			//add to detector of useless jumps
			nbJump++;

			parent.GetComponent<Rigidbody2D>().AddForce(
				new Vector2(directionMultNow * (float)(forcex),
					(float)(forcey)));

			Monster.print("Jump to " + jumpTo + " (" + Vector3.Distance(parent.transform.position, jumpTo.transform.position)
				+ "): " + new Vector2(directionMultNow * (float)(forcex), (float)(forcey)) + ", " + nbCeilinghit + ", " + up);
		}

		public static Waypoint plusCourtChemin(Waypoint current, Waypoint objectif)
		{
			HashSet<Waypoint> seen = new HashSet<Waypoint>();
			seen.Add(current);
			Waypoint best = null;
			float bestDist = 100000;
			foreach (Waypoint link in current.linked)
			{
				if (!seen.Contains(link))
				{
					if (link == objectif)
					{
						return link;
					}
					else
					{
						float dist = plusCourtCheminElem(seen, link, objectif);
						if (dist > 0)
						{
							//Monster.print("find a way: " + link.name + ":" + dist
							//	+ ",  best:" + (best == null ? "null" : best.name) + ":" + bestDist);
							if (dist < bestDist)
							{
								bestDist = dist;
								best = link;
							}
						}
					}
				}
			}
			return best;
		}

		public static float plusCourtCheminElem(HashSet<Waypoint> seen, Waypoint current, Waypoint objectif)
		{
			seen.Add(current);
			foreach (Waypoint link in current.linked)
			{
				if (!seen.Contains(link))
				{
					if (link == objectif)
					{
						return Vector3.Distance(current.transform.position, link.transform.position);
					}
					else
					{
						float dist = plusCourtCheminElem(seen, link, objectif);
						if (dist > 0)
						{
							return Vector3.Distance(current.transform.position, link.transform.position) + dist;
						}
					}
				}
			}
			seen.Remove(current);
			return -1;
		}

	}
}
