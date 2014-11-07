using UnityEngine;


namespace Medusa
{

    class TurnManagement : MonoBehaviour
    {

        public readonly GameObject masterOne;
        public readonly GameObject masterTwo;
        private GameObject currentMasterPlaying;


        public TurnManagement(GameObject masterOne, GameObject masterTwo)
        {
            this.masterOne = masterOne;
            this.masterTwo = masterTwo;
            currentMasterPlaying = masterOne;
        }


        public void ChangeTurn()
        {
            if (currentMasterPlaying == masterOne)
                currentMasterPlaying = masterTwo;
            else
                currentMasterPlaying = masterOne;
        }


        public GameObject CurrentMasterPlaying
        {
            get { return currentMasterPlaying; }
        }


        public GameObject EnemyMasterThisTurn
        {
            get
            {
                if (currentMasterPlaying == masterOne)
                    return masterTwo;

                return masterOne;
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