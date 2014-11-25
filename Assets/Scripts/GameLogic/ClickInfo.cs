using UnityEngine;


namespace Medusa
{

    public class ClickInfo
    {

        private readonly Position position;
        private readonly Skill skill;
        private readonly string buttonId;


        public ClickInfo(Position position, Skill skill, string buttonId)
        {
            this.position = position;
            this.skill = skill;
            this.buttonId = buttonId;
        }


        public Position Position
        {
            get { return position; }
        }


        public Skill Skill
        {
            get { return skill; }
        }


        public string ButtonId
        {
            get { return buttonId; }
        }
    }

}
