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
    public class FinalTestBLL
    {
        int i = 0;
        public List<Questions> GenerateFinalQuestions(int testId)
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            Questions objQuestions = new Questions();
            List<Questions> lstQuestions = new List<Questions>();
            using (var ctx = new IPTSE_EXAMEntities())
            {
                var lstQuestionsNew = ctx.Database
                                    .SqlQuery<Questions>("SELECT A.*,QM.TestId  FROM tbl_Question_Category AC(NoLock) INNER JOIN(SELECT ROW_NUMBER() OVER(PARTITION BY A.QuestionCategoryId ORDER BY CHECKSUM(NEWID())) AS RowNumber, A.* FROM tbl_Question A(NoLock)) A ON  A.QuestionCategoryId = AC.Id left outer join tbl_Test_Question_Map QM on  QM.QuestionId=A.Id WHERE A.RowNumber < 3 and QM.TestId="+ testId + "")
                                    .ToList();
                lstQuestions = lstQuestionsNew;
            }
            int index = 0;
            foreach (var quest in lstQuestions)
            {
                quest.QuestNo = ++index;
                quest.questionsChoice = objEntities.tbl_Question_Choice.Where(t => t.Question_Id == quest.Id && t.IsActive == true)
                                                                 .Select(t1 =>
                                                                 new QuestionsChoice
                                                                 {
                                                                     Id = t1.Id,
                                                                     Question_Id = t1.Question_Id,
                                                                     ChoiceText = t1.ChoiceText,
                                                                     IsAnswer = t1.IsAnswer,

                                                                 }).ToList();
            }
            return lstQuestions;
        }

    }
}
