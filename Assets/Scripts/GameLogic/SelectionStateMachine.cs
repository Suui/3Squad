using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

    public class SelectionStateMachine : MonoBehaviour
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


        public enum Selected
        {
            Nothing,
            Token,
            Skill,
            ExitEndTurn,
            ShowingInfo
        }


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
        }


        public void BreakDownClickInfo(ClickInfo clickInfo)
        {
            CheckSelection(clickInfo.Position, clickInfo.SkillName, clickInfo.ButtonId);
        }


        private void CheckSelection(Position position, string skillName, string buttonId)
        {
            Debug.Log("Selection Performed! " + 
                "Current SM State was: " + currentState +
                ". Parameter position was: " + position + 
                ". Parameter skillName was: " + skillName +
				". Parameter buttonId was: " + buttonId);

            // NOTHING
            if (currentState == Selected.Nothing)
            {

                // Selected a Button
                if (buttonId != null)
                {
                    ShowTransparentBackground(true);
                    ShowConfirmCancel(true);

                    previousId = buttonId;
                    previousState = Selected.Nothing;

                    currentState = Selected.ExitEndTurn;
                    return;
                }

                DisplaySelectionOverlay(position);

                // Selected a Token
                if (board["tokens"][position] != null)
                {
	                selectedToken = board["tokens"][position];

                    // Display Info of the character getting the right components. David.

                    ShowInfoButton(true);

                    // Selected a Character || Master
                    if (selectedToken.GetComponent<Skill>() != null)
                    {
						if (selectedToken.GetComponent<PlayerComponent>().Player != playingPlayer)
                        {
                            currentState = Selected.Token;
                            return;
                        }

						Skill[] skills = selectedToken.GetComponents<Skill>();

                        foreach (Skill sk in skills)
                        {
                            if (sk.ActionPointCost <= playingPlayer.ActionPoints)
                                sk.ShowUpSkill();
                        }
                    }

                    currentState = Selected.Token;
                    return;
                }

            }

            // CHARACTER
            if (currentState == Selected.Token)
            {

                // Selected a Button
                if (buttonId != null)
                {
                    ShowTransparentBackground(true);

                    if (buttonId != "Info")
                        ShowConfirmCancel(true);

                    GameObject[] skillIcons = GameObject.FindGameObjectsWithTag("SkillIcon");

					foreach (var go in skillIcons)
						Destroy(go);

                    previousId = buttonId;
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
                if (buttonId != null)
                {
                    // Selected the Confirm Button
                    if (buttonId == "Confirm")
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
                    if (buttonId == "Cancel")
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
                if (buttonId == null)
                    return;
                
                if (buttonId == "Confirm")
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

						// Removing the clicks on ChangeTurn and Confirm
						Actions.RemoveAt(Actions.Count - 1);
						Actions.RemoveAt(Actions.Count - 1);

                        if (OnChangingTurn != null)
                            OnChangingTurn(new TurnActions(Actions));
                    }
                }

                if (buttonId == "Cancel")
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


        private void ShowConfirmCancel(bool b)
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
                go.GetComponent<GUITexture>().enabled = b;
        }


        private void ShowExitEndTurn(bool b)
        {
            GameObject.FindGameObjectWithTag("ExitButton").GetComponent<GUITexture>().enabled = b;
            GameObject.FindGameObjectWithTag("EndTurnButton").GetComponent<GUITexture>().enabled = b;
        }


        private void ShowInfoButton(bool b)
        {
            GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = b;
        }


        private void ShowTransparentBackground(bool b)
        {
            backgroundObject.SetActive(b);

            Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

            backgroundObject.transform.position = 
                pos + new Vector3(pos.x / Mathf.Abs(pos.x), pos.y / Mathf.Abs(pos.y), pos.z / Mathf.Abs(pos.z)) * -0.4f;
            backgroundObject.transform.eulerAngles = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        }


        private void DisplaySelectionOverlay(Position position)
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


        private void SetUpBackground()
        {
            GameObject background = Instantiate(Resources.Load("Prefabs/Background")) as GameObject;
            background.name = "Transparent Background";

            backgroundObject = background;
            ShowTransparentBackground(false);
        }


        public Player PlayingPlayer
        {
            set { playingPlayer = value; }
        }


        public List<Action> Actions { get; set; }


	    public string DebuggingID
	    {
		    get
		    {
			    if (transform.parent.gameObject.name == "Player 01")
				    return "Player 01 SM";

			    if (transform.parent.gameObject.name == "Player 02")
				    return "Player 02 SM";

			    return "FAILURE";
		    }
	    }

    }

}