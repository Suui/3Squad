using System.Collections.Generic;


namespace Medusa
{

    public class TurnEvents
    {

        public TurnEvents(List<ClickInfo> clickEvents)
        {
            ClickEvents = clickEvents;
        }


        private void GenerateJSON()
        {
            // set up JSON...
        }

        //public JSON auto property...
        public List<ClickInfo> ClickEvents { get; private set; }
    }

}
