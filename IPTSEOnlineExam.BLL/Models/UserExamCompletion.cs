using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class UserExamCompletion
    {
        public int Id { get; set; }
        public Nullable<int> CandidateId { get; set; }
        public Nullable<bool> IsExamCompleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
    }
}
