//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_Emp_Adm_Profile.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Answer()
        {
            this.Emps = new HashSet<Emp>();
        }
    
        public int AnsId { get; set; }
        public string Answer1 { get; set; }
        public Nullable<int> EId { get; set; }
        public Nullable<int> QId { get; set; }
        public System.DateTime AnsDate { get; set; }
    
        public virtual Emp Emp { get; set; }
        public virtual Queryfeed Queryfeed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp> Emps { get; set; }
    }
}