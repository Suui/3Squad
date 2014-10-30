﻿using UnityEngine;


namespace Medusa
{

    public class BasicAttack : Skill
    {

        public override void ShowUpSkill()
        {
            GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
            skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/TestButton2") as Texture2D;
            skillGUI.transform.position = BasicAttackPos;
            skillGUI.transform.parent = gameObject.transform;

            skillGUI.GetComponent<SkillToFire>().Skill = this;
        }


        // TODO: Remove testing block
        public override void FireSkill()
        {
            Debug.Log("Basic Attack!");
        }
    }

}