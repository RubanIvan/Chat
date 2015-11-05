using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chat.Models;

namespace Chat.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet]
        public ActionResult Login(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Logon()
        {
            string login = Request["UserName"];
            string passwd = Request["UserPasswd"];
            string color = Request["rad"] ?? "#000000";
            if (login ==null || passwd==null ) return RedirectToAction("Login","Authorization",new {id=1});

            //дергаем хранимку
            SqlConnection con = DataBase.GetSqlConnection();
            SqlCommand command = new SqlCommand("Logon", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.NChar));
            command.Parameters.Add(new SqlParameter("@Passwd", SqlDbType.NChar));
            command.Parameters.Add(new SqlParameter("@Color", SqlDbType.NChar));
            command.Parameters[0].Value = login;
            command.Parameters[1].Value = passwd;
            command.Parameters[2].Value = color;

            SqlDataReader DateReader = command.ExecuteReader();
            DateReader.Read();
            
            //если пользователь есть или он новый авторизуем
            if ((int) DateReader["Loged"] == 1)
            {
                DateReader.Close();
                con.Close();

                Response.Cookies.Add(new HttpCookie("ChatAutorize", "1"));
                Response.Cookies.Add(new HttpCookie("LoginName", login));
                Response.Cookies.Add(new HttpCookie("Color", color));

                
                return RedirectToAction("Index", "Chat");
            }
            else //пароль не совпал выдаем сообщение
            {
                DateReader.Close();
                con.Close();
                return RedirectToAction("Login","Authorization", new { id = 1 });
            }

        }

    }
}