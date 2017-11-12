### qcloudapi-sdk-dotnet

qcloudapi-sdk-dotnet是为了让.Net开发者能够在自己的代码里更快捷方便的使用腾讯云的API而开发的SDK工具包。

#### 更新

* 2017-11-12 新增Bgpip模块
* 2017-07-31 新增Bmeip和Bmvpc模块
* 2017-07-31 新增Feecenter模块
* 2017-07-31 新增Bm和Bmlb模块
* 2017-07-12 回滚：不默认传Version参数
* 2017-05-19 设置接口默认Version： Cvm模块新版本API已经上线，通过是否传Version区分新旧版本。SDK默认调用新接口，因此需要增加Version的默认设置。 CvmAPI接口介绍见：https://www.qcloud.com/document/api/213/569
* 2017-03-10 增加HmacSHA256签名算法的兼容
* 07-15 增加Tdsql模块

#### 资源

* [公共参数](http://wiki.qcloud.com/wiki/%E5%85%AC%E5%85%B1%E5%8F%82%E6%95%B0)
* [API列表](http://wiki.qcloud.com/wiki/API)
* [错误码](http://wiki.qcloud.com/wiki/%E9%94%99%E8%AF%AF%E7%A0%81)

#### 入门

1. 申请安全凭证。
在第一次使用云API之前，用户首先需要在腾讯云网站上申请安全凭证，安全凭证包括 SecretId 和 SecretKey, SecretId 是用于标识 API 调用者的身份，SecretKey是用于加密签名字符串和服务器端验证签名字符串的密钥。SecretKey 必须严格保管，避免泄露。

2. 下载SDK，放入到您的程序目录。
使用方法请参考下面的例子。

#### 例子

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
	    //在这里指定所用的签名算法，不指定默认为HmacSHA1
	    //requestParams["SignatureMethod"] = "HmacSHA256";
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
