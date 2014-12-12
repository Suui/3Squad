using System.Linq;
using UnityEngine;


namespace Medusa
{

	public class ExitEndTurnState : State
	{

		public delegate void ChangingTurn(TurnActions turnActions);
		public static event ChangingTurn OnChangingTurn;


		public ExitEndTurnState(StateMachine stateMachine) : base(stateMachine)
		{
		}


		public override State ClickButton(string buttonID)
		{
			if (buttonID == "Confirm")
			{
				if (sm.PreviousId == "Exit")
				{
					// TODO: Save game state and exit (need more stuff here)

					Application.Quit();
					return this;
				}

				if (sm.PreviousId == "EndTurn")
				{
					sm.ShowInfoButton(false);
					sm.ShowConfirmCancel(false);
					sm.ShowTransparentBackground(false);
					sm.ShowExitEndTurn(false);
					sm.DisplaySelectionOverlay(null);

					if (OnChangingTurn != null)
						OnChangingTurn(new TurnActions(sm.Actions));
				}
			}

			if (buttonID == "Cancel")
			{
				sm.ShowConfirmCancel(false);
				sm.ShowTransparentBackground(false);

				if (sm.PreviousState.GetType() == typeof(TokenSelectedState) && sm.SelectedToken != null && sm.SelectedToken.GetComponent<Skill>())
				{
					Skill[] skills = sm.SelectedToken.GetComponents<Skill>();

					foreach (Skill sk in skills.Where(sk => sk.ActionPointCost <= sm.PlayingPlayer.ActionPoints))
						sk.ShowUpSkill();
				}

				return sm.PreviousState;
			}

			return this;
		}


		public override State ClickSkill(string skillName)
		{
			return this;
		}


		public override State ClickPosition(Position position)
		{
			return this;
		}

	}

}