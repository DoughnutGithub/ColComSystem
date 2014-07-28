using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using CL.Models;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using CL.Interfaces;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;


namespace CL.DB
{
    public class MyConfiguration : DbConfiguration
    {

        public MyConfiguration()
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => SuspendExecutionStrategy
              ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
              : new SqlAzureExecutionStrategy());
            this.SetDatabaseInitializer<EFHelper>(null);
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }

    }


    /// <summary>
    /// EF 操作数据
    /// SQL连接字符串使用默认名称:ConnectionString
    /// </summary>
    [DbConfigurationType(typeof(MyConfiguration))]
    public class EFHelper : DbContext
    {
        public EFHelper() :
            base(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {
            if (_mappings == null)
            {
                var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
                var container = new CompositionContainer(catalog);
                _mappings = container.GetExportedValues<IMapping>();
            }

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;


        }
        [ImportMany]
        static IEnumerable<IMapping> _mappings = null;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (_mappings != null)
            {
                foreach (var mapping in _mappings)
                {
                    mapping.RegistTo(modelBuilder.Configurations);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }


    }
}
