using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class Jump : Skill
	{
		public int range;
		private bool doneThisTurn;
		private Position jumpPosition;
		
		private HashSet<Position> posiblePositions = new HashSet<Position>();
		
		
		private Board board;
		private Position playerPosition;
		private GameObject player;
		
		public void Start()
		{
			ActionPointCost = 1;
			doneThisTurn = false;
		}

		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/jump") as Texture2D;
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
			
			SearchWay (posiblePositions, board ["tokens"], playerPosition, range);
			
			foreach(Position posi in posiblePositions)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(2);
				
			}
		}
		
		//add pos to array
		public override bool Click(Position pos)
		{
			
			if(!posiblePositions.Contains (pos))
			{
				return true;
				//return false;
			}
			
			if(pos == playerPosition) {
				Clear ();
				return false;
				//return true;
			}

			if(jumpPosition != null)
			{
				board["overlays"][jumpPosition].GetComponent<Selectable>().SetOverlayMaterial(2);
				jumpPosition = null;

			}
					
			if(posiblePositions.Contains(pos))
			{
				board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(4);
				jumpPosition = pos;
				return true;
			}
			Clear ();
			return false;
		}
		
		//move to the last pos of array
		public override void Confirm()
		{
			board["tokens"].MoveGameObject(player,jumpPosition);
			Clear();
			doneThisTurn = true;
		}
		
		//deselect the cells and empty the array
		public override void Clear()
		{
			
			foreach(Position posi in posiblePositions)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(0);
				
			}
			board["overlays"][playerPosition].GetComponent<Selectable>().SetOverlayMaterial(0);
			posiblePositions.Clear();
			jumpPosition = null;
			
		}
		
		public void SearchWay (HashSet<Position> inRange, Layer layer, Position startingPosition, int stepCount)
		{
			if (stepCount-- == 0)
				return;
			Position position;
			foreach (Direction dir in Direction.AllStaticDirections) 
			{
				position = startingPosition + dir;
				
				if (position.Outside(layer))
					continue;
				if (layer[position] == null) 
				{
					inRange.Add (position);
				}
				SearchWay (inRange, layer, position, stepCount);
			}
			inRange.Remove(playerPosition);
		}


		public override string GetSkillType()
		{
			return GetType().ToString();
		}
	}
}