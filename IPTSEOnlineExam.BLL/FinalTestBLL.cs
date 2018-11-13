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
        public List<Questions> GenerateFinalQuestionsSchool(int testId)
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            Questions objQuestions = new Questions();
            List<Questions> lstQuestions = new List<Questions>();
            using (var ctx = new IPTSE_EXAMEntities())
            {
                var lstQuestionsNew = ctx.Database
                                    .SqlQuery<Questions>("select * from (select Row_number() over (Partition by QuestionCategoryId , QuestionDifficultyId order by NEWID()) Sno ,Q.Id,Q.QuestionText,Q.QuestionCategoryId ,Q.QuestionDifficultyId,QM.TestId	from tbl_Question Q	left outer join tbl_Test_Question_Map QM on  QM.QuestionId=Q.Id	Where QM.TestId = " + testId + "  and Q.IsActive = 1 and Q.QuestionDifficultyId<>4)  as a where a. Sno < = 2")
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
        public List<Questions> GenerateFinalQuestionsCollege(int testId)
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            Questions objQuestions = new Questions();
            List<Questions> lstQuestions = new List<Questions>();
            using (var ctx = new IPTSE_EXAMEntities())
            {
                //var lstQuestionsNew = ctx.Database
                //                    .SqlQuery<Questions>("SELECT A.*,QM.TestId  FROM tbl_Question_Category AC(NoLock) INNER JOIN(SELECT ROW_NUMBER() OVER(PARTITION BY A.QuestionCategoryId ORDER BY CHECKSUM(NEWID())) AS RowNumber, A.* FROM tbl_Question A(NoLock)) A ON  A.QuestionCategoryId = AC.Id left outer join tbl_Test_Question_Map QM on  QM.QuestionId=A.Id WHERE A.RowNumber < 3 and QM.TestId=" + testId + "")
                //                    .ToList();
                var lstQuestionsNew = ctx.Database
                                    .SqlQuery<Questions>("select * from (select Row_number() over (Partition by QuestionCategoryId , QuestionDifficultyId order by NEWID()) Sno ,Q.Id,Q.QuestionText,Q.QuestionCategoryId ,Q.QuestionDifficultyId,QM.TestId	from tbl_Question Q	left outer join tbl_Test_Question_Map QM on  QM.QuestionId=Q.Id	Where QM.TestId = " + testId + " and Q.IsActive = 1) as a where a. Sno < = 2")
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
        public void IsExamCompletion(UserExamCompletion objExamCompletion)
        {
            tbl_txn_Test_Completion objTestCompletion = new tbl_txn_Test_Completion();

            using (var objContext = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = objContext.Database.BeginTransaction())
                {
                    try
                    {
                        objTestCompletion.CandidateId = Convert.ToInt32(objExamCompletion.CandidateId);
                        objTestCompletion.IsExamCompleted = objExamCompletion.IsExamCompleted;
                        objTestCompletion.CreatedBy = objExamCompletion.CreatedBy;
                        objTestCompletion.CreatedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                        objContext.tbl_txn_Test_Completion.Add(objTestCompletion);
                        objContext.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        public bool IsExamGiven(login_table objUProfile)
        {
            tbl_txn_Test_Completion objTestCompletion = new tbl_txn_Test_Completion();

            using (var objContext = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = objContext.Database.BeginTransaction())
                {
                    try
                    {
                        var isCompletion = objContext.tbl_txn_Test_Completion.Where(t => t.CreatedBy == objUProfile.email && t.CandidateId == objUProfile.Id).Select(t1 => t1).FirstOrDefault();
                        if(isCompletion != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch { return false; }
                }
            }
        }
    }
}
