using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class ParabolicShot : Skill
	{
		public int damage;
		public int range;
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
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/parabolic") as Texture2D;
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

			SearchWay (posibleAttacks,posibleRange, board ["tokens"], playerPosition, range);
			
			foreach(Position posi in posibleAttacks)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(4);
				
			}
			foreach(Position posi in posibleRange)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(2);
				
			}
		}
		
		//add pos to array
		public override bool Click(Position pos)
		{
			if(targetPosition != null)
			{
				board["overlays"][targetPosition].GetComponent<Selectable>().SetOverlayMaterial(2);
				targetPosition = null;
			}
			
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
			Clear();
			doneThisTurn = true;
			return null;
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
			if(targetPosition != null) board["overlays"][targetPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			posibleAttacks.Clear();
			posibleRange.Clear();
			targetPosition = null;
			
		}

		public void SearchWay (LinkedList<Position> inRange,LinkedList<Position> posibleRange, Layer layer, Position startingPosition, int stepCount)
		{
			if (stepCount-- == 0)
				return;
			Position position;
			foreach (Direction dir in Direction.AllStaticDirections) 
			{
				position = startingPosition + dir;
				
				if (position.Outside(layer))
					continue;
				if (layer[position] != null) 
				{
					if(layer [position].GetComponent<Life>() != null)
					{
						inRange.AddLast (position);
					}
				}else
				{
					posibleRange.AddLast (position);
				}
				SearchWay (inRange,posibleRange, layer, position, stepCount);
			}
			inRange.Remove(playerPosition);
		}
	}
}