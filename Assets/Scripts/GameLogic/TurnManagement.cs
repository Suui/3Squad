

namespace Medusa
{

    public class TurnManagement
    {

        private readonly Player playerOne;
        private readonly Player playerTwo;


	    public TurnManagement(Player playerOne, Player playerTwo, int seed)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;

            System.Random randStarter = new System.Random(seed);
            int starter = randStarter.Next(0, 1000) % 2;

            CurrentPlayer = starter == 0 ? playerOne : playerTwo;
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