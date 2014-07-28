using CL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    [Serializable]
    [Table("News")]
    public class News : MappingBase<News>
    {

        [JqGrid(Title = "新闻ID", IsKey = "true", IsEdit = "false", IsList = "false", IsCreate = "false")]
        [Key]
        public int ID { get; set; }


        [JqGrid(Title = "新闻标题", InputClass = "validate[required]")]
        public string Title { get; set; }

        [JqGrid(Title = "新闻内容", InputClass = "validate[required]")]
        public string NewsContent { get; set; }


        [JqGrid(Title = "作者")]
        public string Author { get; set; }


        [JqGrid(Title = "来源")]
        public string Source { get; set; }

        [JqGrid(Title = "新闻日期")]
        public DateTime NewsDate { get; set; }


        [JqGrid(Title = "上传日期", InputClass = "validate[required]")]
        public DateTime Mtime { get; set; }


        [JqGrid(Title = "操作者", InputClass = "validate[required]")]
        public string Muser { get; set; }

        [JqGrid(Title = "备注")]
        public string Mnote { get; set; }



    }
}
