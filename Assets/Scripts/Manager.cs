using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		private GameObject playerOneGO;
		private GameObject playerTwoGO;


		void Start()
		{
			GameObject gameMaster = Instantiate(Resources.Load("Prefabs/GameMaster")) as GameObject;
			gameMaster.name = "GameMaster";
			gameMaster.tag = "GameMaster";
		}


		public void SetPlayerGOs()
		{
			playerOneGO = GameObject.Find("Player 01");
			playerTwoGO = GameObject.Find("Player 02");
		}


		public void ActivatePlayer(Player player)
		{
			if (player.name == "Player 01")
			{
				playerOneGO.SetActive(true);
				playerTwoGO.SetActive(false);
			}
			else
			{
				playerOneGO.SetActive(false);
				playerTwoGO.SetActive(true);
			}
		}

	}

}