using UnityEngine;


namespace Medusa
{

    class ColorCells : MonoBehaviour
    {

        public GameObject selectionOverlay;

        private Layer overlays;


        void Start()
        {
            overlays = GetComponent<GameMaster>().CurrentBoard["overlays"];
        }


        void OnEnable()
        {
            RaySelection.OnSelection += ColorSelectionCell;
        }


        void OnDisable()
        {
            RaySelection.OnSelection -= ColorSelectionCell;
        }


        private void ColorSelectionCell(Position selectedPos)
        {
            //if (overlays[selectedPos] == null)
            //{
            //    overlays.ClearLayer();

            //    GameObject go = Instantiate(selectionOverlay) as GameObject;
            //    go.name = "overlay " + selectedPos;
            //    go.transform.position = selectedPos;

            //    overlays[selectedPos] = go;
            //}
        }

    }

}