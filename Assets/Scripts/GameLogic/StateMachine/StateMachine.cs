using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

	public class StateMachine : MonoBehaviour
	{

		public delegate void ChangingTurn(TurnActions turnActions);
		public static event ChangingTurn OnChangingTurn;


		private Board board;
		private GameObject backgroundObject;

		private Player playingPlayer;
		private GameObject selectedToken;
		private Skill selectedSkill;
		private Position previousSelectedPos;
		private Selected currentState;
		private Selected previousState;
		private string previousId;

		private NothingSelected nothingSelected;

		public State curState;
		public State prevState;



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
			board = GetComponent<GameMaster>().CurrentBoard;

			SetUpBackground();

			Actions = new List<Action>();

			playingPlayer = GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement.CurrentPlayer;
			selectedSkill = null;
			previousSelectedPos = new Position(0, 0);
			currentState = Selected.Nothing;
			previousState = Selected.Nothing;

			nothingSelected = new NothingSelected(this);
			curState = new NothingSelectedState(this);
			prevState = curState;
		}


		public void BreakDownClickInfo(ClickInfo clickInfo)
		{
			CheckSelection(clickInfo.Position, clickInfo.SkillName, clickInfo.ButtonID);
		}


		private void CheckSelection(Position position, string skillName, string buttonID)
		{
			Debug.Log("Selection Performed! " + 
				"Current SM State was: " + curState.GetType() +
				". Parameter position was: " + position + 
				". Parameter skillName was: " + skillName +
				". Parameter buttonID was: " + buttonID);


			prevState = curState;

			if (buttonID != null)
			{
				curState = curState.ClickButton(buttonID);
				return;
			}

			if (curState.GetType() == typeof(NothingSelectedState) || curState.GetType() == typeof(TokenSelectedState))
				DisplaySelectionOverlay(position);

			if (skillName != null)
			{
				curState = curState.ClickSkill(skillName);
				return;
			}

			if (board["tokens"][position] != null)
			{
				curState = curState.ClickPosition(position);
				return;
			}

			if (curState.GetType() == typeof (TokenSelectedState))
			{
				// Selected the DisplayInfoAboutToken Button


				// Selected Nothing
				// Delete Info of the token getting the right components. David.
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
					Destroy(go);

				ShowInfoButton(false);

				curState = new NothingSelectedState(this);
				return;
			}

			// NOTHING
			if (currentState == Selected.Nothing)
			{
				if (nothingSelected.ButtonSelection(buttonID))
					return;

				DisplaySelectionOverlay(position);

				nothingSelected.TokenSelection(position);

				return;
			}

			// CHARACTER
			if (currentState == Selected.Token)
			{

				// Selected a Button
				if (buttonID != null)
				{
					ShowTransparentBackground(true);

					if (buttonID != "Info")
						ShowConfirmCancel(true);

					GameObject[] skillIcons = GameObject.FindGameObjectsWithTag("SkillIcon");

					foreach (var go in skillIcons)
						Destroy(go);

					previousId = buttonID;
					previousState = Selected.Token;

					currentState = Selected.ExitEndTurn;
					return;
				}

				DisplaySelectionOverlay(position);

				// Selected a skillName
				if (skillName != null)
				{
					selectedSkill = selectedToken.GetComponent(skillName) as Skill;
					selectedSkill.Setup();

					foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
						Destroy(go);

					ShowConfirmCancel(true);
					ShowExitEndTurn(false);
					ShowInfoButton(false);

					currentState = Selected.Skill;
					return;
				}

				// Selected Token
				if (board["tokens"][position] != null)
				{
					selectedToken = board["tokens"][position];

					// Delete Info of the token getting the right components. David.
					// Display Info of the token getting the right components. David.

					foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
						Destroy(go);

					// Selected another Character || Master
					if (selectedToken.GetComponent<Skill>() != null)
					{

						if (selectedToken.GetComponent<PlayerComponent>().Player != playingPlayer)
							return;

						Skill[] skills = selectedToken.GetComponents<Skill>();

						foreach (Skill sk in skills)
						{
							if (sk.ActionPointCost <= playingPlayer.ActionPoints)
								sk.ShowUpSkill();
						}
					}

					return;
				}

				// Selected the DisplayInfoAboutToken Button


				// Selected Nothing
				// Delete Info of the token getting the right components. David.
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
					Destroy(go);

				ShowInfoButton(false);

				currentState = Selected.Nothing;
				return;
			}

			// SKILL
			if (currentState == Selected.Skill)
			{
				if (buttonID != null)
				{
					// Selected the Confirm Button
					if (buttonID == "Confirm")
					{
						Actions.Add(new Action
							(
								(Position) selectedToken.transform.position, 
								selectedSkill.GetSkillType(), 
								selectedSkill.Confirm()
							));

						selectedSkill.Clear();

						// Manage Action Points
						playingPlayer.ActionPoints -= selectedSkill.ActionPointCost;

						ShowConfirmCancel(false);
						ShowExitEndTurn(true);
						ShowInfoButton(true);

						// Set the selection to the character again
						Position pos = (Position) selectedToken.transform.position;
						DisplaySelectionOverlay(pos);

						Skill[] skills = selectedToken.GetComponents<Skill>();

						foreach (Skill sk in skills)
						{
							if (sk.ActionPointCost <= playingPlayer.ActionPoints)
								sk.ShowUpSkill();
						}

						currentState = Selected.Token;
						return;
					}

					// Selected the Cancel Button
					if (buttonID == "Cancel")
					{
						selectedSkill.Clear();

						ShowConfirmCancel(false);
						ShowExitEndTurn(true);
						ShowInfoButton(true);

						// Set the selection to the character again
						Position pos = (Position)selectedToken.transform.position;
						DisplaySelectionOverlay(pos);
						
						Skill[] skills = selectedToken.GetComponents<Skill>();

						foreach (Skill sk in skills)
						{
							if (sk.ActionPointCost <= playingPlayer.ActionPoints)
								sk.ShowUpSkill();
						}

						currentState = Selected.Token;
						return;
					}
				}

				selectedSkill.Click(position);
				return;
			}

			// EXITENDTURN
			if (currentState == Selected.ExitEndTurn)
			{
				if (buttonID == null)
					return;
				
				if (buttonID == "Confirm")
				{
					if (previousId == "Exit")
					{
						// TODO: Save game state and exit (need more stuff here)
						currentState = previousState;

						Application.Quit();
						return;
					}

					if (previousId == "EndTurn")
					{
						ShowInfoButton(false);
						ShowConfirmCancel(false);
						ShowTransparentBackground(false);
						ShowExitEndTurn(false);
						DisplaySelectionOverlay(null);

						if (OnChangingTurn != null)
							OnChangingTurn(new TurnActions(Actions));
					}
				}

				if (buttonID == "Cancel")
				{
					ShowConfirmCancel(false);
					ShowTransparentBackground(false);

					if (previousState == Selected.Token && selectedToken != null && selectedToken.GetComponent<Skill>())
					{
						Skill[] skills = selectedToken.GetComponents<Skill>();

						foreach (Skill sk in skills)
						{
							if (sk.ActionPointCost <= playingPlayer.ActionPoints)
								sk.ShowUpSkill();
						}
					}

					currentState = previousState;
					return;
				}
			}

		}


		private void RestoreSM()
		{
			currentState = Selected.Nothing;
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
			backgroundObject.SetActive(b);

			Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

			backgroundObject.transform.position = 
				pos + new Vector3(pos.x / Mathf.Abs(pos.x), pos.y / Mathf.Abs(pos.y), pos.z / Mathf.Abs(pos.z)) * -0.4f;
			backgroundObject.transform.eulerAngles = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
		}


		public void DisplaySelectionOverlay(Position position)
		{
			if (position == null)
			{
				board["overlays"][previousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
				return;
			}

			board["overlays"][previousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
			board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(1);

			previousSelectedPos = position;
		}


		public void SetUpBackground()
		{
			GameObject background = Instantiate(Resources.Load("Prefabs/Background")) as GameObject;
			background.name = "Transparent Background";

			backgroundObject = background;
			ShowTransparentBackground(false);
		}


		public Player PlayingPlayer
		{
			get { return playingPlayer; }
			set { playingPlayer = value; }
		}


		public List<Action> Actions { get; set; }


		public Board Board
		{
			get { return board; }
			set { board = value; }
		}

		public GameObject BackgroundObject
		{
			get { return backgroundObject; }
			set { backgroundObject = value; }
		}

		public GameObject SelectedToken
		{
			get { return selectedToken; }
			set { selectedToken = value; }
		}

		public Skill SelectedSkill
		{
			get { return selectedSkill; }
			set { selectedSkill = value; }
		}

		public Position PreviousSelectedPos
		{
			get { return previousSelectedPos; }
			set { previousSelectedPos = value; }
		}

		public Selected CurrentState
		{
			get { return currentState; }
			set { currentState = value; }
		}

		public Selected PreviousState
		{
			get { return previousState; }
			set { previousState = value; }
		}

		public string PreviousId
		{
			get { return previousId; }
			set { previousId = value; }
		}
	}


	public enum Selected
	{
		Nothing,
		Token,
		Skill,
		ExitEndTurn,
		ShowingInfo
	}

}