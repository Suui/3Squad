using UnityEngine;


namespace Medusa
{

	class Initialization : MonoBehaviour
	{

		void Start()
		{
			GameObject playerOne = new GameObject("Player 01");

			GameObject gameMaster = Instantiate(Resources.Load("Prefabs/GameMaster")) as GameObject;
			gameMaster.name = "GameMaster";
			gameMaster.tag = "GameMaster";
			gameMaster.transform.parent = playerOne.transform;
		}

	}

}