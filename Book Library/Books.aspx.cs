using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Book_Library
{
    public partial class Books : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

        
            lblUsername.Text = Session["Username"].ToString();

        
            if (!IsPostBack)
                LoadBooks();
        }

      
        private void LoadBooks()
        {
            gvBooks.DataSource = DatabaseHelper.GetAllBooks();
            gvBooks.DataBind();
        }

  
        protected void gvBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bookId = Convert.ToInt32(e.CommandArgument);

           
            if (e.CommandName == "DeleteBook")
            {
                DatabaseHelper.DeleteBook(bookId);
                ShowMessage("✅ Kitab uğurla silindi.", "msg-success");
                LoadBooks();
            }

            if (e.CommandName == "EditBook")
            {
                Book book = DatabaseHelper.GetBookById(bookId);
                if (book != null)
                {
                    hfBookId.Value = book.Id.ToString();
                    txtTitle.Text = book.Title;
                    txtAuthor.Text = book.Author;
                    txtYear.Text = book.Year.ToString();
                    ddlGenre.SelectedValue = book.Genre;

                    lblFormTitle.Text = "✏️ Kitabı Dəyiş";
                    btnSave.Text = "💾 Yenilə";
                    btnCancel.Visible = true;
                }
            }
        }

      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var book = new Book
            {
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                Year = Convert.ToInt32(txtYear.Text.Trim()),
                Genre = ddlGenre.SelectedValue
            };

            int bookId = Convert.ToInt32(hfBookId.Value);

            if (bookId == 0)
            {
             
                int userId = Convert.ToInt32(Session["UserId"]);
                DatabaseHelper.AddBook(book, userId);
                ShowMessage("✅ Kitab uğurla əlavə edildi.", "msg-success");
            }
            else
            {
               
                book.Id = bookId;
                DatabaseHelper.UpdateBook(book);
                ShowMessage("✅ Kitab uğurla yeniləndi.", "msg-success");
            }

            ResetForm();
            LoadBooks();
        }

 
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadBooks();
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
          
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }


        private void ResetForm()
        {
            hfBookId.Value = "0";
            txtTitle.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtYear.Text = string.Empty;
            ddlGenre.SelectedIndex = 0;

            lblFormTitle.Text = "➕ Yeni Kitab Əlavə Et";
            btnSave.Text = "💾 Saxla";
            btnCancel.Visible = false;
        }

        private void ShowMessage(string text, string cssClass)
        {
            lblMessage.Text = text;
            lblMessage.CssClass = "msg " + cssClass;
            lblMessage.Visible = true;
        }
    }
}