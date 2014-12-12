using System.Linq;


namespace Medusa
{

	public class NothingSelected
	{

		private readonly SelectionStateMachine sm;


		public NothingSelected(SelectionStateMachine sm)
		{
			this.sm = sm;
		}


		public bool ButtonSelection(string buttonID)
		{
			if (buttonID == null) return false;

			sm.ShowTransparentBackground(true);
            sm.ShowConfirmCancel(true);

            sm.PreviousId = buttonID;
            sm.PreviousState = Selected.Nothing;

            sm.CurrentState = Selected.ExitEndTurn;

			return true;
		}


		public void TokenSelection(Position position)
		{
			if (sm.Board["tokens"][position] == null) return;

			sm.SelectedToken = sm.Board["tokens"][position];

            // Display Info of the character getting the right components. David.

            sm.ShowInfoButton(true);

            // Selected a Character || Master
			CharacterSelection();

			sm.CurrentState = Selected.Token;
		}


		public void CharacterSelection()
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
