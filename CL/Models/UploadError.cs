using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    /// <summary>
    /// 上错错误提示
    /// </summary>
    public class UploadError
    {
        /// <summary>
        /// 错误标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string Content { get; set; }

        public List<UploadError> Items = new List<UploadError>();

    }
}
