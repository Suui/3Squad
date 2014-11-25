using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

    public class SelectionSM : MonoBehaviour
    {

        public delegate void ChangingTurn(TurnEvents turnEvents);
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
            RaySelection.OnSelection +=         BreakDownClickInfo;
            ClickableSkill.OnSkillClick +=      BreakDownClickInfo;
            ClickableButton.OnButtonClick +=    BreakDownClickInfo;
        }


        void OnDisable()
        {
            RaySelection.OnSelection -=         BreakDownClickInfo;
            ClickableSkill.OnSkillClick -=      BreakDownClickInfo;
            ClickableButton.OnButtonClick -=    BreakDownClickInfo;
        }


        void Start()
        {
            board = GetComponent<GameMaster>().CurrentBoard;

	        backgroundObject = GameObject.FindGameObjectWithTag("Background");
	        ShowTransparentBackground(false);

            ClickEvents = new List<ClickInfo>();

            playingPlayer = GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement.CurrentPlayer;
            selectedSkill = null;
            previousSelectedPos = new Position(0, 0);
            currentState = Selected.Nothing;
            previousState = Selected.Nothing;
        }


        private void BreakDownClickInfo(ClickInfo clickInfo)
        {
            // TODO: Function to process if clickInfo should be included can be set up here
            ClickEvents.Add(clickInfo);
            CheckSelection(clickInfo.Position, clickInfo.Skill, clickInfo.ButtonId);
        }


        private void CheckSelection(Position position, Skill skill, string buttonId)
        {
            Debug.Log("Selection Performed! " + 
                "Current SM State was: " + currentState +
                ". Parameter position was: " + position + 
                ". Parameter Skill was: " + skill);

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
                    // Display Info of the character getting the right components. David.

                    ShowInfoButton(true);

                    // Selected a Character || Master
                    if (board["tokens"][position].GetComponent<Skill>() != null)
                    {
                        Skill[] skills = board["tokens"][position].GetComponents<Skill>();

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
                    ShowConfirmCancel(true);

                    GameObject[] skillIcons = GameObject.FindGameObjectsWithTag("SkillIcon");

	                if (skillIcons.Length > 0)
	                {
		                selectedToken = skillIcons[0].gameObject.transform.parent.gameObject;

		                foreach (var go in skillIcons)
			                Destroy(go);
	                }

                    previousId = buttonId;
                    previousState = Selected.Token;

                    currentState = Selected.ExitEndTurn;
                    return;
                }

                DisplaySelectionOverlay(position);

                // Selected a Skill
                if (skill != null)
                {
                    skill.Setup();

                    ShowConfirmCancel(true);
                    ShowExitEndTurn(false);
                    ShowInfoButton(false);

                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                        Destroy(go);

                    selectedSkill = skill;
                    selectedToken = board["tokens"][previousSelectedPos].gameObject;
                    currentState = Selected.Skill;

                    return;
                }

                // Selected Token
                if (board["tokens"][position] != null)
                {
                    // Delete Info of the token getting the right components. David.
                    // Display Info of the token getting the right components. David.

                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                        Destroy(go);

                    // Selected another Token || Master
                    if (board["tokens"][position].GetComponent<Skill>() != null)
                    {
                        Skill[] skills = board["tokens"][position].GetComponents<Skill>();

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
                        selectedSkill.Confirm();
                        selectedSkill.Clear();

                        // Manage Action Points
                        playingPlayer.ActionPoints -= selectedSkill.ActionPointCost;

                        ShowConfirmCancel(false);
                        ShowExitEndTurn(true);

                        // Set the selection to the character again
                        Position pos = (Position) selectedToken.transform.position;
                        DisplaySelectionOverlay(pos);

                        Skill[] skills = board["tokens"][pos].GetComponents<Skill>();

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

                        // Set the selection to the character again
                        Position pos = (Position)selectedToken.transform.position;
                        DisplaySelectionOverlay(pos);
                        
                        Skill[] skills = board["tokens"][pos].GetComponents<Skill>();

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
                        if (OnChangingTurn != null)
                        {
                            OnChangingTurn(new TurnEvents(ClickEvents));
                            ClickEvents.Clear();

                            currentState = Selected.Nothing;
                            return;
                        }
                            
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


        private void ShowConfirmCancel(bool b)
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
                go.GetComponent<GUITexture>().enabled = b;
        }


        private void ShowExitEndTurn(bool b)
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ExitEndTurn"))
                go.GetComponent<GUITexture>().enabled = b;
        }


        private void ShowInfoButton(bool b)
        {
            GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = b;
        }


        private void ShowTransparentBackground(bool b)
        {
			backgroundObject.SetActive(b);
        }


        private void DisplaySelectionOverlay(Position position)
        {
            board["overlays"][previousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
            board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(1);

            previousSelectedPos = position;
        }


        public Player PlayingPlayer
        {
            set { playingPlayer = value; }
        }


        public List<ClickInfo> ClickEvents { get; set; }


    }

}