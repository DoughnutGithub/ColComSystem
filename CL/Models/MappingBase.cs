using CL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using CL.Types;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace CL.Models
{
    [Serializable]
    public class MappingBase<TEntity> : IMapping
       where TEntity : class
    {
        public MappingBase()
        {

        }

        [NotMapped]
        public DALType DALStatus { get; set; }

        public void RegistTo(ConfigurationRegistrar cr)
        {
            try
            {
                cr.Add<TEntity>(new EntityTypeConfiguration<TEntity>());


            }
            catch { }
        }

        /// <summary>
        /// copy
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ShallowCopy()
        {
            return this.MemberwiseClone() as TEntity;
        }

    }
}
