using UnityEngine;


namespace Medusa
{

    public class ClickableSkill : MonoBehaviour
    {

        public delegate void Selection(Position characterPos, Skill skill);
        public static event Selection OnSkillClick;


        void OnMouseUp()
        {
            if (OnSkillClick != null)
                OnSkillClick((Position)gameObject.transform.parent.gameObject.transform, GetComponent<SkillToFire>().Skill);
        }

    }

}

