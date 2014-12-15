using UnityEngine;


namespace Medusa
{

    public class Selectable : MonoBehaviour
    {

        public Material[] colorMaterials;
        public int voidMaterialIndex = 0;


        void Start()
        {
            gameObject.renderer.material = colorMaterials[voidMaterialIndex];
        }


        public void SetOverlayMaterial(int materialIndex)
        {
            gameObject.renderer.material = colorMaterials[materialIndex];

        }

    }

}