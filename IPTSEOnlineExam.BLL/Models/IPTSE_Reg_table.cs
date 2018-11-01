namespace IPTSE_portal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IPTSE_Reg_table
    {
        public decimal Id { get; set; }
        public string first_name { get; set; }
        public string mid_name { get; set; }
        public string last_name { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string fathername { get; set; }
        public string mothername { get; set; }
        public string country { get; set; }
        public string addr1 { get; set; }
        public string addr2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string contact { get; set; }
        public string schoolname { get; set; }
        public string InstitutionType { get; set; }
        public string standard { get; set; }
        public string volunteername { get; set; }
        public string School_ID { get; set; }
    }
}
