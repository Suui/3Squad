using UnityEngine;


namespace Medusa
{

    public class Horns : Skill
    {

        public override void ShowUpSkill()
        {
            GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
            skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/TestButton2") as Texture2D;
            skillGUI.transform.position = SpecialAttackPos;
            skillGUI.transform.parent = gameObject.transform;

            skillGUI.GetComponent<SkillToFire>().SkillType = typeof(Horns);
        }


        // TODO: Remove testing block
        private void Test()
        {
            
        }

    }

}