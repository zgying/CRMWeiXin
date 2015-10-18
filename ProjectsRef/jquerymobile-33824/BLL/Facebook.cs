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
 *  http://osnapz.wordpress.com/2010/04/23/using-asp-net-with-facebooks-graph-api-and-oauth-2-0-authentication/
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BLL
{

    public class Facebook
    {
        public enum Method { GET, POST };
        public const string AUTHORIZE = "https://graph.facebook.com/oauth/authorize";
        public const string SCOPE = "email,user_birthday,user_location,user_likes";
        public const string LOGOUT = "https://www.facebook.com/logout.php";
        public const string ACCESS_TOKEN = "https://graph.facebook.com/oauth/access_token";
        public string CALLBACK_URL = Utils.ReturnRootUrl() + "Account/Login.aspx";
        private string _consumerKey = "";
        private string _consumerSecret = "";
        private string _token = "";

        #region Properties

        public string ConsumerKey
        {
            get
            {
                if (_consumerKey.Length == 0)
                {
                    if (ConfigurationManager.AppSettings.Get("FacebookAppID") != null)
                    {
                        _consumerKey = ConfigurationManager.AppSettings.Get("FacebookAppID");
                    }
                    else
                    {
                        ElmahExtension.LogToElmah(new ApplicationException("No FacebookAppID in AppSettings file"));
                    }
                }
                return _consumerKey;
            }
            set { _consumerKey = value; }
        }

        public string ConsumerSecret
        {
            get
            {
                if (_consumerSecret.Length == 0)
                {
                    if (ConfigurationManager.AppSettings.Get("FacebookSecret") != null)
                    {
                        _consumerSecret = ConfigurationManager.AppSettings.Get("FacebookSecret"); 
                    }
                    else
                    {
                        ElmahExtension.LogToElmah(new ApplicationException("No FacebookSecret in AppSettings file"));
                    }
                }
                return _consumerSecret;
            }
            set { _consumerSecret = value; }
        }

        public string Token { get { return _token; } set { _token = value; } }

        /// <summary>
        /// Gets the value of the current Portal Id from the current Session
        /// </summary>
        public static int PortalID { get { object temp = HttpContext.Current.Session["PortalID"]; return temp == null ? 1 : (int)temp; } }
        #endregion

        /// <summary>
        /// Get the link to Facebook's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {

            string URL = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}", AUTHORIZE, this.ConsumerKey, CALLBACK_URL, SCOPE);

            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                URL = URL + "&display=touch"; //Add mobile login support
            }

            return URL;
        }

        /// <summary>
        /// Gets the Facebook logout link
        /// </summary>
        /// <param name="FBToken">The Facebook token</param>
        /// <returns>Returns a string</returns>
        public string LogOutLinkGet(string FBToken)
        {
            return string.Format("{0}?access_token={1}&next={2}", LOGOUT, FBToken, Utils.ReturnRootUrl());
        }

        /// <summary>
        /// Exchange the Facebook "code" for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token or "code" is supplied by Facebook's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken)
        {
            this.Token = authToken;
            string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}&code={4}",
            ACCESS_TOKEN, this.ConsumerKey, CALLBACK_URL, this.ConsumerSecret, authToken);

            string response = WebRequest(Method.GET, accessTokenUrl, String.Empty);

            if (response.Length > 0)
            {
                //Store the returned access_token
                NameValueCollection qs = HttpUtility.ParseQueryString(response);

                if (qs["access_token"] != null)
                {
                    this.Token = qs["access_token"];
                }
            }
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "[You user agent]";
            webRequest.Timeout = 20000;

            if (method == Method.POST)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                // By making use of the using block the stream and writer objects
                // will automatically call their Dispose methods once they leave scope
                using (var stream = webRequest.GetRequestStream())
                using (var requestWriter = new StreamWriter(stream))
                {
                    try
                    {
                        requestWriter.Write(postData);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return WebResponseGet(webRequest);
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            try
            {
                // By making use of the using block the stream and reader objects
                // will automatically call their Dispose methods once they leave scope
                using (var stream = webRequest.GetResponse().GetResponseStream())
                using (var responseReader = new StreamReader(stream))
                {
                    return responseReader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the Facebook App ID for the current portal
        /// </summary>
        /// <returns>Returns a string</returns>
        public static string GetFacebookAppID()
        {
            try
            {
                return Utils.DbContext.Portals.SingleOrDefault(p => p.PortalID == Facebook.PortalID).FacebookAppID;
            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the Facebook secret for the current Portal
        /// </summary>
        /// <returns>Returns a string</returns>
        public static string GetFacebookSecret()
        {
            return Utils.DbContext.Portals.SingleOrDefault(p => p.PortalID == Facebook.PortalID).FacebookSecret;
        }
    }
}
