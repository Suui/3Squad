using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Medusa
{
	
	public class Horns : Skill
	{
		private int damage = 20;
		private bool doneThisTurn;
		
		List<Position> posibleAttacks = new List<Position>();
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
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/TestButton2") as Texture2D;
			skillGUI.transform.position = ThirdPos;
			skillGUI.transform.parent = gameObject.transform;
			
			skillGUI.GetComponent<SkillToFire>().Skill = this;
		}
		
		public override void Setup()
		{
			Layer terrain = board["tokens"];
			
			foreach ( Direction direction in Direction.AllStaticDirections) {
				Position position = playerPosition + direction;
				if( !position.Outside(terrain) && terrain[position].GetComponent<Life>() != null) {
					board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(2);
				}
			}
		}
		
		public string Clicked(Position pos)
		{
			if(pos.GetDistanceTo(playerPosition) != 1) {
				Clear();
				return "0";
			}
			
			
			if(board["tokens"][pos].GetComponent<Life>() != null)
			{
				targetPosition = pos;
				return "1";
			}
			
			return "2";
		}
		
		public override void Confirm()
		{
			board["tokens"][targetPosition].GetComponent<Life>().Damage(damage);
			Clear();
			doneThisTurn = true;
		}
		
		public override void Clear()
		{
			for(int i = 0; i < posibleAttacks.Count;i++) posibleAttacks[i] = null;
			targetPosition = null;
		}
	}
}