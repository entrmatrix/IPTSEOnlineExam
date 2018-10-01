using IPTSEOnlineExam.BLL.Models;
using IPTSEOnlineExam.Common;
using IPTSEOnlineExam.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL
{
    public class MockTestBLL
    {
        public List<Questions> GenerateQuestions()
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            Questions objQuestions = new Questions();
            List<Questions> lstQuestions = new List<Questions>();
            IQueryable<Questions> questionList = GetQuestions(objEntities,1,1);
            lstQuestions.AddRange(QuestionHelper.GetRandom(questionList, 10));

            return lstQuestions;
        }

        private IQueryable<Questions> GetQuestions(IPTSE_EXAMEntities objEntities,int categoryId, int testId)
        {
            return from quest in objEntities.tbl_Question
                   join questMap in objEntities.tbl_Test_Question_Map
                   on quest.Id equals questMap.QuestionId
                   where questMap.TestId == testId
                   select new Questions
                   {
                       Id = quest.Id,
                       QuestionText = quest.QuestionText,
                       QuestionCategoryId = quest.QuestionCategoryId,
                       questionsChoice = quest.tbl_Question_Choice.Where(t => t.Question_Id == quest.Id && t.IsActive == true)
                                                                .Select(t1 =>
                                                                new QuestionsChoice
                                                                {
                                                                    Id = t1.Id,
                                                                    Question_Id = t1.Question_Id,
                                                                    ChoiceText = t1.ChoiceText
                                                                }).ToList(),
                       Points = quest.Points,
                   };
        }

       
    }
}
