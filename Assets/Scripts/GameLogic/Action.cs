using System.Collections.Generic;


namespace Medusa
{

	public class Action
	{

		public Action(Position characterPos, string skillName, List<Position> targetPositions)
		{
			CharacterPos = characterPos;
			SkillName = skillName;
			TargetPositions = targetPositions;
		}


		public Position CharacterPos { get; private set; }


		public string SkillName { get; private set; }


		public List<Position> TargetPositions { get; private set; }
	}

}
