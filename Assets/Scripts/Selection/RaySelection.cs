using UnityEngine;


namespace Medusa
{

    class RaySelection : MonoBehaviour
    {

        public delegate void SelectAction(Position selectedPos);
        public static event SelectAction OnSelection;

        private Position selectedPos;

        private Ray raySelection;
        private RaycastHit rayHit;
        private Layer overlay;


        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                raySelection = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(raySelection, out rayHit))
                {
                    selectedPos = new Position((int)rayHit.transform.position.x, (int)rayHit.transform.position.z);

                    if (OnSelection != null)
                        OnSelection(selectedPos);
                }
            }
        }


        public Position SelectedPos
        {
            get { return selectedPos; }
        }
    }

}