using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Data.Entity.ModelConfiguration.Configuration;


namespace CL.Interfaces
{
    [InheritedExport]
    public interface IMapping
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="cr"></param>
        void RegistTo(ConfigurationRegistrar cr);

    }
}
