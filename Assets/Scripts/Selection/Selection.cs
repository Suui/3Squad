

namespace Medusa
{

    class Selection
    {

        private SelectionStates selectionState;

        public Selection()
        {
            selectionState = SelectionStates.NothingSelected;
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


        public SelectionStates SelectionState
        {
            get { return selectionState; }
        }
    }


    public enum SelectionStates
    {
        NothingSelected,
        CharacterSelected,
        SkillSelected,
        SkillConfirmAction
    }

}