using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Types
{
    public enum DALType
    {
        select, insert, delete, update
    }

    public enum GridColumnType : int
    {
        none = 0,
        textarea = 1,
        date = 2,
        radiobutton = 3,
        checkbox = 4,
        file = 5,
        hidden = 6,
        password = 7
    }
}
