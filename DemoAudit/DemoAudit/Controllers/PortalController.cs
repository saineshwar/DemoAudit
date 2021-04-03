using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DemoAudit.Helpers;
using DemoAudit.Models;
using DemoAudit.Repository;
using Microsoft.AspNetCore.Http;

namespace DemoAudit.Controllers
{
    public class PortalController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditRepository _auditRepository;
        public PortalController(IHttpContextAccessor httpContextAccessor, IAuditRepository auditRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditRepository = auditRepository;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username =="demoaudit" && loginViewModel.Password == "demoaudit")
            {

                HttpContext.Session.SetInt32(AllSessionKeys.UserId, 1);
                HttpContext.Session.SetString(AllSessionKeys.UserName, "demoaudit");
                HttpContext.Session.SetString(AllSessionKeys.IsFirstLogin, "N");
                HttpContext.Session.SetInt32(AllSessionKeys.RoleId, 1);
                HttpContext.Session.SetInt32(AllSessionKeys.LangId, 1);
                AuditLogin();
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View(loginViewModel);
        }

        private void AuditLogin()
        {
            var objaudit = new AuditModel();
            objaudit.RoleId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            objaudit.ControllerName = "Portal";
            objaudit.ActionName = "Login";
            objaudit.Area = "";
            objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_httpContextAccessor.HttpContext != null)
                objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            objaudit.UserId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.UserId));
            objaudit.PageAccessed = "";
            objaudit.UrlReferrer = "";
            objaudit.SessionId = HttpContext.Session.Id;
            _auditRepository.InsertAuditLogs(objaudit);
        }

        private void AuditLogout()
        {
            var objaudit = new AuditModel();
            objaudit.RoleId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            objaudit.ControllerName = "Portal";
            objaudit.ActionName = "Logout";
            objaudit.Area = "";
            objaudit.LoggedOutAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_httpContextAccessor.HttpContext != null)
                objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            objaudit.UserId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.UserId));
            objaudit.PageAccessed = "";
            objaudit.UrlReferrer = "";
            objaudit.SessionId = HttpContext.Session.Id;
       
            _auditRepository.InsertAuditLogs(objaudit);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                AuditLogout();
                return RedirectToAction("Login", "Portal");
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
