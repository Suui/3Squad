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


        private void FireSkill(GameObject gameObject, Type skillType)
        {
            Debug.Log("Pressed button! " + "Parent gameObject is: " + gameObject.name + ". Skill type is: " + skillType);
        }

    }

}