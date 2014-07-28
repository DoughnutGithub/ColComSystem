using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Interfaces
{
    public interface IMemberAccessor
    {
        /// <summary>  
        /// 获取属性值  
        /// </summary>  
        /// <param name="instance"></param>  
        /// <param name="memberName"></param>  
        /// <returns>The member value</returns>  
        object GetValue(object instance, string memberName);

        /// <summary>  
        /// 设置属性值  
        /// </summary>  
        /// <param name="instance"></param>  
        /// <param name="memberName"></param>  
        /// <param name="newValue"></param>  
        void SetValue(object instance, string memberName, object newValue);
    }  
}
