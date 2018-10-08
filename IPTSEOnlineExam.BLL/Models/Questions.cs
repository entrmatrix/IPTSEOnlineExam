using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class Questions
    {
        private static int _value = 0;
        private static readonly object m_lock = new object();
        public int Id { get; set; }
        public Nullable<int> QuestionCategoryId { get; set; }
        public string QuestionText { get; set; }
        public Nullable<int> Points { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int? TestId { get; set; }
        public Nullable<int> selectedvalue { get; set; }
        public List<QuestionsChoice> questionsChoice { get; set; }
        public int QuestNo
        {
            get;set;
            //{
            //    lock (m_lock)
            //    {
            //        if (_value == Int32.MaxValue)
            //            _value = 0;
            //        return ++_value;
            //    }
            //}

        }
        public bool skipQuestions { get; set; }
        public int skippedTime { get; set; }
    }
}
