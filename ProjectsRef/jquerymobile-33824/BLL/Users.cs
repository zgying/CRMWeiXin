/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  URLS
 *  http://developers.facebook.com/docs/reference/api/
 *  http://json.codeplex.com/documentation
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using DAL;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public static class Users
    {
        /// <summary>
        /// Gets the value of the Portal ID for the current session
        /// </summary>
        public static int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"]; 
                return temp == null ? 1 : (int)temp;
            }
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static IQueryable DataSource()
        {
            var query = from q in Utils.DbContext.Users
                        where q.PortalID == Users.PortalID
                        select new
                        {
                            EntityKey = q.UserID,
                            q.PortalID,
                            q.Email,
                            q.IsLockedOut,
                            q.FailedPasswordAttemptCount,
                            q.LastLoginDate,
                            q.CreateDate,
                            q.FacebookID,
                            q.FullName,
                            q.FirstName,
                            q.MiddleName,
                            q.LastName,
                            q.Gender,
                            q.Local,
                            q.Link,
                            q.TimeZone,
                            q.Birthday,
                            q.Picture,
                            q.LocationID,
                            q.Address,
                            q.City,
                            q.Region,
                            q.PostalCode,
                            q.Telephone,
                            q.AltPhone
                        };
            return query;
        }

        public static DataTable DataSourceDT()
        {

            var query = from q in Utils.DbContext.Users
                        where q.PortalID == Users.PortalID
                        select new
                        {
                            EntityKey = q.UserID,
                            q.PortalID,
                            q.Email,
                            q.IsLockedOut,
                            q.FailedPasswordAttemptCount,
                            q.LastLoginDate,
                            q.CreateDate,
                            q.FacebookID,
                            q.FullName,
                            q.FirstName,
                            q.MiddleName,
                            q.LastName,
                            q.Gender,
                            q.Local,
                            q.Link,
                            q.TimeZone,
                            q.Birthday,
                            q.Picture,
                            q.LocationID,
                            q.Address,
                            q.City,
                            q.Region,
                            q.PostalCode,
                            q.Telephone,
                            q.AltPhone
                        };

            return query.LINQToDataTable();
        }

        public static void Delete(int EntityKey)
        {
            //DAL.Role r = Utils.DbContext.Roles.Where(p => p.RoleID == EntityKey).FirstOrDefault();
            //Utils.DbContext.DeleteObject(r);
            //Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            //DAL.Role r = Utils.DbContext.Roles.Where(p => p.RoleID == EntityKey).FirstOrDefault();
            //r.PortalID = BLL.Users.PortalID;
            //r.RoleName = values["RoleName"].ToString();
            //r.Description = values["Description"].ToString();
            //Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {
            //DAL.Role r = new DAL.Role
            //{
            //    PortalID = BLL.Users.PortalID,
            //    RoleName = values["RoleName"].ToString(),
            //    Description = values["Description"].ToString(),
            //    CreateDate = DateTime.Now
            //};
            //Utils.DbContext.AddToRoles(r);
            //Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static bool IsEmailSyntaxValid(string emailToValidate)
        {
            try
            {
                var email = new System.Net.Mail.MailAddress(emailToValidate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int CreateAccount(string email, string password, bool isSuperUser = false)
        {
            bool IsValid = IsEmailSyntaxValid(email);
            string PasswordSalt = Crypto.GeneratePasswordSalt();
            string EncryptPassword = Crypto.Encrypt(password, PasswordSalt);

            if (!UserExists(email) & IsValid)
            {
                User U = new User
                    {
                        CreateDate = DateTime.Now,
                        Email = email,
                        Password = EncryptPassword,
                        PasswordSalt = PasswordSalt,
                        LastLoginDate = DateTime.Now,
                        FailedPasswordAttemptCount = 0,
                        IsLockedOut = false,
                        IsSuperUser = isSuperUser,
                        PortalID = Users.PortalID,
                    };
                Utils.DbContext.Users.AddObject(U);
                Utils.DbContext.SaveChanges();

                BLL.Roles roles = new Roles(Utils.DbContext);
                roles.AddUsersToRoles(1, U.UserID, 1);

                return U.UserID;
            }
            else
            {
                return 0;
            }
        }

        public static bool UserExists(string email)
        {
            return Utils.DbContext.Users.Any(Rl => Rl.Email == email & Rl.PortalID == Users.PortalID);
        }

        public static void DeleteAccount(int userid)
        {
            var U = Utils.DbContext.Users.FirstOrDefault(Usr => Usr.UserID == userid & Usr.PortalID == Users.PortalID);
            Utils.DbContext.Users.DeleteObject(U);
            Utils.DbContext.SaveChanges();
            
        }

        public static string GetPassword(int userid)
        {
            string Password = null;

            var U = Utils.DbContext.Users.FirstOrDefault(Usr => Usr.UserID == userid & Usr.PortalID == Users.PortalID);
            Password = Crypto.Decrypt(U.Password, U.PasswordSalt);            

            return Password;
        }

        public static byte SendPasswordEmail(string email)
        {
            byte ErrCode = 0; //Default no error
            string Password = null;
            bool EmailSent = false;

            var U = Utils.DbContext.Users.FirstOrDefault(Usr => Usr.Email == email & Usr.PortalID == Users.PortalID);
            if (U != null)
            {
                Password = Crypto.Decrypt(U.Password, U.PasswordSalt);
                EmailSent = Messages.SendMail(email, "Password", "Your password is: " + Password, false);
                if (EmailSent == false)
                {
                    ErrCode = 1; //Issue with the mail server
                }
            }
            else
            {
                ErrCode = 2; //User does not exsists
            }

            return ErrCode;
        }

        public static bool Login(string email, string password, bool persistCookie = false)
        {
            int UserID = ValidateUser(email, password);

            if (UserID != 0)
            {
                BLL.Roles roles = new Roles(Utils.DbContext);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, email, DateTime.Now, DateTime.Now.AddMinutes(60), false, roles.GetRoles(UserID));
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Current.Response.Cookies.Add(authCookie);
                string ReturnURL = FormsAuthentication.GetRedirectUrl(email, false);
                HttpContext.Current.Response.Redirect(ReturnURL.Replace("?", ""));
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int ValidateUser(string email, string password)
        {
            try
            {
                string Password = null;
                int UserID;

                var U = Utils.DbContext.Users.FirstOrDefault(Usr => (Usr.Email == email & Usr.PortalID == Users.PortalID) || (Usr.Email == email & Usr.IsSuperUser == true));

                if (U != null)
                {
                    UserID = U.UserID;
                    Password = Crypto.Decrypt(U.Password, U.PasswordSalt);
                    if (password == Password)
                    {
                        U.FailedPasswordAttemptCount = 0;
                        U.LastLoginDate = DateTime.Now;
                        Utils.DbContext.SaveChanges();

                        HttpContext.Current.Session["UserID"] = U.UserID;
                        HttpContext.Current.Session["Email"] = U.Email;
                        HttpContext.Current.Session["IsSuperUser"] = U.IsSuperUser;
                        return UserID;
                    }
                    else
                    {
                        U.FailedPasswordAttemptCount = U.FailedPasswordAttemptCount + 1;
                        Utils.DbContext.SaveChanges();
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
                return 0;
            }
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
        }

        public static void GraphUser()
        {
            string url = "";
            Facebook oAuth = new Facebook();

            if (HttpContext.Current.Request["code"] != null)
            {
                //Get the access token and secret.
                oAuth.AccessTokenGet(HttpContext.Current.Request["code"]);

                if (oAuth.Token.Length > 0)
                {

                    HttpContext.Current.Session["FBToken"] = oAuth.Token.ToString();
                    url = "https://graph.facebook.com/me?fields=" + FBQuery() + "&access_token=" + oAuth.Token;
                    var data = oAuth.WebRequest(Facebook.Method.GET, url, String.Empty);
                    JObject o = JObject.Parse(data);

                    string _Email = null;
                    string _FacebookID = null;
                    string _FullName = null;
                    string _FirstName = null;
                    string _MiddleName = null;
                    string _LastName = null;
                    string _Gender = null;
                    string _Local = null;
                    string _Link = null;
                    string _TimeZone = null;
                    string _Birthday = null;
                    string _Picture = null;
                    string _LocationID = null;
                    string _LocationName = null;
                    string _City = null;
                    string _Region = null;

                    if (o.SelectToken("email") != null)
                    {
                        _Email = o.SelectToken("email").ToString();
                    }

                    if (o.SelectToken("id") != null)
                    {
                        _FacebookID = o.SelectToken("id").ToString();
                    }

                    if (o.SelectToken("name") != null)
                    {
                        _FullName = o.SelectToken("name").ToString();
                    }

                    if (o.SelectToken("first_name") != null)
                    {
                        _FirstName = o.SelectToken("first_name").ToString();
                    }

                    if (o.SelectToken("middle_name") != null)
                    {
                        _MiddleName = o.SelectToken("middle_name").ToString();
                    }

                    if (o.SelectToken("last_name") != null)
                    {
                        _LastName = o.SelectToken("last_name").ToString();
                    }

                    if (o.SelectToken("gender") != null)
                    {
                        _Gender = o.SelectToken("gender").ToString();
                    }

                    if (o.SelectToken("locale") != null)
                    {
                        _Local = o.SelectToken("locale").ToString();
                    }

                    if (o.SelectToken("link") != null)
                    {
                        _Link = o.SelectToken("link").ToString();
                    }

                    if (o.SelectToken("timezone") != null)
                    {
                        _TimeZone = o.SelectToken("timezone").ToString();
                    }

                    if (o.SelectToken("birthday") != null)
                    {
                        _Birthday = o.SelectToken("birthday").ToString();
                    }

                    if (o.SelectToken("picture") != null)
                    {
                        _Picture = o.SelectToken("picture").ToString();
                    }

                    if (o.SelectToken("location.id") != null)
                    {
                        _LocationID = o.SelectToken("location.id").ToString();
                    }

                    if (o.SelectToken("location.name") != null)
                    {
                        _LocationName = o.SelectToken("location.name").ToString();
                    }

                    if (_LocationName != null)
                    {
                        string[] s = _LocationName.Split(',');
                        _City = s[0];
                        _Region = s[1];
                    }

                    if (_Email != null)
                    {

                        if (UserExists(_Email) == false)
                        {
                            CreateAccount(_Email, Crypto.GenerateSimplePassword()); //Create the facebook user and asign a simple password
                        }

                        if (UserExists(_Email) == true)
                        {
                            var U = Utils.DbContext.Users.FirstOrDefault(Usr => Usr.Email == _Email);
                            U.FacebookID = _FacebookID;
                            U.FullName = _FullName;
                            U.FirstName = _FirstName;
                            U.MiddleName = _MiddleName;
                            U.LastName = _LastName;
                            U.Gender = _Gender;
                            U.Local = _Local;
                            U.Link = _Link;
                            U.TimeZone = int.Parse(_TimeZone);
                            U.Birthday = DateTime.Parse(_Birthday);
                            U.Picture = _Picture;
                            U.LocationID = _LocationID;
                            U.City = _City;
                            U.Region = _Region;
                            U.IsLockedOut = false;
                            U.IsSuperUser = false;
                            Utils.DbContext.SaveChanges();

                            if ((BLL.Users.Login(_Email, GetPassword(U.UserID)) != false))
                            {
                                HttpContext.Current.Response.Redirect("/Default.aspx");
                            }
                        }
                    }
                }
            }

            string FBError = HttpContext.Current.Request.QueryString["error_reason"];

            if (FBError == "user_denied") //User canceled go back to login
            {
                HttpContext.Current.Response.Redirect("/Account/Login.aspx");
            }
        }

        public static string FBQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("id,");
            sb.Append("name,");
            sb.Append("first_name,");
            sb.Append("middle_name,");
            sb.Append("last_name,");
            sb.Append("gender,");
            sb.Append("locale,");
            sb.Append("link,");
            sb.Append("username,");
            sb.Append("timezone,");
            sb.Append("birthday,");
            sb.Append("email,");
            sb.Append("picture,");
            sb.Append("location");
            return sb.ToString();
        }

        public static void ExportUsers()
        {
            //var query = (from q in Utils.DbContext.Users select new {q.UserID, q.Email}).ToArray();
            var query = (from q in Utils.DbContext.Users select q).ToArray();
            System.IO.File.WriteAllText(@"C:\codeplex\jQueryMobile\jQueryMobile\Job\SampleData\Users.csv", Utils.GenerateCsv(query));
        }

        public static void ImportUsers()
        {
            string[] csvlines = File.ReadAllLines(@"C:\codeplex\jQueryMobile\jQueryMobile\Job\SampleData\Users.csv");

            var query = from csvline in csvlines
                        let data = csvline.Split(',')
                        select new
                        {
                            PortalID = data[0],
                            Email = data[1],
                            Password = data[2],
                            FacebookID = data[3],
                            FullName = data[4],
                            FirstName = data[5],
                            MiddleName = data[6],
                            LastName = data[7],
                            Gender = data[8],
                            Local = data[9],
                            Link = data[10],
                            _TimeZone = data[11],
                            Birthdate = data[12],
                            Picture = data[13],
                            Telephone = data[14],
                            LocationID = data[15],
                            Address = data[16],
                            City = data[17],
                            Region = data[18],
                            PostalCode = data[19]
                        };

            int row = 0;

            foreach (var i in query)
            {
                if (row != 0) // skip first row header name
                {
                    string passwordsalt = Crypto.GeneratePasswordSalt();

                    User U = new User
                    {
                        PortalID = int.Parse(i.PortalID),
                        Email = i.Email,
                        Password = Crypto.Encrypt("Password", passwordsalt),
                        PasswordSalt = passwordsalt,
                        FacebookID = i.FacebookID,
                        FullName = i.FullName,
                        FirstName = i.FirstName,
                        MiddleName = i.MiddleName,
                        LastName = i.LastName,
                        Gender = i.Gender,
                        Local = i.Local,
                        Link = i.Link,
                        TimeZone = int.Parse(i._TimeZone),
                        Birthday = DateTime.Parse(i.Birthdate),
                        Picture = i.Picture,
                        Telephone = i.Telephone,
                        LocationID = i.LocationID,
                        Address = i.Address,
                        City = i.City,
                        Region = i.Region,
                        PostalCode = i.PostalCode,
                        CreateDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        FailedPasswordAttemptCount = 0,
                        IsLockedOut = false,
                        IsSuperUser = false
                    };

                    Utils.DbContext.Users.AddObject(U);
                    Utils.DbContext.SaveChanges();

                    BLL.Roles roles = new Roles(Utils.DbContext);
                    roles.AddUsersToRoles(1, U.UserID, 1);
                }
            }
                row++;
        }
    }
}