

namespace Medusa
{

    class SelectionBehaviour
    {

        private Board board;
        private State selectionState;
        private RaySelection raySelection;


        public SelectionBehaviour(Board board)
        {
            this.board = board;
            selectionState = State.NothingSelected;
        }


        public void Begin()
        {
            OnNothingSelected();
        }


        private void OnNothingSelected()
        {
            Position pos = raySelection.SelectedPos;
            if (ComponentCheck.IsCharacter(board["tokens"][pos]))
            {
                OnCharacterSelected();
            }
        }


        private void OnCharacterSelected()
        {
            
        }


        private void OnSkillSeleceted()
        {
            
        }


        private void OnSkillConfirmAction()
        {
            
        }


        public State SelectionState
        {
            get { return selectionState; }
        }
    }


    public enum State
    {
        NothingSelected,
        CharacterSelected,
        SkillSelected,
        SkillConfirmAction
    }

}