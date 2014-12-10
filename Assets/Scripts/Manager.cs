using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		private GameObject gameMaster;

		private SelectionStateMachine currentStateMachine;
		private RaySelection currentRaySel;
		private TurnManagement turnManagement;


		void Start()
		{
			gameMaster = Instantiate(Resources.Load("Prefabs/GameMaster")) as GameObject;
			gameMaster.name = "GameMaster";
			gameMaster.tag = "GameMaster";


			turnManagement = gameMaster.GetComponent<GameMaster>().TurnManagement;
		}


		public void PerformTurnChangeActions(Player player, TurnActions turnActions)
		{
			turnManagement.ChangeTurn();
			StartCoroutine(PerformPreviousTurnActions(turnActions));
		}


		IEnumerator PerformPreviousTurnActions(TurnActions turnActions)
		{
			currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
			currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();

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

	}

}