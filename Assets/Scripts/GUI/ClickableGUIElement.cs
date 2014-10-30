using System;
using UnityEngine;


namespace Medusa
{

    public class ClickableGUIElement : MonoBehaviour
    {

        public delegate void ClickAction(GameObject guiElement, Type skill);
        public static event ClickAction OnClick;


        void OnMouseUp()
        {
            if (OnClick != null)
                OnClick(gameObject.transform.parent.gameObject, GetComponent<SkillToFire>().SkillType);
        }

    }

}

