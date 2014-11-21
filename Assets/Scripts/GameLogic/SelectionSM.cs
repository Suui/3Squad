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
        private Skill previousSelectedSkill;
        private Position previousSelectedPos;
        private Selected currentState;


        public enum Selected
        {
            Nothing,
            Character,
            Skill,
            SkillConfirm
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
            previousSelectedSkill = null;
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

                    // Selected a Character || Master
                    if (board["tokens"][position].GetComponent<Skill>() != null)
                    {
                        Skill[] skills = board["tokens"][position].GetComponents<Skill>();

                        foreach (Skill sk in skills)
                            sk.ShowUpSkill();
                    }

                    currentState = Selected.Character;
                    return;
                }

            }

            // CHARACTER
            if (currentState == Selected.Character)
            {
                DisplaySelectionOverlay(position);

                // Selected a Skill
                if (skill != null)
                {
                    skill.Setup();
                    previousSelectedSkill = skill;
                    currentState = Selected.Skill;

                    // Display Confirm / Cancel Buttons

                    return;
                }

                // Selected a Token
                if (board["tokens"][position] != null)
                {
                    // Delete Info of the token getting the right components. David.
                    // Display Info of the token getting the right components. David.

                    // Selected another Character || Master
                    if (board["tokens"][position] != null && board["tokens"][position].GetComponent<Skill>() != null)
                    {
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                            Destroy(go);

                        Skill[] skills = board["tokens"][position].GetComponents<Skill>();

                        foreach (Skill sk in skills)
                            sk.ShowUpSkill();
                    }

                    return;
                }

                // Selected the DisplayInfoAboutToken Button


                // Selected the Exit Button


                // Selected the End Turn Button


                // Selected Nothing
                // Delete Info of the token getting the right components. David.
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                    Destroy(go);

                currentState = Selected.Nothing;
                return;
            }

            // SKILL
            if (currentState == Selected.Skill)
            {

                // Clicked on Confirm


                // Clicked on Cancel


                // Selected an available position
                return;
            }

        }


        private void CheckButtons(string buttonId)
        {
            // Selcted the Exit Button
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