using Colyseus.Schema;

namespace GameDrive
{
    public class DefaultMainState : Schema
    {
        [Type(0, "string")]
        public string defaultField;
    }
}