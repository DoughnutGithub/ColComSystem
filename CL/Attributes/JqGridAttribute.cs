using CL.Common;
using CL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Attributes
{
    public class JqGridAttribute : Attribute
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set { _title = value; }
        }

        public string IsList { get; set; }
        public string IsKey { get; set; }
        public string IsCreate { get; set; }
        public string IsEdit { get; set; }
        public string IsHidden { get; set; }
        public string Width { get; set; }
        public string ListClass { get; set; }
        public string Display { get; set; }
        /// <summary>
        /// { 'M': 'Male', 'F': 'Female' }
        /// '@Url.Action("GetCityOptions")'
        /// </summary>
        public string Options { get; set; }
        public GridColumnType type { get; set; }
        /// <summary>
        /// yy-mm-dd
        /// </summary>
        public string DisplayFormat { get; set; }
        public string Values { get; set; }
        public string DefaultValue { get; set; }
        public string IsSort { get; set; }
        /// <summary>
        /// validate[required]
        /// </summary>
        public string InputClass { get; set; }

        public bool? List
        {
            get { return string.IsNullOrEmpty(IsList) ? null : ConvertTo.ConvertToBooleanNull(IsList); }
        }
        public bool? Key
        {
            get { return string.IsNullOrEmpty(IsKey) ? null : ConvertTo.ConvertToBooleanNull(IsKey); }
        }
        public bool? Create
        {
            get { return string.IsNullOrEmpty(IsCreate) ? null : ConvertTo.ConvertToBooleanNull(IsCreate); }
        }
        public bool? Edit
        {
            get { return string.IsNullOrEmpty(IsEdit) ? null : ConvertTo.ConvertToBooleanNull(IsEdit); }
        }
        public bool? Hidden
        {
            get { return string.IsNullOrEmpty(IsHidden) ? null : ConvertTo.ConvertToBooleanNull(IsHidden); }
        }
        public bool? Sort
        {
            get { return string.IsNullOrEmpty(IsSort) ? null : ConvertTo.ConvertToBooleanNull(IsSort); }
        }


    }
}
