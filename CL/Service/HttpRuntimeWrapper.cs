using CL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CL.Service
{
    public class HttpRuntimeWrapper : IHttpRuntimeWrapper
    {
        public string AppDomainAppPath()
        {

            return HttpRuntime.AppDomainAppPath;

        }
    }
}
