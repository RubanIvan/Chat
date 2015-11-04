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

            if(login ==null || passwd==null ) return RedirectToAction("Login","Authorization",new {id=1});

            SqlCommand command = new SqlCommand("Logon", DataBase.GetSqlConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.NChar));
            command.Parameters.Add(new SqlParameter("@Passwd", SqlDbType.NChar));
            command.Parameters[0].Value = login;
            command.Parameters[1].Value = passwd;
            SqlDataReader DateReader = command.ExecuteReader();
            DateReader.Read();

            if ((int)DateReader["Loged"] == 1)
            {
                // Создать объект cookie-набора
                HttpCookie cookie = new HttpCookie("ChatAutorize");

                // Установить значения в нем
                cookie["Authorize"] = 1.ToString();
                cookie["LoginName"] = login;

                // Добавить куки в ответ
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Chat");
            }
            else
            { return RedirectToAction("Login","Authorization", new { id = 1 });}

        }

    }
}