//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Gallery
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public bool Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LangCode { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int PageElementId { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public Nullable<int> Ordering { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public bool IsHome { get; set; }
    }
}