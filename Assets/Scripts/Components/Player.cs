using UnityEngine;


namespace Medusa
{

    class PlayerComponent : MonoBehaviour
    {

        private readonly GameObject masterPlayer;


        public PlayerComponent(GameObject masterPlayer)
        {
            this.masterPlayer = masterPlayer;
        }


        public GameObject MasterPlayer
        {
            get { return masterPlayer; }
        }
    }

}