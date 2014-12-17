using System.Linq;


namespace Medusa
{

	public class NothingSelectedState : State
	{

		public NothingSelectedState(StateMachine stateMachine) : base(stateMachine)
		{
		}


		public override State ClickButton(string buttonID)
		{
			sm.ShowTransparentBackground(true);
			sm.ShowConfirmCancel(true);

			sm.PreviousId = buttonID;
			sm.PreviousState = this;

			return new ExitEndTurnState(sm);
		}


		public override State ClickSkill(string skillName)
		{
			return this;
		}


		public override State ClickPosition(Position position)
		{
			sm.SelectedToken = sm.Board["tokens"][position];

			// Display Info of the character getting the right components. David.
            //sm.SelectedToken.GetComponent<BaseInfo>().ShowUpInfo();

			sm.ShowInfoButton(true);

			// Selected a Character || Master
			CharacterSelection();

			return new TokenSelectedState(sm);
		}


		private void CharacterSelection()
		{
			if (sm.SelectedToken.GetComponent<Skill>() == null) return;

			if (sm.SelectedToken.GetComponent<PlayerComponent>().Player == sm.PlayingPlayer)
			{
				Skill[] skills = sm.SelectedToken.GetComponents<Skill>();

				foreach (Skill sk in skills.Where(sk => sk.ActionPointCost <= sm.PlayingPlayer.ActionPoints))
					sk.ShowUpSkill();
			}
		}
	}

}
