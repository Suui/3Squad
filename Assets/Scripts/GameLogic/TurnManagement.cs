using UnityEngine;


namespace Medusa
{

    public class TurnManagement
    {

        private readonly GameObject masterOne;
        private readonly GameObject masterTwo;
        private GameObject currentMasterPlaying;


        public TurnManagement(GameObject masterOne, GameObject masterTwo)
        {
            this.masterOne = masterOne;
            this.masterTwo = masterTwo;
            currentMasterPlaying = masterOne;
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


        public GameObject MasterOne
        {
            get { return masterOne; }
        }

        public GameObject MasterTwo
        {
            get { return masterTwo; }
        }

    }

}