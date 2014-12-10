using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		private GameObject playerOneGO;
		private GameObject playerTwoGO;

		private SelectionStateMachine currentStateMachine;
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

			turnManagement = GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement;
		}


		public void PerformTurnChangeActions(Player player, TurnActions turnActions)
		{
			if (player.name == "Player 01")
			{
				playerOneGO.SetActive(true);
				playerTwoGO.SetActive(false);
				//currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}
			else
			{
				playerOneGO.SetActive(false);
				playerTwoGO.SetActive(true);
				//currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}

			turnManagement.ChangeTurn();
			//StartCoroutine(PerformPreviousTurnActions(TurnActions));
			StartCoroutine(Wait(0.5f, turnActions));
		}


		private void UpdateRaySelAndSM(TurnActions turnActions)
		{
			currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
			currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();

			Debug.Log("The current SM is: " + currentStateMachine.DebuggingID);

			StartCoroutine(PerformPreviousTurnActions(turnActions));
		}


		IEnumerator PerformPreviousTurnActions(TurnActions turnActions)
		{
			currentRaySel.enabled = false;
			currentStateMachine.PlayingPlayer = turnManagement.EnemyPlayerThisTurn;

			foreach (var action in turnActions.Actions)
			{
				//currentStateMachine.BreakDownClickInfo(clickInfo);
				yield return new WaitForSeconds(0.6f);
			}

			currentStateMachine.SetReady();

			currentStateMachine.PlayingPlayer = turnManagement.CurrentPlayer;
			currentRaySel.enabled = true;
		}


		IEnumerator Wait(float time, TurnActions turnActions)
		{
			yield return new WaitForSeconds(time);

			UpdateRaySelAndSM(turnActions);
		}

	}

}