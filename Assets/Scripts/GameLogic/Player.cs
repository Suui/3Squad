

namespace Medusa
{

    public class Player
    {
        // TODO: Might remove the name, using now for debugging
        public string name;

        private int actionPoints;


        public Player(string name, int actionPoints)
        {
            this.name = name;
            this.actionPoints = actionPoints;
        }


        public int ActionPoints
        {
            get { return actionPoints; }
            set { actionPoints = value; }
        }

    }

}
