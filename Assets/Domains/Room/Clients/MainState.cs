using Colyseus.Schema;
using System.Collections.Generic;

namespace GameDriveSample
{
    public partial class Item : Schema
    {
        [Type(0, "string")]
        public string name = default(string);

        [Type(1, "uint8")]
        public byte level = default(byte);

        public override string ToString()
        {
            return $"Item(name={name}, level={level})";
        }
    }

    public partial class Player : Schema
    {
        [Type(0, "string")]
        public string id = default(string);

        [Type(1, "string")]
        public string name = default(string);

        [Type(2, "int8")]
        public sbyte positionX = default(sbyte);

        [Type(3, "int8")]
        public sbyte positionY = default(sbyte);

        [Type(4, "boolean")]
        public bool connected = default(bool);

        public override string ToString()
        {
            return $"Player(id={id}, name={name}, position=({positionX},{positionY}), connected={connected})";
        }
    }

    public partial class MainState : Schema
    {
        [Type(0, "float32")]
        public float timer = default(float);

        [Type(1, "array", typeof(ArraySchema<Player>))]
        public ArraySchema<Player> players = new ArraySchema<Player>();

        [Type(2, "map", typeof(MapSchema<Item>))]
        public MapSchema<Item> itemMap = new MapSchema<Item>();

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
