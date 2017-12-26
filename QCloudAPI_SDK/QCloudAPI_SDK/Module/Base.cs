using System;
using System.Collections.Generic;
using System.Text;

using QCloudAPI_SDK.Common;

namespace QCloudAPI_SDK.Module
{
    public abstract class Base
    {
        protected string serverHost = "";
        protected string serverUri = "/v2/index.php";
        protected string secretId = "";
        protected string secretKey = "";
        protected string defaultRegion = "";
        protected string requestMethod = "GET";

        public string SecretId
        {
            set { secretId = value; }
        }

        public string SecretKey
        {
            set { secretKey = value; }
        }

        public string DefaultRegion
        {
            set { defaultRegion = value; }
        }

        public string RequestMethod
        {
            set { requestMethod = value; }
        }

        public void setConfig(SortedDictionary<string, object> config)
        {
            if (config == null)
                return;
            foreach (string key in config.Keys)
            {
                switch (key)
                {
                    case "SecretId":
                        secretId = config["SecretId"].ToString();
                        break;
                    case "SecretKey":
                        secretKey = config["SecretKey"].ToString();
                        break;
                    case "DefaultRegion":
                        defaultRegion = config["DefaultRegion"].ToString();
                        break;
                    case "RequestMethod":
                        requestMethod = config["RequestMethod"].ToString();
                        break;
                }
            }
        }

        private string UpperCaseFirst(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public string GenerateUrl(string actionName, SortedDictionary<string, object> requestParams)
        {
            actionName = UpperCaseFirst(actionName);
            if (requestParams == null)
                requestParams = new SortedDictionary<string, object>();
            requestParams["Action"] = actionName;
            if (!requestParams.ContainsKey("Region"))
            {
                requestParams["Region"] = defaultRegion;
            }
            return Request.GenerateUrl(requestParams, secretId, secretKey, requestMethod, serverHost, serverUri);
        }

        public string Call(string actionName, SortedDictionary<string, object> requestParams, string fileName = null)
        {
            actionName = UpperCaseFirst(actionName);
            if (requestParams == null)
                requestParams = new SortedDictionary<string, object>();
            requestParams["Action"] = actionName;
            if (!requestParams.ContainsKey("Region"))
            {
                requestParams["Region"] = defaultRegion;
            }
            return Request.Send(requestParams, secretId, secretKey, requestMethod, serverHost, serverUri, fileName);
        }
    }
}