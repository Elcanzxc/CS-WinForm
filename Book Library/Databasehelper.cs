using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace Book_Library
{
    // -------------------------------------------------------
    //  Book model
    // -------------------------------------------------------
    public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
}

// -------------------------------------------------------
//  User model
// -------------------------------------------------------
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}

// -------------------------------------------------------
//  DatabaseHelper  –  bütün CRUD əməliyyatları
// -------------------------------------------------------
public static class DatabaseHelper
{
    // Bağlantı sətri Web.config-dən oxunur
    private static string ConnectionString =>
        ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

    /* ============================================================
       USER əməliyyatları
       ============================================================ */

    /// <summary>Yeni istifadəçi qeydiyyatı</summary>
    /// <returns>true – uğurlu; false – username artıq mövcuddur</returns>
    public static bool RegisterUser(string username, string password, string email)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();

            // Eyni username yoxlaması
            var checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
            checkCmd.Parameters.AddWithValue("@Username", username);
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0) return false;   // artıq mövcuddur

            // Yeni istifadəçi əlavə et
            var insertCmd = new SqlCommand(
                @"INSERT INTO Users (Username, Password, Email)
                      VALUES (@Username, @Password, @Email)", conn);
            insertCmd.Parameters.AddWithValue("@Username", username);
            insertCmd.Parameters.AddWithValue("@Password", password);  // sadəlik üçün plain-text
            insertCmd.Parameters.AddWithValue("@Email", email);
            insertCmd.ExecuteNonQuery();
            return true;
        }
    }

    /// <summary>Login yoxlaması</summary>
    /// <returns>User obyekti uğurlu girişdə, əks halda null</returns>
    public static User LoginUser(string username, string password)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                @"SELECT Id, Username, Email
                      FROM Users
                      WHERE Username = @Username AND Password = @Password", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }
            }
        }
        return null;
    }

    /* ============================================================
       BOOK əməliyyatları
       ============================================================ */

    /// <summary>Bütün kitabları gətir</summary>
    public static List<Book> GetAllBooks()
    {
        var books = new List<Book>();
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT Id, Title, Author, Year, Genre FROM Books ORDER BY Id DESC", conn);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = (int)reader["Year"],
                        Genre = reader["Genre"].ToString()
                    });
                }
            }
        }
        return books;
    }

    /// <summary>ID-yə görə tək kitab gətir</summary>
    public static Book GetBookById(int id)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT Id, Title, Author, Year, Genre FROM Books WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Book
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = (int)reader["Year"],
                        Genre = reader["Genre"].ToString()
                    };
                }
            }
        }
        return null;
    }

    /// <summary>Yeni kitab əlavə et</summary>
    public static void AddBook(Book book, int userId)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                @"INSERT INTO Books (Title, Author, Year, Genre, AddedBy)
                      VALUES (@Title, @Author, @Year, @Genre, @AddedBy)", conn);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Year", book.Year);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@AddedBy", userId);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Kitab məlumatlarını yenilə</summary>
    public static void UpdateBook(Book book)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                @"UPDATE Books
                      SET Title  = @Title,
                          Author = @Author,
                          Year   = @Year,
                          Genre  = @Genre
                      WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Year", book.Year);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Id", book.Id);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Kitabı sil</summary>
    public static void DeleteBook(int id)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM Books WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
}