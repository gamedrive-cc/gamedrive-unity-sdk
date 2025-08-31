using System.Collections.Generic;

namespace GameDriveSample
{


    public class MessageTypes
    {
        public const string MESSAGE_TYPE_MOVE_PLAYER = "MOVE_PLAYER";
        public const string MESSAGE_TYPE_ADD_ITEM = "ADD_ITEM";
        public const string MESSAGE_TYPE_REMOVE_ITEM = "REMOVE_ITEM";

        public const string MESSAGE_CHAT_DM = "CHAT_DM";
        public const string MESSAGE_CHAT_BC = "CHAT_BC";

        public static readonly string[] Types = {
            MESSAGE_TYPE_MOVE_PLAYER,
            MESSAGE_TYPE_ADD_ITEM,
            MESSAGE_TYPE_REMOVE_ITEM,
            MESSAGE_CHAT_DM,
            MESSAGE_CHAT_BC
        };
    }
}