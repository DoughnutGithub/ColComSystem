using CL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Interfaces
{
    /// <summary>
    /// 数据库
    /// </summary>
    public interface IDatabaseFactory
    {
        EFHelper Get();
    }
}
