using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class DistanceAttack : Skill
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
			Clear ();
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
		
		}
		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/arco") as Texture2D;
			skillGUI.transform.position = FirstPos;
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
				//Position posiblePosition = position + direction;
				if(!position.Outside(terrain) && terrain[position] != null && terrain[position].GetComponent<Life>() != null) {
					board["overlays"][position].GetComponent<Selectable>().SetOverlayMaterial(4);
					posibleAttacks.AddLast(position);
				}
			}
		}
		
		//add pos to array
		public override bool Click(Position pos)
		{


			if(posibleAttacks.Contains(pos))
			{
				if(board["tokens"][pos].GetComponent<Life>() != null)
				{
					targetPosition = pos;
					board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(3);
					return true;
				}
				Clear ();
				return false;

			}

			Clear ();
			return false;
		}
		
		//move to the last pos of array
		public override LinkedList<Position> Confirm()
		{
			board["tokens"][targetPosition].GetComponent<Life>().Damage(damage);
			LinkedList<Position> returnValue = new LinkedList<Position>();
			returnValue.AddLast(targetPosition);
			doneThisTurn = true;
			return returnValue;
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
