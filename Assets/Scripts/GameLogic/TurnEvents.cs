

namespace Medusa
{

    public class TurnEvents
    {

        public TurnEvents(ClickInfo[] clickEvents)
        {
            ClickEvents = clickEvents;
        }


        private void GenerateJSON()
        {
            // set up JSON...
        }

        //public JSON auto property...
        public ClickInfo[] ClickEvents { get; private set; }
    }

}
