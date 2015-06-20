using UnityEngine;
using System.Collections;
using System;
namespace decembreNoir
{

	public class BasciGun : MonoBehaviour
	{

		public Rigidbody2D bulletPrefab;
		private float shootCooldown = 0;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

			if (shootCooldown > 0)
			{
				shootCooldown -= Time.deltaTime;
			}
			if (shootCooldown <= 0 && Input.GetButton("Fire1"))//Input.GetKeyDown(KeyCode.LeftControl))
			{
				print("FIRE " + bulletPrefab);
				shootCooldown += 0.2f;
				//Transform shotTransform = Instantiate(shotPrefab) as Transform;
				float angleDegree = 0;
				Rigidbody2D bulletInstance = Instantiate(bulletPrefab, transform.position,
					Quaternion.Euler(new Vector3(0, 0, angleDegree))) as Rigidbody2D;
				//Physics.IgnoreCollision(bulletInstance.GetComponent<Collider>(), GetComponent<Collider>());
				Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 velocity = (mousePos - transform.position);//.normalized;
				print("transform.position.x=" + transform.position.x);
				print("transform.position.x=" + (transform.parent.localScale));
				if (Math.Sign(transform.parent.localScale.x) != Math.Sign(velocity.x))
				{
					velocity.x = -velocity.x;
				}
				velocity = (velocity.normalized * 40);
				bulletInstance.velocity = velocity;
				//print(mousePos + " - " + transform.position + " = " + (mousePos - transform.position));
				print("vel = " + bulletInstance.velocity.magnitude
					+ " ... " + bulletInstance.velocity.normalized.magnitude
					+ " => " + (bulletInstance.velocity.normalized * 10).magnitude);
				bulletInstance.velocity = (bulletInstance.velocity.normalized * 40);
			}


		}
	}
}