namespace GameDriveSample
{
    public partial class Item : global::Colyseus.Schema.Schema
    {
#if UNITY_5_3_OR_NEWER
        [global::UnityEngine.Scripting.Preserve]
#endif
        public Item() { }

        [global::Colyseus.Schema.Type(0, "string")]
        public string name = default(string);

        [global::Colyseus.Schema.Type(1, "uint8")]
        public byte level = default(byte);

        public override string ToString()
        {
            return $"Item( name={name}, level={level})";
        }
    }

    public partial class Player : global::Colyseus.Schema.Schema
    {
#if UNITY_5_3_OR_NEWER
        [global::UnityEngine.Scripting.Preserve]
#endif
        public Player() { }

        [global::Colyseus.Schema.Type(0, "string")]
        public string id = default(string);

        [global::Colyseus.Schema.Type(1, "string")]
        public string name = default(string);

        [global::Colyseus.Schema.Type(2, "int8")]
        public sbyte positionX = default(sbyte);

        [global::Colyseus.Schema.Type(3, "int8")]
        public sbyte positionY = default(sbyte);

        [global::Colyseus.Schema.Type(4, "boolean")]
        public bool connected = true;

        public override string ToString()
        {
            return $"Player( id={id}, name={name}, positionX={positionX}, positionY={positionY}, connected={connected})";
        }
    }

    public partial class MainState : global::Colyseus.Schema.Schema
    {
#if UNITY_5_3_OR_NEWER
        [global::UnityEngine.Scripting.Preserve]
#endif
        public MainState() { }

        [global::Colyseus.Schema.Type(0, "float32")]
        public double timer = default(double);

        [global::Colyseus.Schema.Type(1, "array", typeof(global::Colyseus.Schema.ArraySchema<Player>))]
        public global::Colyseus.Schema.ArraySchema<Player> players = new global::Colyseus.Schema.ArraySchema<Player>();

        [global::Colyseus.Schema.Type(2, "map", typeof(global::Colyseus.Schema.MapSchema<Item>))]
        public global::Colyseus.Schema.MapSchema<Item> itemMap = new global::Colyseus.Schema.MapSchema<Item>();

        public override string ToString()
        {
            // Build players string
            string playersStr = "";
            for (int i = 0; i < players.Count; i++)
            {
                playersStr += players[i].ToString();
                if (i < players.Count - 1)
                {
                    playersStr += ", ";
                }
            }

            string itemsStr = "";
            int index = 0;
            foreach (string key in itemMap.Keys)
            {
                itemsStr += key + ":" + itemMap[key].ToString();
                if (index < itemMap.Count - 1)
                {
                    itemsStr += ", ";
                }
                index++;
            }

            return $"MainState( players=[{playersStr}], items={{ {itemsStr} }})";
        }
    }
}
