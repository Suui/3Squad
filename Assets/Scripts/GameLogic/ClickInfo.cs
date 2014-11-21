using UnityEngine;


namespace Medusa
{

    public class ClickInfo
    {

        private readonly Position position;
        private readonly Skill skill;


        public ClickInfo(Position position, Skill skill)
        {
            this.position = position;
            this.skill = skill;
        }


        public Position Position
        {
            get { return position; }
        }


        public Skill Skill
        {
            get { return skill; }
        }
    }

}
