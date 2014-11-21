using UnityEngine;


namespace Medusa
{

    public class ClickableButton : MonoBehaviour
    {

        public delegate void ButtonClick(ClickInfo clickInfo);
        public static event ButtonClick OnButtonClick;


        void OnMouseUp()
        {
            if (OnButtonClick != null)
                OnButtonClick(new ClickInfo
                    (
                        (Position)gameObject.transform.parent.gameObject.transform,
                        null,
                        Id
                    )
                );
        }


        public string Id { get; set; }

    }

}
