using UnityEngine;


namespace Medusa
{

    class ComponentChecker : MonoBehaviour
    {

        private Board board;


        void Start()
        {
            board = GetComponent<GameMaster>().CurrentBoard;
        }


        void OnEnable()
        {
            RaySelection.OnSelection += CheckToken;
        }


        void OnDisable()
        {
            RaySelection.OnSelection -= CheckToken;
        }


        private void CheckToken(Position selectedPos)
        {

            if (board["tokens"][selectedPos] != null)
            {
                Skill[] skills = board["tokens"][selectedPos].GetComponents<Skill>();

                foreach (Skill sk in skills)
                    sk.ShowUpSkill();
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("SkillIcon"))
                {
                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillIcon"))
                        Destroy(go);
                }
            }

            // TODO: Add elses and check on other layers if desired...
        }

    }

}