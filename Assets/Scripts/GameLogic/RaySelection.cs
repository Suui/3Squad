using UnityEngine;


namespace Medusa
{

    class RaySelection : MonoBehaviour
    {

        public delegate void Selection(ClickInfo clickInfo);
        public static event Selection OnSelection;

        private Position selectedPos;

        private Ray raySelection;
        private RaycastHit rayHit;


        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                raySelection = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(raySelection, out rayHit))
                {
                    selectedPos = new Position((int)rayHit.transform.position.x, (int)rayHit.transform.position.z);

                    if (OnSelection != null)
                        OnSelection(new ClickInfo(selectedPos, null, null));
                }
            }
        }


        public Position SelectedPos
        {
            get { return selectedPos; }
        }
    }

}