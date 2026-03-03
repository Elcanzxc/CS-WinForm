using Mini_Student_Management_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace Mini_Student_Management_System
{
    public partial class Login : System.Web.UI.Page
    {
        string filePath = HttpContext.Current.Server.MapPath("~/Data/users.json");


        //Login olunduqu ve cookie accept etdiyini yoxlayir , sayti acanda
        protected void Page_Load(object sender, EventArgs e)
        {
          // int x = 0;
          //  int y = 5 / x;
           
            if (Session["CurrentUser"] != null)
            {
                Response.Redirect("Students.aspx");
            }

            
            if (!IsPostBack && Request.Cookies["UserAuth"] != null)
            {
                
                string savedUsername = Request.Cookies["UserAuth"]["Username"];

                
                Session["CurrentUser"] = savedUsername;

              
                Response.Redirect("Students.aspx");
            }
        }

        // Login Duymesine basildiqda
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text.Trim();
            string password = txtLoginPassword.Text.Trim();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var users = JsonConvert.DeserializeObject<List<User>>(jsonData);

              
                var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                   
                    Session["CurrentUser"] = user.Username;

                 
                    if (chkRememberMe.Checked)
                    {
                        HttpCookie userCookie = new HttpCookie("UserAuth");
                        userCookie["Username"] = user.Username;
                        userCookie.Expires = DateTime.Now.AddDays(7); 
                        Response.Cookies.Add(userCookie);
                    }

                  
                    Response.Redirect("Students.aspx");
                }
                else
                {
                    lblLoginMessage.Text = "Username və ya şifrə yanlışdır!";
                }
            }
            else
            {
                lblLoginMessage.Text = "Sistemdə heç bir istifadəçi tapılmadı.";
            }
        }
    }
}