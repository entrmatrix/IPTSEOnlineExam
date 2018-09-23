using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class QuestionDuration
    {
        public int Id { get; set; }
        public Nullable<int> TestXQId { get; set; }
        public Nullable<System.DateTime> RequestTime { get; set; }
        public Nullable<System.DateTime> LeaveTime { get; set; }
        public Nullable<int> AnswerTime_Sec { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
