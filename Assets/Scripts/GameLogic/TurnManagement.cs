using UnityEngine;


namespace Medusa
{

    public class TurnManagement
    {

        private readonly GameObject masterOne;
        private readonly GameObject masterTwo;
        private GameObject currentMasterPlaying;


        public TurnManagement(GameObject masterOne, GameObject masterTwo, int seed)
        {
            this.masterOne = masterOne;
            this.masterTwo = masterTwo;

            System.Random randStarter = new System.Random(seed);
            int starter = randStarter.Next(0, 1000) % 2;

            currentMasterPlaying = starter == 0 ? masterOne : masterTwo;
        }


        public void ChangeTurn()
        {
            currentMasterPlaying = currentMasterPlaying == masterOne ? masterTwo : masterOne;
        }


        public GameObject CurrentMasterPlaying
        {
            get { return currentMasterPlaying; }
        }


        public GameObject EnemyMasterThisTurn
        {
            get
            {
                return currentMasterPlaying == masterOne ? masterTwo : masterOne;
            }
        }

    }

}