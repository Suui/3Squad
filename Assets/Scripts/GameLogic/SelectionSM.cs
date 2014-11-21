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
            RaySelection.OnSelection +=     BreakDownClickInfo;
            ClickableSkill.OnSkillClick +=  BreakDownClickInfo;
        }


        void OnDisable()
        {
            RaySelection.OnSelection -=     BreakDownClickInfo;
            ClickableSkill.OnSkillClick -=  BreakDownClickInfo;
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
            CheckSelection(clickInfo.Position, clickInfo.Skill);
        }


        private void CheckSelection(Position position, Skill skill)
        {
            Debug.Log("Selection Performed! " + 
                "Current SM State was: " + currentState +
                ". Parameter position was: " + position + 
                ". Parameter Skill was: " + skill);

            // NOTHING
            if (currentState == Selected.Nothing)
            {
                DisplaySelectionOverlay(position);

                // Selected a Character || Master
                if (board["tokens"][position] != null && board["tokens"][position].GetComponent<Skill>() != null)
                {
                    Skill[] skills = board["tokens"][position].GetComponents<Skill>();

                    foreach (Skill sk in skills)
                        sk.ShowUpSkill();

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
                    skill.Setup();  // TODO: Coordinar con David
                    previousSelectedSkill = skill;
                    currentState = Selected.Skill;
                    return;
                }

                // Selected another Character || Master
                if (board["tokens"][position] != null && board["tokens"][position].GetComponent<Skill>() != null)
                {
                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                        Destroy(go);

                    Skill[] skills = board["tokens"][position].GetComponents<Skill>();

                    foreach (Skill sk in skills)
                        sk.ShowUpSkill();

                    return;
                }

                // Deselected
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                    Destroy(go);

                currentState = Selected.Nothing;
                return;
            }

            // SKILL
            if (currentState == Selected.Skill)
            {
                // Selected another skill
                if (skill != null)
                {
                    if (skill != previousSelectedSkill)
                    {
                        previousSelectedSkill.Clear();
                        skill.Setup(); // TODO: Coordinar con David
                        previousSelectedSkill = skill;
                        return;
                    }

                    return;
                }

                // Selected an unavailable position == Cancel
                if (previousSelectedSkill.Click(position) == false)
                {
                    previousSelectedSkill.Clear();
                    currentState = Selected.Character;
                    return;
                }

                // Selected an available position
                currentState = Selected.SkillConfirm;
                return;
            }

            // SKILL CONFIRM
            if (currentState == Selected.SkillConfirm)
            {
                // Selected another skill
                if (skill != null && skill != previousSelectedSkill)
                {
                    previousSelectedSkill.Clear();
                    skill.Setup();
                    previousSelectedSkill = skill;
                    currentState = Selected.Skill;
                    return;
                }

                // Selected the same skill to confirm
                if (skill == previousSelectedSkill)
                {
                    skill.Confirm();
                    skill.Clear();
                    currentState = Selected.Character;
                }

                // Selected an unavailable position == Cancel
                if (previousSelectedSkill.Click(position) == false)
                {
                    previousSelectedSkill.Clear();
                    currentState = Selected.Character;
                    return;
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