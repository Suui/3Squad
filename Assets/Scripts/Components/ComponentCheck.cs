using UnityEngine;


namespace Medusa
{

    public class ComponentCheck
    {

        public static bool IsCharacter(GameObject gameObject)
        {
            if (gameObject.GetComponents<Skill>().Length > 0)
                return true;

            return false;
        }


    }

}