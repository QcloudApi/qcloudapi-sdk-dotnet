using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace QCloudAPI_SDK.Module
{
    class Cdn : Base
    {
        public Cdn()
        {
            serverHost = "cdn.api.qcloud.com";
        }

        public string UploadCdnEntity(SortedDictionary<string, object> requestParams)
        {
            string actionName = "UploadCdnEntity";

            string entityFile = requestParams["entityFile"].ToString();
            requestParams.Remove("entityFile");
            var file = new FileInfo(entityFile);
            if (!file.Exists)
            {
                throw new FileNotFoundException();
            }

            if (!requestParams.ContainsKey("entityFileMd5"))
            {
                FileStream md5_file = new FileStream(entityFile, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(md5_file);
                md5_file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                requestParams["entityFileMd5"] = sb.ToString();
            }

            return Call(actionName, requestParams, entityFile);
        }
    }
}
