using UnityEngine;
using System.Collections;
namespace decembreNoir
{

	public class BulletScript : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			//Destroy(this.gameObject, 5);
		}

		// Update is called once per frame
		void Update()
		{

		}
		public void OnCollisionEnter2D(Collision2D collision)
		{
			print("bullet collison with " + collision.rigidbody);
			Rigidbody2D rigidBody = collision.rigidbody;
			if (rigidBody != null)
			{
				Monster monster = rigidBody.GetComponent<Monster>();
				if (monster != null)
				{
					print("find monster!");
					monster.shoot(1);
				}
			}
			Destroy(this.gameObject);
		}
	}
}
