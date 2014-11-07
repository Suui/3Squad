using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class Movement : Skill
	{
		public int range;
		private bool doneThisTurn;
		
		private List<Position> posiblePositions = new List<Position>();
		private List<Position> stepList = new List<Position>();
		
		
		private Board board;
		private Position playerPosition;
		private GameObject player;
		
		public void Start()
		{
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
			doneThisTurn = false;
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			
		}
		
		
		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/TestButton") as Texture2D;
			skillGUI.transform.position = SecondPos;
			skillGUI.transform.parent = gameObject.transform;
			
			skillGUI.GetComponent<SkillToFire>().Skill = this;
		}
		
		//show posible movements marking the cells and creating an array of posible movements
		public override void Setup()
		{
			
			SearchWay (posiblePositions, board ["tokens"], playerPosition, range);
			
			foreach(Position posi in posiblePositions)
			{
				Debug.Log(posi + " " + board["overlays"][posi].name);
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(2);
				
			}
		}
		
		//add pos to array
		public override bool Click(Position pos)
		{
			
			if(!posiblePositions.Contains (pos))
			{
				return false;
			}
			
			if(pos == playerPosition) {
				return false;
			}
			
			if(stepList.Count == 0) {
				if(pos.GetDistanceTo(playerPosition) == 1 &&  stepList.Count < range) {
					stepList.Add(pos);
					return true;
				}
				
				return false;
			}
			
			/*if(pos == stepList[stepList.Count - 1]) {
				stepList.RemoveAt((stepList.Count - 1));
				return "2";
			}*/
			
			if (pos.GetDistanceTo(stepList[stepList.Count - 1]) == 1 && !stepList.Contains (pos) && stepList.Count < range) {
				stepList.Add(pos);
				return true;
			}
			
			return false;
		}
		
		//move to the last pos of array
		public override void Confirm()
		{
			
			
			board["tokens"].MoveGameObject(player,stepList[stepList.Count - 1]);
			Clear();
			doneThisTurn = true;
		}
		
		//deselect the cells and empty the array
		public override void Clear()
		{
			for(int i = 0; i < posiblePositions.Count;i++) posiblePositions[i] = null;
			for(int j = 0; j < stepList.Count;j++) stepList[j] = null;
		}
		
		public void SearchWay (List<Position> inRange, Layer layer, Position startingPosition, int stepCount)
		{
			if (stepCount-- == 0)
				return;
			Position position;
			foreach (Direction dir in Direction.AllStaticDirections) {
				position = startingPosition + dir;
				if (position.Outside(layer))
					continue;
				if (layer [position] == null) {
					inRange.Add (position);
					SearchWay (inRange, layer, position, stepCount);
				}
			}
		}
	}
}