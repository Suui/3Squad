

namespace Medusa
{

    public class ClickInfo
    {

        private readonly Position position;
        private readonly string skillName;
        private readonly string buttonId;


        public ClickInfo(Position position, string skillName, string buttonId)
        {
            this.position = position;
            this.skillName = skillName;
            this.buttonId = buttonId;
        }


        public Position Position
        {
            get { return position; }
        }


        public string SkillName
        {
            get { return skillName; }
        }


        public string ButtonId
        {
            get { return buttonId; }
        }
    }

}
