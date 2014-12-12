

using System.Linq;

namespace Medusa
{

	public class SkillSelectedState : State
	{

		public SkillSelectedState(StateMachine stateMachine) : base(stateMachine)
		{
		}


		public override State ClickButton(string buttonID)
		{
			// Selected the Confirm Button
			if (buttonID == "Confirm")
			{
				// Confirm and Add the Action to the list for the other player to see it when we change turn
				sm.Actions.Add(new Action
					(
						(Position) sm.SelectedToken.transform.position, 
						sm.SelectedSkill.GetSkillType(), 
						sm.SelectedSkill.Confirm()		// Confirmed!
					));

				// Manage Action Points
				sm.PlayingPlayer.ActionPoints -= sm.SelectedSkill.ActionPointCost;
			}

			// The following code is executed whether we Confirm OR Cancel
			// Those are the ONLY possible values buttonID MUST have in this call

			sm.SelectedSkill.Clear();

			// Set the selection to the character again
			Position pos = (Position)sm.SelectedToken.transform.position;
			sm.DisplaySelectionOverlay(pos);

			Skill[] skills = sm.SelectedToken.GetComponents<Skill>();

			foreach (Skill sk in skills.Where(sk => sk.ActionPointCost <= sm.PlayingPlayer.ActionPoints))
				sk.ShowUpSkill();

			sm.ShowConfirmCancel(false);
			sm.ShowExitEndTurn(true);
			sm.ShowInfoButton(true);

			return new TokenSelectedState(sm);
		}
		
		
		
		public override State ClickPosition(Position position)
		{
			return this;
		}


		public override State ClickSkill(string skillName)
		{
			return this;
		}

	}

}