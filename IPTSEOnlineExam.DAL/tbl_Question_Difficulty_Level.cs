//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IPTSEOnlineExam.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Question_Difficulty_Level
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Question_Difficulty_Level()
        {
            this.tbl_Question = new HashSet<tbl_Question>();
        }
    
        public int Id { get; set; }
        public string Difficulty_Level { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Question> tbl_Question { get; set; }
    }
}
