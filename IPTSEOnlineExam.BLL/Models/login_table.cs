using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq.Mapping;

namespace IPTSEOnlineExam.BLL.Models
{
    public class login_table
    {
        public decimal Id { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public string Login_type { get; set; }
    }
}
