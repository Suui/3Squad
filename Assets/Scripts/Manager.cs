using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		void Start()
		{
			GameObject gameMaster = Instantiate(Resources.Load("Prefabs/GameMaster")) as GameObject;
			gameMaster.name = "GameMaster";
			gameMaster.tag = "GameMaster";
		}


		public void GroupNodes(string player)
		{
			GameObject currentPlayer = new GameObject(player);

			GameObject.Find("GameMaster").transform.parent = currentPlayer.transform;
			GameObject.Find("BoardNode").transform.parent = currentPlayer.transform;
		}

	}

}