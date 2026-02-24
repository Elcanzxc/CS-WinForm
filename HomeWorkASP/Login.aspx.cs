using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeWorkASP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "admin" && txtPass.Text == "123")
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Login successful!";
            }
            else
            {
                lblMessage.Text = "Incorrect username or password";
            }
        }
    }
}