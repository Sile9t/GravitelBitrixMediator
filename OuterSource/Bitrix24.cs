﻿using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net;
using System.Text;
using RestSharp;
using System.Web;

namespace Entities
{
    public class Bitrix24
    {
        //Your application id (client_id)
        private const string BX_ClientID = "local.58f736938000000000000";
        //Your client secret (client_secret)
        private const string BX_ClientSecret = "N9NjwzOeh6jhgkhkjhjkhkjk6ZdrzcC4f2";
        //Your Bitrix URL
        private const string BX_Portal = "https://xxxx.bitrix24.ru";
        //OAuth address
        private const string BX_OAuthSite = "https://oauth.bitrix.info/oauth";
        //Change to your Username
        private const string Username = @"mail@mail.ru";
        //Change to your password
        private const string Password = @"sdfdFDSfsdf";

        //Private field for Auth
        private string AccessToken;
        private string RefreshToken;
        private DateTime RefreshTime;
        private string Code;
        private string Cookie;

        public Bitrix24() => Connect();

        //Connection method for configure OAuth and access 
        private void Connect()
        {
            const string bx_URI = "/authorize";
            
            RestClient client = new RestClient(BX_Portal);

            var request = new RestRequest(bx_URI, Method.Post);
            request.AddParameter("client_id", BX_ClientID);

            string svcCredentials = Convert.ToBase64String(
                ASCIIEncoding.ASCII.GetBytes(Username + ":" + Password));

            request.AddHeader("Authorization", "Basic " + svcCredentials);

            var response = client.Execute(request);

            if (response.StatusCode is HttpStatusCode.Found)
            {
                var locationURI = new Uri(response.GetHeaderValue("Location")!);

                var locationParams = HttpUtility.ParseQueryString(locationURI.Query);

                Cookie = response.GetHeaderValue("Set-Cookie")!;
                Code = locationParams["Code"]!;

                if (String.IsNullOrEmpty(Code))
                    throw new FormatException("CodeNotFound");
            }
            client.Dispose();

            SetToken();

        }

        private void SetToken(bool refresh = false)
        {
            var OAuth_Request_URI = "/token";

            using (var client = new RestClient(BX_OAuthSite))
            {
                var request = new RestRequest(OAuth_Request_URI, Method.Post);
                request.AddHeader("Cookie", Cookie);
                request.AddParameter("client_id", BX_ClientID);
                request.AddParameter("grant_type", refresh ? "refresh_token" : "authorization_code");
                request.AddParameter("client_secret", BX_ClientSecret);
                if (refresh && !String.IsNullOrEmpty(RefreshToken))
                    request.AddParameter("refresh_token", RefreshToken);
                request.AddParameter("code", Code);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new FormatException("ErrorLogonBitrixOAuth");

                var converter = new ExpandoObjectConverter();

                dynamic objLogonBitrixOAuth = JsonConvert
                    .DeserializeObject<ExpandoObject>(response.Content!, converter)!;

                AccessToken = objLogonBitrixOAuth.AccessToken;
                RefreshToken = objLogonBitrixOAuth.RefreshToken;
                RefreshTime = DateTime.Now.AddSeconds(objLogonBitrixOAuth.ExpiresIn);
            }
        }

        private void CheckTokensAndRefreshIfNeeded()
        {
            if (RefreshTime == DateTime.MinValue)
            {
                Connect();

                return;
            }

            if (RefreshTime.AddSeconds(-5) < DateTime.Now)
                SetToken(refresh: true);
        }

        public string SendCommand(string Command, string Params = "", string Body = "")
        {
            CheckTokensAndRefreshIfNeeded();

            using (var client = new RestClient(BX_Portal))
            {
                string restQuery = "/rest/" + Command + "?auth=" + AccessToken;
                if (!String.IsNullOrEmpty(Params))
                    restQuery += "&" + Params;

                var request = new RestRequest(restQuery, Method.Post);
                request.AddHeader("Cookie", Cookie);

                byte[] ByteArrayBody = Encoding.UTF8.GetBytes(Body);
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                request.AddHeader("ContentLength", ByteArrayBody.Length);
                request.AddBody(ByteArrayBody, "application/x-www-form-urlencoded");

                var response = client.Execute(request);

                return response.Content!;
            }
        }
    }
}