﻿using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class Manager : MonoBehaviour
	{

		private GameObject playerOneGO;
		private GameObject playerTwoGO;

		private SelectionStateMachine _currentStateMachine;
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
				SelectedCharacters characters = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();
				int pos = 5;

				foreach (var name in characters.selectedCharacters)
				{
					GameObject go = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
					go.name = name;
					go.transform.position = new Position(pos, 0);
					go.GetComponent<PlayerComponent>().Player = master.GetPlayerOne;

					master.CurrentBoard["tokens"][new Position(pos, 0)] = go;

					GameObject go2 = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
					go2.name = name;
					go2.transform.position = new Position(pos, 13);
					go2.GetComponent<PlayerComponent>().Player = master.GetPlayerTwo;

					master.CurrentBoard["tokens"][new Position(pos, 13)] = go2;

					pos -= 2;
				}
			}

			turnManagement = GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement;
		}


		public void PerformTurnChangeActions(Player player, TurnActions turnActions)
		{
			if (player.name == "Player 01")
			{
				playerOneGO.SetActive(true);
				playerTwoGO.SetActive(false);
				//_currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}
			else
			{
				playerOneGO.SetActive(false);
				playerTwoGO.SetActive(true);
				//_currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
				//currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();
			}

			turnManagement.ChangeTurn();
			//StartCoroutine(PerformPreviousTurnActions(TurnActions));
			StartCoroutine(Wait(0.5f, turnActions));
		}


		private void UpdateRaySelAndSM(TurnActions turnActions)
		{
			_currentStateMachine = GameObject.Find("GameMaster").GetComponent<SelectionStateMachine>();
			currentRaySel = GameObject.Find("GameMaster").GetComponent<RaySelection>();

			Debug.Log("The current SM is: " + _currentStateMachine.DebuggingID);

			StartCoroutine(PerformPreviousTurnActions(turnActions));
		}


		IEnumerator PerformPreviousTurnActions(TurnActions turnActions)
		{
			currentRaySel.enabled = false;
			_currentStateMachine.PlayingPlayer = turnManagement.EnemyPlayerThisTurn;

			foreach (var clickInfo in turnActions.ClickEvents)
			{
				_currentStateMachine.BreakDownClickInfo(clickInfo);
				yield return new WaitForSeconds(0.6f);
			}

			_currentStateMachine.SetReady();

			_currentStateMachine.PlayingPlayer = turnManagement.CurrentPlayer;
			currentRaySel.enabled = true;
		}


		IEnumerator Wait(float time, TurnActions turnActions)
		{
			yield return new WaitForSeconds(time);

			UpdateRaySelAndSM(turnActions);
		}

	}

}