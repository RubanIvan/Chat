using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class UserModel
    {
        public int UserID;
        public string Name;
        public DateTime LastUpdate;
        public string Color;

        public UserModel(int userId, string name, DateTime lastUpdate, string color)
        {
            UserID = userId;
            Name = name;
            LastUpdate = lastUpdate;
            Color = color;
        }
    }

    public class Message
    {
        public int MsgId;

    }

}