using System;
using UnityEngine;



namespace Medusa
{

    public class SkillFirer : MonoBehaviour
    {

        void OnEnable()
        {
            ClickableSkill.OnClick += FireSkill;
        }


        void OnDisable()
        {
            ClickableSkill.OnClick -= FireSkill;
        }


        private void FireSkill(GameObject character, Skill skill)
        {
            Debug.Log("Pressed button! " + "Parent gameObject is: " + character.name + ". Skill type is: " + skill);
            // skill.FuncionX();
        }

    }

}