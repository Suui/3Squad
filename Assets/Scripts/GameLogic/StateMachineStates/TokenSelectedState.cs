using System.Linq;
using UnityEngine;


namespace Medusa
{

	public class TokenSelectedState : State
	{

		public TokenSelectedState(StateMachine stateMachine) : base(stateMachine)
		{
		}


		public override State ClickButton(string buttonID)
		{
			sm.ShowTransparentBackground(true);

			if (buttonID != "Info")
				sm.ShowConfirmCancel(true);

			GameObject[] skillIcons = GameObject.FindGameObjectsWithTag("SkillIcon");

			foreach (var go in skillIcons)
				Object.Destroy(go);

			sm.PreviousId = buttonID;
			sm.PreviousState = this;

			return new ExitEndTurnState(sm);
		}


		public override State ClickSkill(string skillName)
		{
			sm.SelectedSkill = sm.SelectedToken.GetComponent(skillName) as Skill;
			sm.SelectedSkill.Setup();

			foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
				Object.Destroy(go);

			sm.ShowConfirmCancel(true);
			sm.ShowExitEndTurn(false);
			sm.ShowInfoButton(false);

			return new SkillSelectedState(sm);
		}


		public override State ClickPosition(Position position)
		{
			sm.SelectedToken = sm.Board["tokens"][position];

			// We are also deleting the BaseInfo GUI within this foreach, as it is tagged as SkillIcon
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
				Object.Destroy(go);

			sm.SelectedToken.GetComponent<BaseInfo>().ShowUpInfo();

			// Selected another Character || Master
			CharacterSelection();

			return this;
		}


		private void CharacterSelection()
		{
			if (sm.SelectedToken.GetComponent<Skill>() == null) return;

			GameMaster gameMaster = sm.gameObject.GetComponent<GameMaster>();

			if (sm.SelectedToken.GetComponent<PlayerComponent>().Player == sm.PlayingPlayer && sm.PlayingPlayer == gameMaster.TurnManagement.CurrentPlayer)
			{
				Skill[] skills = sm.SelectedToken.GetComponents<Skill>();

				foreach (Skill sk in skills.Where(sk => sk.ActionPointCost <= sm.PlayingPlayer.ActionPoints))
					sk.ShowUpSkill();
			}
		}

	}

}