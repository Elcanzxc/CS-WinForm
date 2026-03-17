using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Book_Library
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
   
            if (Session["UserId"] != null)
                Response.Redirect("Books.aspx");
        }

     
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;         
            string email = txtEmail.Text.Trim();

            bool success = DatabaseHelper.RegisterUser(username, password, email);

            if (success)
            {
                var cookie = new System.Web.HttpCookie("LastRegistered", username);
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cookie);

                Response.Redirect("Login.aspx?registered=1");
            }
            else
            {
                lblMessage.Text = "⚠️ Bu istifadəçi adı artıq mövcuddur. Başqa ad seçin.";
                lblMessage.CssClass = "msg msg-error";
                lblMessage.Visible = true;
            }
        }
    }
}