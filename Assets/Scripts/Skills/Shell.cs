using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class Shell : Skill
	{
		public int damage;
		private bool doneThisTurn;
		
		private LinkedList<Position> posibleAttacks = new LinkedList<Position>();
		private LinkedList<Position> posibleRange = new LinkedList<Position>();
		private Position targetPosition;
		
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
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/shell") as Texture2D;
			skillGUI.transform.position = ThirdPos;
			skillGUI.transform.parent = gameObject.transform;
			
			skillGUI.GetComponent<SkillToFire>().Skill = this;
		}
		

		//show posible movements marking the cells and creating an array of posible movements
		public override void Setup()
		{
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
			Layer terrain = board["tokens"];
			
			foreach (Direction direction in Direction.AllStaticDirections) 
			{
				Position position = playerPosition + direction;
				
				while(!position.Outside(terrain) && board["tokens"][position] == null)
				{
					board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(2);
					posibleRange.AddLast(position);	
					position += direction;
					
				}
				if(position-direction != playerPosition)
				{
					board["overlays"][position-direction].GetComponent<Selectable>().SetOverlayMaterial(4);
					posibleAttacks.AddLast(position-direction);
				}
			}
		}

		
		//add pos to array
		public override bool Click(Position pos)
		{
			
			
			if(posibleAttacks.Contains(pos))
			{
				targetPosition = pos;
				board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(3);
				return true;	
			}
			
			Clear ();
			return false;
		}
		

		//move to the last pos of array
		public override List<Position> Confirm()
		{
			board["tokens"].MoveGameObject(player,targetPosition);
			playerPosition= targetPosition;
			foreach (Direction direction in Direction.AllStaticDirections) 
			{
				Position position = playerPosition + direction;
				if(!position.Outside(board["tokens"]) && board["tokens"][position] != null && board["tokens"][position].GetComponent<Life>() != null) {
					board["tokens"][position].GetComponent<Life>().Damage(damage);
				}
			}

			List<Position> returnPosition = new List<Position> {targetPosition};

			Clear();
			doneThisTurn = true;
			return returnPosition;
		}
		

		//deselect the cells and empty the array
		public override void Clear()
		{
			
			foreach(Position posi in posibleAttacks)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(0);
				
			}
			foreach(Position posi in posibleRange)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(0);
				
			}
			board["overlays"][playerPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			posibleAttacks.Clear();
			posibleRange.Clear();
			targetPosition = null;
			
		}
	}
}

