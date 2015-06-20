using UnityEngine;
using System.Collections;

namespace decembreNoir
{
	public class RootSceneLoader : MonoBehaviour
	{

		public GameObject scene1;

		// Use this for initialization
		//IEnumerator Start()
		//{
		//	AsyncOperation async  = Application.LoadLevelAdditiveAsync ("Scene1");
		//	yield return async;
		//	Debug.Log("Loading complete");
		//}

		void Start()
		{
			GameObject scene1_i1 = Instantiate(scene1, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			//GameObject scene1_i2 = Instantiate(scene1, new Vector3(30, 0, 0), Quaternion.identity) as GameObject;

		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}