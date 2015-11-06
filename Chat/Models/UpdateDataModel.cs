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

    public class MessageModel
    {
        public int MsgId;
        public DateTime Time;
        public string Name;
        public string Color;
        public string Mesage;

        public MessageModel(int msgId, DateTime time, string name, string color, string mesage)
        {
            MsgId = msgId;
            Time = time;
            Name = name;
            Color = color;
            Mesage = mesage;
        }
    }

    public class UpdateModel
    {
        public List<UserModel> Users =new List<UserModel>();
        public List<MessageModel> Messages =new List<MessageModel>(); 
    }

}