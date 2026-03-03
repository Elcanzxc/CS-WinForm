using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Mini_Student_Management_System
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        // Global Exception catch imitasiyasi , xeta olanda ErorrPage sehivesine atir
        protected void Application_Error(object sender, EventArgs e)
        {
       
            Exception exc = Server.GetLastError();

          
            if (exc != null)
            {
              
                Response.Clear();

            
                string logPath = Server.MapPath("~/Data/error_log.txt");
                string message = $"\n[{DateTime.Now}] Error: {exc.Message}\nStackTrace: {exc.StackTrace}\n";
                System.IO.File.AppendAllText(logPath, message);


                Response.Redirect("~/ErrorPage.aspx");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}