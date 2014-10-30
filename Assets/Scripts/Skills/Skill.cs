using UnityEngine;


namespace Medusa
{

    public class Skill : MonoBehaviour
    {

        public static Vector3 BasicAttackPos    = new Vector3(0.1f, 0.1f, 0.0f);
        public static Vector3 MovementPos       = new Vector3(0.3f, 0.1f, 0.0f);
        public static Vector3 SpecialAttackPos  = new Vector3(0.5f, 0.1f, 0.0f);


        public virtual void ShowUpSkill()
        {
            
        }

    }

}
