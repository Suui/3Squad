using UnityEngine;


namespace Medusa
{

    public class SelectionSM : MonoBehaviour
    {

        private Board board;

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
            RaySelection.OnSelection    += CheckSelection;
            ClickableSkill.OnSkillClick += CheckSelection;
        }


        void OnDisable()
        {
            RaySelection.OnSelection    -= CheckSelection;
            ClickableSkill.OnSkillClick -= CheckSelection;
        }


        void Start()
        {
            board = GetComponent<GameMaster>().CurrentBoard;

            previousSelectedSkill = null;
            previousSelectedPos = new Position(0, 0);
            currentState = Selected.Nothing;
        }


        private void CheckSelection(Position position, Skill skill)
        {
            Debug.Log("Selection Performed! " + 
                "Current SM State is: " + currentState +
                ". Parameter position is: " + position + 
                ". Parameter Skill is: " + skill);

            // NOTHING
            if (currentState == Selected.Nothing)
            {
                DisplaySelectionOverlay(position);

                // Selected a Master
                if (board.IsInside(position) == false)
                {
                    Skill[] skills = board.Masters[position].Master.GetComponents<Skill>();

                    foreach (Skill sk in skills)
                        sk.ShowUpSkill();

                    currentState = Selected.Character;
                    return;
                }

                // Selected a Character
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

                // Selected another Master
                if (board.IsInside(position) == false)
                {
                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                        Destroy(go);

                    Skill[] skills = board.Masters[position].Master.GetComponents<Skill>();

                    foreach (Skill sk in skills)
                        sk.ShowUpSkill();

                    return;
                }

                // Selected another Character
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
            if (board.IsInside(previousSelectedPos))
                board["overlays"][previousSelectedPos].GetComponent<Selectable>().SetOverlayMaterial(0);
            else
                board.Masters[previousSelectedPos].Overlay.GetComponent<Selectable>().SetOverlayMaterial(0);

            if (board.IsInside(position))
                board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(1);
            else
                board.Masters[position].Overlay.GetComponent<Selectable>().SetOverlayMaterial(1);

            previousSelectedPos = position;
        }
    }

}