using System;
using System.Collections.Generic;
using System.Text;
using QCloudAPI_SDK.Center;
using QCloudAPI_SDK.Module;

namespace QCloudAPI_SDK
{
    class DynamicModuleDemo
    {
        static void Main(string[] args)
        {
            SortedDictionary<string, object> config = new SortedDictionary<string, object>(StringComparer.Ordinal);
            // 从环境变量中读取您的密钥，您需要首先在环境变量中进行设置。
            // 如果环境变量中没有相关信息，执行程序时将报空指针错误。
            // 或者您可以将密钥直接写在代码中用于测试，但请注意复制、提交或者分发代码时将密钥对删除。
            config["SecretId"] = Environment.GetEnvironmentVariable("QCLOUD_SECRET_ID");
            config["SecretKey"] = Environment.GetEnvironmentVariable("QCLOUD_SECRET_KEY");
            config["RequestMethod"] = "GET";
            config["DefaultRegion"] = "gz";

            // 这里ckafka并未被SDK显式支持，采取动态模块的方法直接指定模块
            QCloudAPIModuleCenter module = new QCloudAPIModuleCenter(new Morphling("ckafka"), config);
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["offset"] = 0;
            requestParams["limit"] = 3;
            //您可以在这里指定签名算法，不指定默认为HmacSHA1
            //requestParams["SignatureMethod"] = "HmacSHA256";
            //Console.WriteLine(module.GenerateUrl("DescribeInstances", requestParams));
            Console.WriteLine(module.Call("ListInstance", requestParams));

            Console.ReadKey();
        }
    }
}
