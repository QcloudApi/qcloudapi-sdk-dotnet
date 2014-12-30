using System;
using System.Collections.Generic;
using System.Text;
using QCloudAPI_SDK.Center;
using QCloudAPI_SDK.Module;

namespace QCloudAPI_SDK
{
    class Demo
    {
        static void Main(string[] args)
        {
            SortedDictionary<string, object> config = new SortedDictionary<string, object>(StringComparer.Ordinal);
		    config["SecretId"] = "你的secretId";
		    config["SecretKey"] = "你的secretKey";
            config["RequestMethod"] = "GET";
		    config["DefaultRegion"] = "gz";

            QCloudAPIModuleCenter module = new QCloudAPIModuleCenter(new Cvm(), config);
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["offset"] = 0;
            requestParams["limit"] = 3;
            Console.WriteLine(module.GenerateUrl("DescribeInstances", requestParams));
            Console.WriteLine(module.Call("DescribeInstances", requestParams));


            //SortedDictionary<string, object> config = new SortedDictionary<string, object>(StringComparer.Ordinal);
            //config["SecretId"] = "你的secretId";
            //config["SecretKey"] = "你的secretKey";
            //config["RequestMethod"] = "POST";
            //config["DefaultRegion"] = "gz";

            //UploadCdnEntity
            //string fileName = "c:\\del.bat";
            //SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            //requestParams["entityFileName"] = "/upload/del.bat";
            //requestParams["entityFile"] = fileName;
            //QCloudAPIModuleCenter module = new QCloudAPIModuleCenter(new Cdn(), config);
            //Console.WriteLine(module.Call("UploadCdnEntity", requestParams));

            Console.ReadKey();
        }
    }
}
