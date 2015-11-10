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
    public class OtherController : Controller
    {
        // GET: Other
        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Stat()
        {
            //Пользователь должен быть авторизован
            if (Request.Cookies["ChatAutorize"] == null || Request.Cookies["ChatAutorize"].Value != "1") return null;
            string login = Request.Cookies["LoginName"].Value;
            if (login == null) return null;

            using (SqlConnection con = DataBase.GetSqlConnection())
            {
                string cmd =$"SELECT COUNT(M.UserId) as CountMsg FROM Messages M JOIN Users U ON M.UserId=U.UserID WHERE Name='{login}' GROUP BY M.UserId";

                SqlCommand command = new SqlCommand(cmd, con);
                SqlDataReader DateReader = command.ExecuteReader();

                while (DateReader.Read())
                {
                    ViewBag.MsgCount = (int) DateReader["CountMsg"];
                }

                DateReader.Close();
            }
            ViewBag.UserName = login;
            return View();
        }
    }
}