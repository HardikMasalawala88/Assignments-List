using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS_WPF_AWS_S3_IAM.Class
{
    public class BucketList
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }

    }

    public class BucketFileList
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public long Size { get; set; }

    }
}
