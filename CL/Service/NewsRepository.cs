using CL.Interfaces;
using CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Service
{
    public class NewsRepository : RepositoryBase<News>,INewsRepository
    {
        public NewsRepository(IDatabaseFactory df) : base(df) { }
    }
}
