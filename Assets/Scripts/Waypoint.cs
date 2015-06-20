using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using UnityEditor;
using UnityEngine;

namespace decembreNoir
{
	public class Waypoint : MonoBehaviour
	{
		public bool hard = false;
		public List<Waypoint> linked;
		public Dictionary<Waypoint, bool> ceiling;


		// to be more visible in unity editor
		void OnDrawGizmosSelected()
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.magenta;
			foreach (Waypoint w in linked)
			{
				Gizmos.DrawLine(transform.position, w.transform.position);
				Gizmos.DrawSphere(w.transform.position + (transform.position - w.transform.position).normalized*0.7f, 0.3f);
			}

		}
	}
}
