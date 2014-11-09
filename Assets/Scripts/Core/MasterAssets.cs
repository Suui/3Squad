using UnityEngine;


namespace Medusa
{

    public class MasterAssets
    {
        private GameObject master;
        private GameObject overlay;


        public MasterAssets(GameObject master, GameObject overlay)
        {
            this.master = master;
            this.overlay = overlay;
        }


        public GameObject Master
        {
            get { return master; }
            set { master = value; }
        }

        public GameObject Overlay
        {
            get { return overlay; }
            set { overlay = value; }
        }
    }

}