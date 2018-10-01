using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public Nullable<int> QuestionCategoryId { get; set; }
        public string QuestionText { get; set; }
        public Nullable<int> Points { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int TestId { get; set; }
        public List<QuestionsChoice> questionsChoice { get; set; }
    }
}
