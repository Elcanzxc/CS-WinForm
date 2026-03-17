using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Book_Library
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UserId"] != null)
                Response.Redirect("Books.aspx");

            if (!IsPostBack)
            {
                
                if (Request.QueryString["registered"] == "1")
                {
                  
                    string lastUser = Request.Cookies["LastRegistered"]?.Value ?? "";
                    lblSuccess.Text = $"✅ Qeydiyyat uğurlu oldu! Xoş gəldiniz, {lastUser}. İndi daxil ola bilərsiniz.";
                    lblSuccess.Visible = true;
                }
            }
        }

       
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            User user = DatabaseHelper.LoginUser(username, password);

            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["Email"] = user.Email;

                Response.Redirect("Books.aspx");
            }
            else
            {
                lblError.Text = "❌ İstifadəçi adı və ya şifrə yanlışdır.";
                lblError.Visible = true;
            }
        }
    }
}
