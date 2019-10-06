
using System;
using System.IO;
using System.Net;
using System.Text;


namespace FHIRDataManager.Utility
{
    public class AuthorizationParam
    {
        /// <summary>
        /// NONE-不使用验证 Base-Base验证
        /// </summary>
        public string AuthorizationType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            switch (AuthorizationType.ToUpper())
            {
                case "BASE":
                    String userPassword = string.Format("{0}:{1}", UserName, Password);
                    var authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPassword));
                    return "Basic " + authorization;
                default:
                    return base.ToString();
            }
        }
    }
    public class HttpHelper
    {
        static internal String HttpPost(String url, String contentType, String postData, AuthorizationParam authorization = null)
        {
            string responseData = string.Empty;
            try
            {
                Console.WriteLine("HTTP-POST:" + url);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData); //转化
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                request.Credentials = CredentialCache.DefaultCredentials;
                if (authorization != null)
                    request.Headers.Add("Authorization", authorization.ToString());
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                    throw new Exception("POST请求失败HTTP-STATUS:" + response.StatusCode.ToString() + "错误信息：" + responseData);
                }
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nurl:" + url + "\r\nData:" + postData);
            }
            return responseData;
        }
        static internal String HttpPut(String url, String contentType, String postData, AuthorizationParam authorization = null)
        {
            string responseData = string.Empty;
            try
            {
                Console.WriteLine("HTTP-PUT:" + url);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData); //转化
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "PUT";
                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                request.Credentials = CredentialCache.DefaultCredentials;
                if (authorization != null)
                    request.Headers.Add("Authorization", authorization.ToString());
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                    throw new Exception("PUT请求失败HTTP-STATUS:" + response.StatusCode.ToString() + "错误信息：" + responseData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nurl:" + url + "\r\nData:" + postData);
            }
            return responseData;
        }
        static internal String HttpGet(string url, String contentType, AuthorizationParam authorization = null)
        {
            string responseData = string.Empty;
            try
            {
                Console.WriteLine("HTTP-GET:" + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "GET";
                request.ContentType = contentType;
                request.Credentials = CredentialCache.DefaultCredentials;
                if (authorization != null)
                    request.Headers.Add("Authorization", authorization.ToString());
                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                    throw new Exception("GET请求失败HTTP-STATUS:" + response.StatusCode.ToString() + "错误信息：" + responseData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nurl:" + url);
            }
            return responseData;
        }

        static internal String HttpDelete(string url, String contentType, AuthorizationParam authorization = null)
        {
            string responseData = string.Empty;
            try
            {
                Console.WriteLine("HTTP-DELETE:" + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "DELETE";
                request.ContentType = contentType;
                request.Credentials = CredentialCache.DefaultCredentials;
                if (authorization != null)
                    request.Headers.Add("Authorization", authorization.ToString());
                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                    throw new Exception("DELETE请求失败HTTP-STATUS:" + response.StatusCode.ToString() + "错误信息：" + responseData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\nurl:" + url);
            }
            return responseData;
        }
    }
}