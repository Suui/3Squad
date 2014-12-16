

namespace Medusa
{

    public class TurnManagement
    {

        private readonly Player playerOne;
        private readonly Player playerTwo;


	    public TurnManagement(Player playerOne, Player playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;

            CurrentPlayer = playerOne;
        }


        public void ChangeTurn()
        {
            CurrentPlayer = CurrentPlayer == playerOne ? playerTwo : playerOne;
        }


	    public Player EnemyPlayerThisTurn
        {
            get
            {
                return CurrentPlayer == playerOne ? playerTwo : playerOne;
            }
        }


	    public Player CurrentPlayer { get; private set; }

    }

}