using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL.Models
{
    public class QuestionsChoice
    {
        public int Id { get; set; }
        public int Question_Id { get; set; }
        public string ChoiceText { get; set; }
        public bool IsActive { get; set; }
        public bool IsAnswer { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
