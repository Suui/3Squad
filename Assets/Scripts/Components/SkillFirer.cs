using System;
using UnityEngine;



namespace Medusa
{

    public class SkillFirer : MonoBehaviour
    {

        void OnEnable()
        {
            ClickableGUIElement.OnClick += FireSkill;
        }


        void OnDisable()
        {
            ClickableGUIElement.OnClick -= FireSkill;
        }


        private void FireSkill(GameObject gameObject, Type skillType)
        {
            Debug.Log("Pressed button!");
            Debug.Log("Parent gameObject is: " + gameObject.name);
            Debug.Log("Skill type is: " + skillType);
        }

    }

}