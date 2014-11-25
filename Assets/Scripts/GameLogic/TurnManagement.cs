using UnityEngine;


namespace Medusa
{

    public class TurnManagement
    {

        private readonly Player playerOne;
        private readonly Player playerTwo;
        private Player currentPlayer;


        public TurnManagement(Player playerOne, Player playerTwo, int seed)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;

            System.Random randStarter = new System.Random(seed);
            int starter = randStarter.Next(0, 1000) % 2;

            currentPlayer = starter == 0 ? playerOne : playerTwo;
        }


		public override string ToString()
		{
			return currentPlayer == playerOne ? "Player 01" : "Player 02";
		}


        public void ChangeTurn()
        {
            currentPlayer = currentPlayer == playerOne ? playerTwo : playerOne;
        }


        public Player CurrentPlayer
        {
            get { return currentPlayer; }
        }


        public Player EnemyPlayerThisTurn
        {
            get
            {
                return currentPlayer == playerOne ? playerTwo : playerOne;
            }
        }

    }

}