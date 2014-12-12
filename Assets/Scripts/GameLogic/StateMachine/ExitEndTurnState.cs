

namespace Medusa
{

	public class ExitEndTurnState : State
	{

		public ExitEndTurnState(StateMachine stateMachine) : base(stateMachine)
		{
		}


		public override State ClickPosition(Position pos)
		{
			throw new System.NotImplementedException();
		}


		public override State ClickButton(string buttonID)
		{
			throw new System.NotImplementedException();
		}


		public override State ClickSkill(string skillName)
		{
			throw new System.NotImplementedException();
		}

	}

}