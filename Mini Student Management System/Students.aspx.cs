using Mini_Student_Management_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mini_Student_Management_System
{
    public partial class Students : System.Web.UI.Page
    {
        string filePath = HttpContext.Current.Server.MapPath("~/Data/students.json");


        // Sessiya yoxdusa login olunmalidir
        protected void Page_Load(object sender, EventArgs e)
        {
         //  int x = 0; 
         // int y = 5 / x;

            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblUser.Text = "Xoş gəldiniz, " + Session["CurrentUser"].ToString();
                LoadStudents();
            }
        }

        // load data from json u know :)
        private void LoadStudents()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var students = JsonConvert.DeserializeObject<List<Student>>(json);
                gvStudents.DataSource = students;
                gvStudents.DataBind();
            }
        }

        // Yeni setir elave etmek
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var students = GetList();
        
            var existing = students.FirstOrDefault(s => s.ID == txtID.Text);
            if (existing != null)
            {
                existing.Name = txtName.Text;
                existing.Surname = txtSurname.Text;
            }
            else
            {
                students.Add(new Student { ID = txtID.Text, Name = txtName.Text, Surname = txtSurname.Text });
            }
            SaveAndRefresh(students);
        }


        // Silmek ve Edit etmek
        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var students = GetList();
            if (e.CommandName == "DeleteStudent")
            {
                string id = e.CommandArgument.ToString();
                students.RemoveAll(s => s.ID == id);
                SaveAndRefresh(students);
            }
            else if (e.CommandName == "EditStudent")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                txtID.Text = gvStudents.Rows[index].Cells[0].Text;
                txtName.Text = gvStudents.Rows[index].Cells[1].Text;
                txtSurname.Text = gvStudents.Rows[index].Cells[2].Text;
            }
        }


        // Convert edir Data qovluqdaki users.json 
        private List<Student> GetList()
        {
            try
            {
                if (!File.Exists(filePath)) return new List<Student>();

                string data = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Student>>(data) ?? new List<Student>();
            }
            catch (JsonException)
            {
               
                lblUser.Text = "Xəta: Məlumat formatı düzgün deyil!";
                return new List<Student>();
            }
            catch (Exception)
            {
                throw; 
            }
        }


        // Save duymesi
        private void SaveAndRefresh(List<Student> list)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(list, Formatting.Indented));
            LoadStudents();
          
            txtID.Text = txtName.Text = txtSurname.Text = "";
        }


    }
}