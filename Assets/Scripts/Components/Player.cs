using UnityEngine;


namespace Medusa
{

    class Player : MonoBehaviour
    {

        private readonly GameObject masterPlayer;


        public Player(GameObject masterPlayer)
        {
            this.masterPlayer = masterPlayer;
        }


        public GameObject MasterPlayer
        {
            get { return masterPlayer; }
        }
    }

}