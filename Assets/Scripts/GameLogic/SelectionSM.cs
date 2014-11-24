using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

    public class SelectionSM : MonoBehaviour
    {

        public delegate void ChangingTurn(TurnEvents turnEvents);
        public static event ChangingTurn OnChangingTurn;


        private Board board;

        private Player playingPlayer;
        private GameObject selectedToken;
        private Skill selectedSkill;
        private Position previousSelectedPos;
        private Selected currentState;


        public enum Selected
        {
            Nothing,
            Token,
            Skill,
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

            ClickEvents = new List<ClickInfo>();

            playingPlayer = null;
            selectedSkill = null;
            previousSelectedPos = new Position(0, 0);
            currentState = Selected.Nothing;
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
                    CheckButtons(buttonId);
                    return;
                }

                DisplaySelectionOverlay(position);

                // Selected a Token
                if (board["tokens"][position] != null)
                {
                    // Display Info of the character getting the right components. David.

	                ShowInfoButton();

                    // Selected a Character || Master
                    if (board["tokens"][position].GetComponent<Skill>() != null)
                    {
                        Skill[] skills = board["tokens"][position].GetComponents<Skill>();

                        foreach (Skill sk in skills)
                            sk.ShowUpSkill();
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
                    CheckButtons(buttonId);
                    return;
                }

                DisplaySelectionOverlay(position);

                // Selected a Skill
                if (skill != null)
                {
                    skill.Setup();

                    ShowConfirmCancel();
                    HideExitEndTurn();

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
                            sk.ShowUpSkill();
                    }

                    return;
                }

                // Selected the DisplayInfoAboutToken Button


                // Selected Nothing
                // Delete Info of the token getting the right components. David.
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                    Destroy(go);

	            HideInfoButton();

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

                        HideConfirmCancel();
                        ShowExitEndTurn();

                        currentState = Selected.Token;
                        return;
                    }

                    // Selected the Cancel Button
                    if (buttonId == "Cancel")
                    {

                    }
                }

                // Selected an available position
                return;
            }

        }


        private void CheckButtons(string buttonId)
        {
            // Selected the Exit Button
            if (buttonId == "Exit")
            {
                // I guess we need to save the state of the game before this
                Application.Quit();
            }

            // Selected the EndTurn Button
            if (buttonId == "EndTurn")
            {
                if (OnChangingTurn != null)
                {
                    OnChangingTurn(new TurnEvents(ClickEvents));
                    ClickEvents.Clear();

                    currentState = Selected.Nothing;
                }
            }
        }


        private void ShowConfirmCancel()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
                go.GetComponent<GUITexture>().enabled = true;
        }


        private void HideConfirmCancel()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
                go.GetComponent<GUITexture>().enabled = false;
        }


        private void ShowExitEndTurn()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ExitEndTurn"))
                go.GetComponent<GUITexture>().enabled = true;
        }


        private void HideExitEndTurn()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("ExitEndTurn"))
                go.GetComponent<GUITexture>().enabled = false;
        }


	    private void ShowInfoButton()
	    {
			GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = true;
	    }


		private void HideInfoButton()
		{
			GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = false;
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