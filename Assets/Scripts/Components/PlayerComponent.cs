using UnityEngine;


namespace Medusa
{
	
	public class PlayerComponent : MonoBehaviour
	{
		public PlayerComponent(Player player)
		{
			Player = player;
		}
		
		
		public Player Player { get; set; }
	}
	
}