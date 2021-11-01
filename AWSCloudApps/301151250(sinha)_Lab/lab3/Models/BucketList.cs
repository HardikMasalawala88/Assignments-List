using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Models
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
