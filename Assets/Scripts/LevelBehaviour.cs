using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace decembreNoir
{
	public class LevelBehaviour : MonoBehaviour
	{

		public static Dictionary<string, LevelBehaviour> allLevels = new Dictionary<string, LevelBehaviour>();

		public string nameid;
		public List<Waypoint> waypoints;
		public List<Waypoint> doors;



		public List<GameObject> spawnPoint = new List<GameObject>();
		List<Monster> monsters = new List<Monster>();
		float nextSpawn = 0;
		public Monster monsterPrefab;

		System.Random rand;

		// Use this for initialization
		void Start()
		{
			allLevels.Add(nameid, this);
			rand = new System.Random();
		}

		// Update is called once per frame
		void Update()
		{
			if (nextSpawn <= 0)
			{
				Monster monster = Instantiate(monsterPrefab, transform.position,
					Quaternion.Euler(new Vector3(0, 0, 0))) as Monster;
				monster.currentlevel = this;
				monster.transform.position = spawnPoint[rand.Next(spawnPoint.Count)].transform.position;
				monsters.Add(monster);
				//refresh list
				monsters.RemoveAll(x => x == null);

				nextSpawn = rand.Next(monsters.Count+1)*0.6f;
			}
			else
			{
				nextSpawn -= Time.deltaTime;
			}
		}
	}
}
