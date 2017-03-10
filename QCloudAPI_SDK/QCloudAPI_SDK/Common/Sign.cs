using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace QCloudAPI_SDK.Common
{
    /// <summary>
    /// 签名类
    /// </summary>
    class Sign
    {
        ///<summary>生成签名</summary>
        ///<param name="signStr">被加密串</param>
        ///<param name="secret">加密密钥</param>
        ///<returns>签名</returns>
        public static string Signature(string signStr, string secret)
        {
            if (SignatureMethod == "HmacSHA256")
                using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
                {
                    byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(signStr));
                    return Convert.ToBase64String(hash);
                }
            else
                using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(secret)))
                {
                    byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(signStr));
                    return Convert.ToBase64String(hash);
                }
        }

        protected static string BuildParamStr(SortedDictionary<string, object> requestParams, string requestMethod = "GET")
        {
            string retStr = "";
            foreach (string key in requestParams.Keys)
            {
                if (key == "Signature")
                {
                    continue;
                }
                if (requestMethod == "POST" && requestParams[key].ToString().Substring(0, 1).Equals("@"))
                {
                    continue;
                }
                retStr += string.Format("{0}={1}&", key.Replace("_", "."), requestParams[key]);
            }
            return "?" + retStr.TrimEnd('&');
        }

        public static string MakeSignPlainText(SortedDictionary<string, object> requestParams, string requestMethod = "GET", 
            string requestHost = "cvm.api.qcloud.com", string requestPath = "/v2/index.php")
        {
            string retStr = "";
            retStr += requestMethod;
            retStr += requestHost;
            retStr += requestPath;
            retStr += BuildParamStr(requestParams);
            return retStr;
        }
    }
}
