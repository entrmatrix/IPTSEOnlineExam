using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class TestQuestions
    {
        public int ID { get; set; }
        public Nullable<int> TestId { get; set; }
        public string QuestionId { get; set; }
        public Nullable<int> QuestionSeqNo { get; set; }
        public Nullable<int> AnswerChoiceId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
