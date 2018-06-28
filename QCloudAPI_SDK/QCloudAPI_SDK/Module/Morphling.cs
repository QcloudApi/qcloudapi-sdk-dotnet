using System;
using System.Collections.Generic;
using System.Text;

namespace QCloudAPI_SDK.Module
{
    public class Morphling : Base
    {
        public Morphling(string module)
        {
            serverHost = module + ".api.qcloud.com";
        }

        public void morph(string module)
        {
            serverHost = module + ".api.qcloud.com";
        }
    }
}
