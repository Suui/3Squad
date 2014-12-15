using UnityEngine;
using System.Collections;
using System.Collections.Generic;
		
namespace Medusa
{
			
	public class Horns : Skill
	{
				
		public int damage;
		private bool doneThisTurn;
		
		LinkedList<Position> posibleAttacks = new LinkedList<Position>();
		Position targetPosition;
		
		private Position playerPosition;
		private GameObject player;
		private Board board;
				
		public void Start()
		{
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
					
					
		}
			
		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/horn") as Texture2D;
			skillGUI.transform.position = ThirdPos;
			skillGUI.transform.parent = gameObject.transform;
			
			skillGUI.GetComponent<SkillToFire>().Skill = this;
		}
				

		public override void Setup()
		{
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
			Layer terrain = board["tokens"];
			
			foreach (Direction direction in Direction.AllStaticDirections) {
				
				Position position = playerPosition + direction;
				if(!position.Outside(terrain) && terrain[position] != null && terrain[position].GetComponent<Life>() != null) {
					board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(2);
				}
			}
		}
		
		public override bool Click(Position pos)
		{
			if(pos.GetDistanceTo(playerPosition) != 1) {
				Clear();
				return false;
			}
			
			
			if(board["tokens"][pos].GetComponent<Life>() != null && pos != playerPosition)
			{
				targetPosition = pos;
				board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(4);
				return true;
			}
			Clear ();
			return false;
		}
		
		public override LinkedList<Position> Confirm()
		{
			board["tokens"][targetPosition].GetComponent<Life>().Damage(damage);
			Clear();
			doneThisTurn = true;
			return null;
		}
		
		public override void Clear()
		{
			foreach(Position posi in posibleAttacks)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(0);
				
			}
			board["overlays"][playerPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			//board["overlays"][targetPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			posibleAttacks.Clear();
			
			targetPosition = null;
		}
	}
}