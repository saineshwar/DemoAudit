using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DemoAudit.Models;
using Microsoft.Extensions.Configuration;

namespace DemoAudit.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IConfiguration _configuration;
        public AuditRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void InsertAuditLogs(AuditModel objauditmodel)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AuditDatabaseConnection"));
            try
            {
                con.Open();
                var para = new DynamicParameters();
                para.Add("@UserID", objauditmodel.UserId);
                para.Add("@SessionID", objauditmodel.SessionId);
                para.Add("@IPAddress", objauditmodel.IpAddress);
                para.Add("@PageAccessed", objauditmodel.PageAccessed);
                para.Add("@LoggedInAt", objauditmodel.LoggedInAt);
                para.Add("@LoggedOutAt", objauditmodel.LoggedOutAt);
                para.Add("@LoginStatus", objauditmodel.LoginStatus);
                para.Add("@ControllerName", objauditmodel.ControllerName);
                para.Add("@ActionName", objauditmodel.ActionName);
                para.Add("@UrlReferrer", objauditmodel.UrlReferrer);
                para.Add("@Area", objauditmodel.Area);
                para.Add("@RoleId", objauditmodel.RoleId);
                para.Add("@LangId", objauditmodel.LangId);
                para.Add("@IsFirstLogin", objauditmodel.IsFirstLogin);
                con.Execute("Usp_InsertAuditLogs", para, null, 0, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}