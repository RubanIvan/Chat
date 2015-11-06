using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.Models;

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            //Пользователь должен быть авторизован
            if(Request.Cookies["ChatAutorize"]==null || Request.Cookies["ChatAutorize"].Value!="1") return RedirectToAction("Login", "Authorization");

            return View();
        }

        [HttpPost]
        public JsonResult Update()
        {
            //Пользователь должен быть авторизован
            if (Request.Cookies["ChatAutorize"] == null || Request.Cookies["ChatAutorize"].Value != "1") return null;
            string login = Request.Cookies["LoginName"].Value;
            if (login == null) return null;

            UpdateModel model =new UpdateModel();

            //List<UserModel> Users=new List<UserModel>();

            //обновляем временную метку (пользователь не отключился от нас)
            SqlConnection con = DataBase.GetSqlConnection();
            SqlCommand command = new SqlCommand("UpdateUserTime", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.NChar));
            command.Parameters[0].Value = login;
            command.ExecuteNonQuery();

            //получаем список подключенных пользователей (onLine)
            command = new SqlCommand("GetOnlineUsers", con);
            command.CommandType = CommandType.StoredProcedure;
            
            SqlDataReader DateReader = command.ExecuteReader();

            while (DateReader.Read())
            {
                model.Users.Add(new UserModel( (int)DateReader["UserID"], (string)DateReader["Name"], (DateTime)DateReader["LastUpdate"], (string)DateReader["Color"]));
                //UserID,Name,LastUpdate,Color
            }
            DateReader.Close();
            //----------------------------- закончили работу с user ------------------------------

            int LastMsgId=0 ;
            int.TryParse(Request["LastMsgId"],out LastMsgId);

            if (LastMsgId==0)
            {
                command = new SqlCommand("GetMessage", con);
                command.CommandType = CommandType.StoredProcedure;
                DateReader = command.ExecuteReader();
            }
            else
            {
                command = new SqlCommand("GetLastMessage", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.NChar));
                command.Parameters[0].Value = LastMsgId;
                DateReader = command.ExecuteReader();
            }

            while (DateReader.Read())
            {
                model.Messages.Add(new MessageModel((int)DateReader["MsgId"], (DateTime)DateReader["Time"], (string)DateReader["Name"], (string)DateReader["Color"], (string)DateReader["Mesage"]));
                //MsgId ,Time ,Name ,Color ,Mesage 
            }
            DateReader.Close();

            con.Close();

            return Json(model);

        }
    }
}