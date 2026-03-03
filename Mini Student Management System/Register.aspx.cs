using Mini_Student_Management_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace Mini_Student_Management_System
{
    public partial class Register : System.Web.UI.Page
    {
        string filePath = HttpContext.Current.Server.MapPath("~/Data/users.json");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Registrasiya edir
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();

        
            List<User> users = new List<User>();
            if (File.Exists(filePath))
            {
                string existingData = File.ReadAllText(filePath);
                users = JsonConvert.DeserializeObject<List<User>>(existingData) ?? new List<User>();
            }

          
            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                lblMessage.Text = "Bu username artıq mövcuddur!";
                return;
            }

          
            users.Add(new User { Username = username, Password = password, Email = email });

           
            string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Qeydiyyat uğurla tamamlandı! İndi Login ola bilərsiniz.";

        
            txtUsername.Text = "";
            txtEmail.Text = "";
        }
    }
}