using UnityEngine;


namespace Medusa
{

    class PlayerComponent : MonoBehaviour
    {

        private Player player;


        public PlayerComponent(Player player)
        {
            this.player = player;
        }


        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
    }

}