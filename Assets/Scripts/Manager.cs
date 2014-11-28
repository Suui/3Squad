using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		private GameObject playerOneGO;
		private GameObject playerTwoGO;

		private SelectionSM currentSM;
		private RaySelection currentRaySel;
		private TurnManagement turnManagement;


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

			foreach (var masterGO in GameObject.FindGameObjectsWithTag("GameMaster"))
			{
				var master = masterGO.GetComponent<GameMaster>();

				GameObject go = Instantiate(Resources.Load("Prefabs/Fox")) as GameObject;
				go.name = "Fox";
				go.transform.position = new Position(0, 0);
				go.GetComponent<PlayerComponent>().Player = master.GetPlayerOne;

				master.CurrentBoard["tokens"][new Position(0, 0)] = go;


				GameObject go2 = Instantiate(Resources.Load("Prefabs/Fox")) as GameObject;
				go2.name = "Fox2";
				go2.transform.position = new Position(5, 13);
				go2.GetComponent<PlayerComponent>().Player = master.GetPlayerTwo;

				master.CurrentBoard["tokens"][new Position(5, 13)] = go2;
			}

			turnManagement = GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement;
		}


		public void PerformTurnChangeActions(Player player, TurnEvents turnEvents)
		{
			if (player.name == "Player 01")
			{
				playerOneGO.SetActive(true);
				playerTwoGO.SetActive(false);
				//currentSM = GameObject.Find("GameMaster").GetComponent<SelectionSM>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}
			else
			{
				playerOneGO.SetActive(false);
				playerTwoGO.SetActive(true);
				//currentSM = GameObject.Find("GameMaster").GetComponent<SelectionSM>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}

			turnManagement.ChangeTurn();
			//StartCoroutine(PerformPreviousTurnActions(turnEvents));
			StartCoroutine(Wait(0.5f, turnEvents));
		}


		private void UpdateRaySelAndSM(TurnEvents turnEvents)
		{
			currentSM = GameObject.Find("GameMaster").GetComponent<SelectionSM>();
			currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();

			Debug.Log("The current SM is: " + currentSM.DebuggingID);

			StartCoroutine(PerformPreviousTurnActions(turnEvents));
		}


		IEnumerator PerformPreviousTurnActions(TurnEvents turnEvents)
		{
			currentRaySel.enabled = false;
			currentSM.PlayingPlayer = turnManagement.EnemyPlayerThisTurn;

			foreach (var clickInfo in turnEvents.ClickEvents)
			{
				currentSM.BreakDownClickInfo(clickInfo);
				yield return new WaitForSeconds(2.0f);
			}

			currentSM.SetReady();

			currentSM.PlayingPlayer = turnManagement.CurrentPlayer;
			currentRaySel.enabled = true;
		}


		IEnumerator Wait(float time, TurnEvents turnEvents)
		{
			yield return new WaitForSeconds(time);

			UpdateRaySelAndSM(turnEvents);
		}

	}

}