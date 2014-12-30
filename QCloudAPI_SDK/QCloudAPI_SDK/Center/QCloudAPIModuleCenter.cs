using System;
using System.Collections.Generic;
using System.Text;
using QCloudAPI_SDK.Module;


namespace QCloudAPI_SDK.Center
{
    /// <summary>
    /// 模块调用类 
    /// </summary>
    class QCloudAPIModuleCenter
    {
        private Base module;

        /// <summary>
        /// 构造模块调用类
        /// </summary>
        /// <param name="module">实际模块实例</param>
        /// <param name="config">模块配置参数</param>
        public QCloudAPIModuleCenter(Base module, SortedDictionary<string, object> config)
        {
            this.module = module;
            this.module.setConfig(config);
        }

        /// <summary>
        /// 生成Api调用地址
        /// </summary>
        /// <param name="actionName">模块动作名称</param>
        /// <param name="requestParams">模块请求参数</param>
        /// <returns>Api调用地址</returns>
        public string GenerateUrl(string actionName, SortedDictionary<string, object> requestParams)
        {
            return module.GenerateUrl(actionName, requestParams);
        }

        /// <summary>
        /// Api调用
        /// </summary>
        /// <param name="actionName">模块动作名称</param>
        /// <param name="requestParams">模块请求参数</param>
        /// <returns>json字符串</returns>
        public string Call(string actionName, SortedDictionary<string, object> requestParams)
        {
            var m = module.GetType();
            var method = m.GetMethod(actionName);
            if (method != null)
            {
                object[] p = {requestParams};
                return (string)method.Invoke(module, p);
            }
            return module.Call(actionName, requestParams);
        }

        public string SecretId
        {
            set { module.SecretId = value; }
        }

        public string SecretKey
        {
            set { module.SecretKey = value; }
        }

        public string DefaultRegion
        {
            set { module.DefaultRegion = value; }
        }

        public string RequestMethod
        {
            set { module.RequestMethod = value; }
        }
    }
}
