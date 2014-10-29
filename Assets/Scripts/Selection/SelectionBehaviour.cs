

namespace Medusa
{

    class SelectionBehaviour
    {

        private State selectionState;


        public SelectionBehaviour()
        {
            selectionState = State.NothingSelected;
            OnNothingSelected();
        }


        private void OnNothingSelected()
        {
            
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