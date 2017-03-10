using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Web;

namespace QCloudAPI_SDK.Common
{
    /// <summary>
    /// 请求调用类
    /// </summary>
    class Request
    {
        private static string VERSION = "SDK_DOTNET_1.1";
        private static int timeOut = 10000;//设置连接超时时间，默认10秒，用户可以根据具体需求适当更改timeOut的值

        public static void GetParams(SortedDictionary<string, object> requestParams, string secretId, string secretKey, 
            string requestMethod, string requestHost, string requestPath)
        {
            requestParams.Add("SecretId", secretId);
            Random rand = new Random();
            requestParams.Add("Nonce", rand.Next(Int32.MaxValue).ToString());
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            requestParams.Add("Timestamp", (unixTime / 1000).ToString());
            requestParams.Add("RequestClient", VERSION);
            string plainText = Sign.MakeSignPlainText(requestParams, requestMethod, requestHost, requestPath);
            string SignatureMethod = "HmacSHA1";
            if (requestParams.ContainsKey("SignatureMethod") && requestParams["SignatureMethod"] == "HmacSHA256")
            {
                SignatureMethod = "HmacSHA256";
            }
            string sign = Sign.Signature(plainText, secretKey, SignatureMethod);
            requestParams.Add("Signature", sign);
        }

        public static string GenerateUrl(SortedDictionary<string, object> requestParams, string secretId, string secretKey, 
            string requestMethod, string requestHost, string requestPath)
        {
            GetParams(requestParams, secretId, secretKey, requestMethod, requestHost, requestPath);
            string url =  "https://" + requestHost + requestPath;
            if (requestMethod == "GET")
            {
                url += "?" + BuildQuery(requestParams);
            }
            return url;
        }

        public static string BuildQuery(SortedDictionary<string, object> requestParams)
        {
            string paramStr = "";
            foreach (string key in requestParams.Keys)
            {
                paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(requestParams[key].ToString()));
            }
            paramStr = paramStr.TrimEnd('&');
            return paramStr;
        }

        public static string Send(SortedDictionary<string,object> requestParams, string secretId, string secretKey, string requestMethod, String requestHost, String requestPath, String fileName)
        {
            if (!requestParams.ContainsKey("SecretId"))
            {
                requestParams["SecretId"] = secretId;
            }
            if(!requestParams.ContainsKey("Nonce"))
            {
                requestParams["Nonce"] = new Random().Next(Int32.MaxValue);
            }
            if (!requestParams.ContainsKey("Timestamp"))
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                DateTime nowTime = DateTime.Now;
                long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
                requestParams.Add("Timestamp", (unixTime / 1000).ToString());
            }
            requestParams["RequestClient"] = VERSION;
            String plainText = Sign.MakeSignPlainText(requestParams, requestMethod, requestHost, requestPath);
            string SignatureMethod = "HmacSHA1";
            if (requestParams.ContainsKey("SignatureMethod") && requestParams["SignatureMethod"]=="HmacSHA256")
            {
                SignatureMethod = "HmacSHA256";
            }
            requestParams["Signature"] = Sign.Signature(plainText, secretKey, SignatureMethod);
            string url = "https://" + requestHost + requestPath;
            return SendRequest(url, requestParams, requestMethod, fileName);
        }

        public static string SendRequest(string url, SortedDictionary<string, object> requestParams, string requestMethod, string fileName)
        {
            if (requestMethod == "GET")
            {
                var paramStr = "";
                foreach (var key in requestParams.Keys)
                {
                    paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(requestParams[key].ToString())); 
                }
                paramStr = paramStr.TrimEnd('&');
                url += (url.EndsWith("?") ? "&" : "?") + paramStr;
            }

            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Accept = "*/*";
            request.KeepAlive = true;
            request.Timeout = timeOut;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)";
            if (requestMethod == "POST")
            {
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x"); 
                var beginBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n"); 
                var endBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n"); 
                request.Method = requestMethod;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                var memStream = new MemoryStream();

                var strBuf = new StringBuilder();
                foreach(var key in requestParams.Keys)
                {
                    strBuf.Append("\r\n--" + boundary + "\r\n");
                    strBuf.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                    strBuf.Append(requestParams[key].ToString());
                }
                var paramsByte = Encoding.GetEncoding("utf-8").GetBytes(strBuf.ToString());
                memStream.Write(paramsByte, 0, paramsByte.Length);

                if (fileName != null)
                {
                    memStream.Write(beginBoundary, 0, beginBoundary.Length);
                    var fileInfo = new FileInfo(fileName);
                    var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read); 
             
                    const string filePartHeader = 
                        "Content-Disposition: form-data; name=\"entityFile\"; filename=\"{0}\"\r\n" +
                        "Content-Type: application/octet-stream\r\n\r\n";
                    var header = string.Format(filePartHeader, fileInfo.Name);
                    var headerbytes = Encoding.UTF8.GetBytes(header);
                    memStream.Write(headerbytes, 0, headerbytes.Length);

                    var buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }  
                }
                memStream.Write(endBoundary, 0, endBoundary.Length);
                request.ContentLength = memStream.Length;

                var requestStream = request.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();  
            }

            var response = request.GetResponse();
            using (var s = response.GetResponseStream())
            {
                var reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}
