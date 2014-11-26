using UnityEngine;


namespace Medusa
{

    class PlayerComponent : MonoBehaviour
    {
        public PlayerComponent(Player player)
        {
            Player = player;
        }


        public Player Player { get; set; }
    }

}