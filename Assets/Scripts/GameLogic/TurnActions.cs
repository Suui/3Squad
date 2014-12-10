using System.Collections.Generic;


namespace Medusa
{

    public class TurnActions
    {

        public TurnActions(List<Action> actions)
        {
	        Actions = actions;
        }


        private void GenerateJSON()
        {
            // set up JSON...
        }

        //public JSON auto property...
        public List<Action> Actions { get; private set; }
    }

}
