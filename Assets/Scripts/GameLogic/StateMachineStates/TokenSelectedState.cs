﻿using System.Linq;
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

			// Delete Info of the token getting the right components. David.
			// Display Info of the token getting the right components. David.

			foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
				Object.Destroy(go);

			// Selected another Character || Master
			CharacterSelection();

			return this;
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