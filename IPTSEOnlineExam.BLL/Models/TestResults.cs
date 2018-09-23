using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class TestResults
    {
        public int Id { get; set; }
        public Nullable<int> CandidateId { get; set; }
        public string CandidateEmailId { get; set; }
        public Nullable<int> TestXQId { get; set; }
        public Nullable<int> ChoiceId { get; set; }
        public Nullable<int> MarkScored { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
