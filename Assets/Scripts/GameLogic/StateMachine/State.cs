


namespace Medusa
{

	public abstract class State
	{

		protected readonly StateMachine sm;


		protected State(StateMachine stateMachine)
		{
			sm = stateMachine;
		}


		public abstract State ClickPosition(Position pos);


		public abstract State ClickButton(string buttonID);


		public abstract State ClickSkill(string skillName);

	}

}