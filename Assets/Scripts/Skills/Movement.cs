﻿using UnityEngine;


namespace Medusa
{

    public class Movement : Skill
    {

        public override void ShowUpSkill()
        {
            GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
            skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/TestButton") as Texture2D;
            skillGUI.transform.position = MovementPos;
            skillGUI.transform.parent = gameObject.transform;

            skillGUI.GetComponent<SkillToFire>().SkillType = typeof(Movement);
        }


    }

}