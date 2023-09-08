using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace WebAppTest.Models
{
    public class db
    {
        SqlConnection con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=test_mvc;Persist Security Info=True;User ID=admin;Password=12345678;Multiple Active Result Sets=True");
        
        public async Task<string> Register(User user)
        {
            SqlCommand com = new SqlCommand("User_Register", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserName", user.UserName);
            com.Parameters.AddWithValue("@FirstName", user.FirstName);
            com.Parameters.AddWithValue("@LastName", user.LastName);
            com.Parameters.AddWithValue("@Email", user.Email);

            string hash = String.Empty;
            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }
            com.Parameters.AddWithValue("@Password", hash);
            com.Parameters.Add("@res", SqlDbType.VarChar, 30);
            com.Parameters["@res"].Direction = ParameterDirection.Output;
            string res;
            try
            {
                con.Open();
                int i = com.ExecuteNonQuery();
                //Storing the output parameters value in 3 different variables.  
                res = Convert.ToString(com.Parameters["@res"].Value);
                // Here we get all three values from database in above three variables.  
            }
            catch (Exception ex)
            {
                // throw the exception  
                res = "error";
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public async Task<string> Login(User user) {
            SqlCommand com = new SqlCommand("User_Login", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserName", user.UserName);

            string hash = String.Empty;
            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }
            com.Parameters.AddWithValue("@Password", hash);

            com.Parameters.Add("@FirstName", SqlDbType.VarChar, 50);
            com.Parameters["@FirstName"].Direction = ParameterDirection.Output;

            com.Parameters.Add("@LastName", SqlDbType.VarChar, 50);
            com.Parameters["@LastName"].Direction = ParameterDirection.Output;

            com.Parameters.Add("@Email", SqlDbType.VarChar, 255);
            com.Parameters["@Email"].Direction = ParameterDirection.Output;

            com.Parameters.Add("@UserRole", SqlDbType.VarChar, 1);
            com.Parameters["@UserRole"].Direction = ParameterDirection.Output;

            com.Parameters.Add("@res", SqlDbType.VarChar, 30);
            com.Parameters["@res"].Direction = ParameterDirection.Output;

            string res;
            try
            {
                con.Open();
                int i = com.ExecuteNonQuery();
                //Storing the output parameters value in 3 different variables.  
                res = Convert.ToString(com.Parameters["@res"].Value);
                if (res.Equals("Login Completed")) { 
                    user.FirstName = Convert.ToString(com.Parameters["@FirstName"].Value);
                    user.LastName = Convert.ToString(com.Parameters["@LastName"].Value);
                    user.Email = Convert.ToString(com.Parameters["@Email"].Value);
                    user.UserRole = Convert.ToString(com.Parameters["@UserRole"].Value);
                }
                // Here we get all three values from database in above three variables.  
            }
            catch (Exception ex)
            {
                // throw the exception  
                res = "error";
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        //Article Create
        public async Task<string> InsertNewArticle(Article article)
        {
            string res;

            SqlCommand com = new SqlCommand("CreateArticle", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Subject", article.Subject);
            com.Parameters.AddWithValue("@Body", article.Body);

            com.Parameters.Add("@res", SqlDbType.VarChar, 30);
            com.Parameters["@res"].Direction = ParameterDirection.Output;


            try
            {
                con.Open();
                int i = com.ExecuteNonQuery();
                //Storing the output parameters value in 3 different variables.  
                res = Convert.ToString(com.Parameters["@res"].Value);
                // Here we get all three values from database in above three variables.  
            }
            catch (Exception ex)
            {
                // throw the exception  
                res = "error";
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public async Task<Article[]> GetAllArticle()
        {
            SqlCommand com = new SqlCommand("select * from article", con);
            List<Article> allarticle = new List<Article>();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Article buffer = new Article();
                    buffer.ID = (int)reader["idArticle"];
                    buffer.Subject = (string)reader["Subject"];
                    buffer.Body = (string)reader["Body"];
                    buffer.Time = (DateTime?)reader["TIMESTAMP"];
                    allarticle.Add(buffer);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
            
            return allarticle.ToArray();
        }

        public async Task<Article[]> GetAllArticleDesc()
        {
            SqlCommand com = new SqlCommand("select * from article order by TIMESTAMP desc", con);
            List<Article> allarticle = new List<Article>();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Article buffer = new Article();
                    buffer.ID = (int)reader["idArticle"];
                    buffer.Subject = (string)reader["Subject"];
                    buffer.Body = (string)reader["Body"];
                    buffer.Time = (DateTime?)reader["TIMESTAMP"];
                    allarticle.Add(buffer);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

            return allarticle.ToArray();
        }


        public async Task<Article> GetArticle(int? id)
        {
            if (id == null) return null;
            SqlCommand com = new SqlCommand("select * from article where idArticle = " + id.ToString(), con);
            Article article = new Article();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                article.ID = (int)reader["idArticle"];
                article.Subject = (string)reader["Subject"];
                article.Body = (string)reader["Body"];
                article.Time = (DateTime?)reader["TIMESTAMP"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                con.Close();
            }

            return article;
        }

        public async Task<Boolean> UpdateArticle(int? id,Article article)
        {
            if (id == null) return false;
            SqlCommand com = new SqlCommand("Update article set Subject = '"+article.Subject+"' , Body = '"+article.Body+"' where idArticle = "+id, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                con.Close();
            }

        }

        public async Task<Boolean> DeleteArticle(int? id)
        {
            if (id == null) return false;
            SqlCommand com = new SqlCommand("delete from article where idArticle = " + id, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                con.Close();
            }

        }

        //-------Banner-------
        public async Task<string> InsertNewBanner(BannerCreate banner) {

            if (isImage(banner.Image)) {
                string res;
                string newFileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "]" + banner.Image.FileName;
                string path = Path.Combine("/Uploads/Banners", Path.GetFileName(newFileName));

                try {
                    using (Stream fileStream = new FileStream("./wwwroot"+path, FileMode.Create))
                    {
                        await banner.Image.CopyToAsync(fileStream);
                    }
                }
                catch (Exception)
                {
                    return "Create Banner Failed [Error when try to save file]";
                }

                try {
                    SqlCommand com = new SqlCommand("CreateBanner", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Title", banner.Title);

                    if (banner.Description == null) banner.Description = "";
                    com.Parameters.AddWithValue("@Description", banner.Description);
                    com.Parameters.AddWithValue("@Path", path);

                    com.Parameters.Add("@res", SqlDbType.VarChar, 30);
                    com.Parameters["@res"].Direction = ParameterDirection.Output;

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@res"].Value);

                }
                catch (Exception)
                {
                    return "Create Banner Failed [Error when try to insert new record]";
                }
                finally
                {
                    con.Close();
                }
                return res;
            }
            else
            {
                return "Create Banner Failed [File is not image type]";
            }
        }

        public async Task<Banner[]> GetAllBanner()
        {
            SqlCommand com = new SqlCommand("select * from banner", con);
            List<Banner> allbanner = new List<Banner>();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Banner buffer = new Banner();
                    buffer.ID = (int)reader["idBanner"];
                    buffer.Title = (string)reader["Title"];
                    buffer.Description = (string)reader["Description"];
                    buffer.Path = (string)reader["Path"];
                    allbanner.Add(buffer);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

            return allbanner.ToArray();
        }

        public async Task<Banner> GetBanner(int? id)
        {
            if (id == null) return null;
            SqlCommand com = new SqlCommand("GetSingleBanner", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@idBanner", id);
            Banner banner = new Banner();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                banner.ID = (int)reader["idBanner"];
                banner.Title = (string)reader["Title"];
                banner.Description = (string)reader["Description"];
                banner.Path = (string)reader["Path"];

                List<SubPicture> list = new List<SubPicture>();
                reader.NextResult();
                while (reader.Read())
                {
                    SubPicture buffer = new SubPicture();
                    buffer.ID = (int)reader["idSubpicture"];
                    buffer.Path = (string)reader["Path"];
                    list.Add(buffer);
                }
                banner.SubPictures =list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                con.Close();
            }

            return banner;
        }

        public async Task<BannerEdit> GetBannerEdit(int? id)
        {
            if (id == null) return null;
            SqlCommand com = new SqlCommand("GetSingleBanner", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@idBanner", id);
            BannerEdit banner = new BannerEdit();
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                banner.ID = (int)reader["idBanner"];
                banner.Title = (string)reader["Title"];
                banner.Description = (string)reader["Description"];
                banner.Path = (string)reader["Path"];

                List<SubPicture> list = new List<SubPicture>();
                reader.NextResult();
                while (reader.Read())
                {
                    SubPicture buffer = new SubPicture();
                    buffer.ID = (int)reader["idSubpicture"];
                    buffer.Path = (string)reader["Path"];
                    list.Add(buffer);
                }
                banner.SubPictures = list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                con.Close();
            }

            return banner;
        }

        public async Task<Boolean> UpdateBanner(int? id, BannerEdit banner)
        {
            if (id == null) return false;
            if (banner.Image == null) {
                try
                {
                    SqlCommand com = new SqlCommand("Update banner set Title = '" + banner.Title + "' , Description = '" + banner.Description + "' where idBanner = " + id, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
            if (isImage(banner.Image))
            {
                string newFileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "]" + banner.Image.FileName;
                string path = Path.Combine("/Uploads/Banners", Path.GetFileName(newFileName));

                try
                {
                    using (Stream fileStream = new FileStream("./wwwroot" + path, FileMode.Create))
                    {
                        await banner.Image.CopyToAsync(fileStream);
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                try
                {
                    SqlCommand com = new SqlCommand("Update banner set Title = '" + banner.Title + "' , Description = '" + banner.Description + "' , Path = '"+path+"' where idBanner = " + id, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> DeleteBanner(int? id)
        {
            if (id == null) return false;
            SqlCommand com = new SqlCommand("DeleteSingleBanner", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@idBanner", id);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                con.Close();
            }

        }

        //----Sub Picture -------------

        public async Task<string> InsertNewSubPicture(int? idBanner, IFormFile? image) {
            if (isImage(image))
            {
                string res;
                string newFileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "]" + image.FileName;
                string path = Path.Combine("/Uploads/Pictures", Path.GetFileName(newFileName));

                try
                {
                    using (Stream fileStream = new FileStream("./wwwroot" + path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
                catch (Exception)
                {
                    return "Create Sub Picture Failed [Error when try to save file]";
                }

                try
                {
                    SqlCommand com = new SqlCommand("CreateSubPicture", con);
                    com.CommandType = CommandType.StoredProcedure;
                    
                    com.Parameters.AddWithValue("@Path", path);
                    com.Parameters.AddWithValue("@idBanner", idBanner);

                    com.Parameters.Add("@res", SqlDbType.VarChar, 30);
                    com.Parameters["@res"].Direction = ParameterDirection.Output;

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@res"].Value);

                }
                catch (Exception)
                {
                    return "Create Sub Picture Failed [Error when try to insert new record]";
                }
                finally
                {
                    con.Close();
                }
                return res;
            }
            else
            {
                return "Create Sub Picture Failed [File is not image type]";
            }
        }

        public async Task<Boolean> DeleteSubPicture(int? id)
        {
            if (id == null) return false;
            SqlCommand com = new SqlCommand("Delete from subpicture where idSubpicture =" + id, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                con.Close();
            }

        }

        //call data for home page---------
        public async Task<HomePageData> GetHomePageData()
        {
            SqlCommand com = new SqlCommand("GetHomePage", con);
            com.CommandType = CommandType.StoredProcedure;
            HomePageData homePageData = new HomePageData();

            List<Banner> listbanner = new List<Banner>();
            List<SubPicture> listsubpicture = new List<SubPicture>();
            List<Article> listarticle = new List<Article>();

            try {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                
                while (reader.Read())
                {
                    Banner buffer = new Banner();
                    buffer.ID = (int)reader["idBanner"];
                    buffer.Path = (string)reader["Path"];
                    listbanner.Add(buffer);
                }
                

                reader.NextResult();
                while (reader.Read())
                {
                    SubPicture buffer = new SubPicture();
                    buffer.idBanner = (int)reader["idBanner"];
                    buffer.Path = (string)reader["Path"];
                    listsubpicture.Add(buffer);
                }
                

                reader.NextResult();
                while (reader.Read())
                {
                    Article buffer = new Article();
                    buffer.ID = (int)reader["idArticle"];
                    buffer.Subject = (string)reader["Subject"];
                    buffer.Body = (string)reader["Body"];
                    buffer.Time = (DateTime?)reader["TIMESTAMP"];
                    listarticle.Add(buffer);
                }
                

                

            }
            catch (Exception ex)
            {
                
            }
            finally { con.Close(); }
            homePageData.banners = listbanner.ToArray();
            homePageData.subpicture = listsubpicture.ToArray();
            homePageData.articles = listarticle.ToArray();
            return homePageData;
        }


        //check is image

        private const int ImageMinimumBytes = 512;
        private bool isImage(IFormFile file) {
            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
            && Path.GetExtension(file.FileName).ToLower() != ".png"
            && Path.GetExtension(file.FileName).ToLower() != ".gif"
            && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (file.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(file.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                file.OpenReadStream().Position = 0;
            }

            return true;
        }

    }
}
