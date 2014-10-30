﻿using System;
using UnityEngine;


namespace Medusa
{

    public class ClickableSkill : MonoBehaviour
    {

        public delegate void ClickAction(GameObject character, Skill skill);
        public static event ClickAction OnClick;


        void OnMouseUp()
        {
            if (OnClick != null)
                OnClick(gameObject.transform.parent.gameObject, GetComponent<SkillToFire>().Skill);
        }

    }

}
