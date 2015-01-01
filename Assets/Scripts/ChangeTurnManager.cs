using System.Collections;
using UnityEngine;


namespace Medusa
{

	public class ChangeTurnManager : MonoBehaviour
	{

		private GameObject gameMaster;

		private StateMachine stateMachine;
		private RaySelection raySelection;
		private TurnManagement turnManagement;
		private Board board;


		void Start()
		{
			gameMaster = Instantiate(Resources.Load("Prefabs/GameMaster")) as GameObject;
			gameMaster.name = "GameMaster";
			gameMaster.tag = "GameMaster";

			board = gameMaster.GetComponent<GameMaster>().CurrentBoard;
			turnManagement = gameMaster.GetComponent<GameMaster>().TurnManagement;

			stateMachine = gameMaster.GetComponent<StateMachine>();
			raySelection = gameMaster.GetComponent<RaySelection>();
		}


		public void PerformTurnChangeActions(TurnActions turnActions)
		{
			turnManagement.ChangeTurn();
			StartCoroutine(PerformPreviousTurnActions(turnActions));
		}


        public IEnumerator WaitForTurn()
        {
            Server server = GameObject.Find("Server").GetComponent<Server>();

            while (server.MyTurn != true)
            {
                server.RequestWait();
                yield return new WaitForSeconds(2.0f);
            }
        }


		IEnumerator PerformPreviousTurnActions(TurnActions turnActions)
		{
			raySelection.enabled = false;
			stateMachine.PlayingPlayer = turnManagement.EnemyPlayerThisTurn;

			foreach (var action in turnActions.ActionList)
			{
				board["overlays"][action.CharacterPos].GetComponent<Selectable>().SetOverlayMaterial(1);
				yield return new WaitForSeconds(0.7f);

				Skill skill = board["tokens"][action.CharacterPos].GetComponent(action.SkillName) as Skill;
				skill.Setup();
				yield return new WaitForSeconds(0.5f);

				foreach (var pos in action.TargetPositions)
				{
					skill.Click(pos);
					yield return new WaitForSeconds(0.5f);
				}

				skill.Confirm();
				board["overlays"][action.CharacterPos].GetComponent<Selectable>().SetOverlayMaterial(0);
				yield return new WaitForSeconds(0.7f);
			}

			stateMachine.SetReady();

			stateMachine.PlayingPlayer = turnManagement.CurrentPlayer;
			raySelection.enabled = true;
		}

	}

}