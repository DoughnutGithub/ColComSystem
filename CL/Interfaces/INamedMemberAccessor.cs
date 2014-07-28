using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Interfaces
{
    /// <summary>
    /// 属性访问器
    /// </summary>
    internal interface INamedMemberAccessor
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object GetValue(object instance);
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="newValue"></param>
        void SetValue(object instance, object newValue);
    }  
}
