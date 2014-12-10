using UnityEngine;
using System.Collections.Generic;


namespace Medusa
{
	
	public class PhysicalAttack : Skill
	{
		
		public int damage;
		private bool doneThisTurn;
		
		LinkedList<Position> posibleAttacks = new LinkedList<Position>();
		Position targetPosition;
		
		private Position playerPosition;
		private GameObject player;
		private Board board;


		void Start()
		{
			ActionPointCost = 1;
		}
		

		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/espada") as Texture2D;
			skillGUI.transform.position = FirstPos;
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
			
			if(targetPosition != null)
			{
				board["overlays"][targetPosition].GetComponent<Selectable>().SetOverlayMaterial(2);
				targetPosition = null;
			}

			if(board["tokens"][pos].GetComponent<Life>() != null && pos != playerPosition)
			{
				targetPosition = pos;
				Debug.Log(pos);
				board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(4);
				return true;
			}
			Clear ();
			return false;
		}

		public override List<Position> Confirm()
		{
			board["tokens"][targetPosition].GetComponent<Life>().Damage(damage);

			List<Position> targetPositions = new List<Position> { targetPosition };

			Clear();
			doneThisTurn = true;

			return targetPositions;
		}
		
		public override void Clear()
		{
			foreach(Position posi in posibleAttacks)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(0);
				
			}
			board["overlays"][playerPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			if(targetPosition != null) board["overlays"][targetPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			posibleAttacks.Clear();
			targetPosition = null;
		}


		public override string GetSkillType()
		{
			return GetType().ToString();
		}
	}
}