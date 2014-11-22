﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{
	
	public class Movement : Skill
	{
		public int range;
		private bool doneThisTurn;
		
		private HashSet<Position> posiblePositions = new HashSet<Position>();
		private LinkedList<Position> stepList = new LinkedList<Position>();
		
		
		private Board board;
		private Position playerPosition;
		private GameObject player;
		
		public void Start()
		{

			doneThisTurn = false;
		}
		
		
		public override void ShowUpSkill()
		{
			GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
			skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/correr") as Texture2D;
			skillGUI.transform.position = SecondPos;
			skillGUI.transform.parent = gameObject.transform;
			
			skillGUI.GetComponent<SkillToFire>().Skill = this;
		}
		
		//show posible movements marking the cells and creating an array of posible movements
		public override void Setup()
		{
			board = FindObjectOfType<GameMaster>().GetComponent<GameMaster>().CurrentBoard;
			playerPosition = (Position) this.transform.position;
			player = this.gameObject;
			SearchWay (posiblePositions, board ["tokens"], playerPosition, range);
			
			foreach(Position posi in posiblePositions)
			{
				board["overlays"][posi].GetComponent<Selectable>().SetOverlayMaterial(2);
				
			}
		}
		
		//add pos to array
		public override bool Click(Position pos)
		{

			if(pos == playerPosition) {
				Clear ();
				return false;
				//return true;
			}

			if(!posiblePositions.Contains (pos))
			{
				return true;
				//return false;
			}

			if(stepList.Count == 0) {
				if(pos.GetDistanceTo(playerPosition) == 1 &&  stepList.Count < range) {
					stepList.AddLast(pos);
					board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(1);
					return true;
				}
				Clear ();
				return false;
			}

			if(pos == stepList.Last.Value)
			{
				stepList.RemoveLast();
				board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(2);
				return true;
			}

			if(stepList.Count >= range)
			{
				return true;
				//return false;
			}

			
			if (pos.GetDistanceTo(stepList.Last.Value) == 1 && !stepList.Contains(pos) && stepList.Count < range) {
				stepList.AddLast(pos);
			
				if(stepList.Count == range)
				{
					board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(4);
				}else
				{
					board["overlays"][pos].GetComponent<Selectable>().SetOverlayMaterial(1);
				}

				return true;
			}


			Clear ();
			return false;
		}
		
		//move to the last pos of array
		public override void Confirm()
		{
			
			
			board["tokens"].MoveGameObject(player,stepList.Last.Value);
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
			stepList.Clear();

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
				if (layer [position] == null) 
				{
						inRange.Add (position);
						SearchWay (inRange, layer, position, stepCount);
				}
			}
			inRange.Remove(playerPosition);
		}
	}
}