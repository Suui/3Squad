using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

	public class StateMachine : MonoBehaviour
	{

		void OnEnable()
		{
			GameMaster.OnChangingTurn +=		RestoreSM;

			RaySelection.OnSelection +=         BreakDownClickInfo;
			ClickableSkill.OnSkillClick +=      BreakDownClickInfo;
			ClickableButton.OnButtonClick +=    BreakDownClickInfo;
		}


		void OnDisable()
		{
			GameMaster.OnChangingTurn -=			RestoreSM;

			RaySelection.OnSelection -=         BreakDownClickInfo;
			ClickableSkill.OnSkillClick -=      BreakDownClickInfo;
			ClickableButton.OnButtonClick -=    BreakDownClickInfo;
		}


		void Start()
		{
			PlayingPlayer = GameObject.Find("GameMaster").GetComponent<GameMaster>().PlayerOne;
			Board = GetComponent<GameMaster>().CurrentBoard;

			SetUpBackground();

			Actions = new List<Action>();

			SelectedSkill = null;
			PreviousSelectedPos = new Position(0, 0);

			CurrentState = new NothingSelectedState(this);
			PreviousState = CurrentState;
		}


		public void BreakDownClickInfo(ClickInfo clickInfo)
		{
			CheckSelection(clickInfo.Position, clickInfo.SkillName, clickInfo.ButtonID);
		}


		private void CheckSelection(Position position, string skillName, string buttonID)
		{
			Debug.Log("Selection Performed! " + 
				"Current SM State was: " + CurrentState.GetType() +
				". Parameter position was: " + position + 
				". Parameter skillName was: " + skillName +
				". Parameter buttonID was: " + buttonID);


			if (buttonID != null)
			{
				CurrentState = CurrentState.ClickButton(buttonID);
				return;
			}

			// If we are on those States, we should always display the selection overlay now
			if (CurrentState.GetType() == typeof(NothingSelectedState) || CurrentState.GetType() == typeof(TokenSelectedState))
				DisplaySelectionOverlay(position);

			if (skillName != null)
			{
				CurrentState = CurrentState.ClickSkill(skillName);
				return;
			}

			// We should not check the position if we are using a Skill,
			// as the Skill is responsible for how to process the selected position
			if (CurrentState.GetType() == typeof(SkillSelectedState))
			{
				SelectedSkill.Click(position);
				return;
			}
			
			if (Board["tokens"][position] != null)
			{
				CurrentState = CurrentState.ClickPosition(position);
				return;
			}

			// If we have a Token selected, and select a NULL position, 
			// we must go back to the NothingSelectedState
			if (CurrentState.GetType() == typeof (TokenSelectedState))
			{
				// Selected the DisplayInfoAboutToken Buttond

				// Selected Nothing
				// Delete Info of the token getting the right components. David.
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
					Destroy(go);

				ShowInfoButton(false);

				CurrentState = new NothingSelectedState(this);
			}
		}


		private void RestoreSM()
		{
			CurrentState = new NothingSelectedState(this);
		}


		public void SetReady()
		{
			if (Actions != null)
				Actions.Clear();

			foreach (var go in GameObject.FindGameObjectsWithTag("SkillIcon"))
				Destroy(go);

			ShowExitEndTurn(true);
		}


		public void ShowConfirmCancel(bool b)
		{
			foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
				go.GetComponent<GUITexture>().enabled = b;
		}


		public void ShowExitEndTurn(bool b)
		{
			GameObject.FindGameObjectWithTag("ExitButton").GetComponent<GUITexture>().enabled = b;
			GameObject.FindGameObjectWithTag("EndTurnButton").GetComponent<GUITexture>().enabled = b;
		}


		public void ShowInfoButton(bool b)
		{
			GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = b;
		}


		public void ShowTransparentBackground(bool b)
		{
			BackgroundObject.SetActive(b);

			Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

			BackgroundObject.transform.position = 
				pos + new Vector3(pos.x / Mathf.Abs(pos.x), pos.y / Mathf.Abs(pos.y), pos.z / Mathf.Abs(pos.z)) * -0.4f;
			BackgroundObject.transform.eulerAngles = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
		}


		public void DisplaySelectionOverlay(Position position)
		{
			if (position == null)
			{
				Board["overlays"][PreviousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
				return;
			}

			Board["overlays"][PreviousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
			Board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(1);

			PreviousSelectedPos = position;
		}


		public void SetUpBackground()
		{
			GameObject background = Instantiate(Resources.Load("Prefabs/Background")) as GameObject;
			background.name = "Transparent Background";

			BackgroundObject = background;
			ShowTransparentBackground(false);
		}


		public List<Action> Actions { get; set; }

		public State CurrentState { get; set; }

		public State PreviousState { get; set; }
		
		public Player PlayingPlayer { get; set; }

		public Board Board { get; set; }

		public GameObject BackgroundObject { get; set; }

		public GameObject SelectedToken { get; set; }

		public Skill SelectedSkill { get; set; }

		public Position PreviousSelectedPos { get; set; }

		public string PreviousId { get; set; }

	}

}