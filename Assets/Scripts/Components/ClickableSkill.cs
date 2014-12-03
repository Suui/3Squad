using UnityEngine;


namespace Medusa
{

    public class ClickableSkill : MonoBehaviour
    {

        public delegate void Selection(ClickInfo clickInfo);
        public static event Selection OnSkillClick;


        void OnMouseUp()
        {
            if (OnSkillClick != null)
                OnSkillClick(new ClickInfo
                    (
                        (Position)gameObject.transform.parent.gameObject.transform,
                        GetComponent<SkillToFire>().Skill.GetSkillType(),
                        null
                    )
                );
        }

    }

}

