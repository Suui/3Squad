using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

	public class Action
	{

		public Action(Position characterPos, string skillName, List<Position> targetPositions)
		{
			CharacterPos = characterPos;
			SkillName = skillName;
			TargetPositions = targetPositions;

			foreach (var pos in TargetPositions)
			{
				Debug.Log(pos);
			}
		}


		public Position CharacterPos { get; private set; }


		public string SkillName { get; private set; }


		public List<Position> TargetPositions { get; private set; }
	}

}
