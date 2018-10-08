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
        int i = 0;
        public List<Questions> GenerateQuestions()
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            Questions objQuestions = new Questions();
            List<Questions> lstQuestions = new List<Questions>();
            IQueryable<Questions> questionList = GetQuestions(objEntities, 1, 1);
            lstQuestions.AddRange(QuestionHelper.GetRandom(questionList, 10));
            int index = 0;
            foreach (var quest in lstQuestions)
            {
                quest.QuestNo = ++index;
            }
            return lstQuestions;
        }

        private IQueryable<Questions> GetQuestions(IPTSE_EXAMEntities objEntities, int categoryId, int testId)
        {
            int qNo = 0;
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
                                                                    ChoiceText = t1.ChoiceText,
                                                                    IsAnswer = t1.IsAnswer,

                                                                }).ToList(),
                       Points = quest.Points,
                       TestId = questMap.TestId,
                       skipQuestions = false
                   };
        }
       
        public void SaveAnswer(Questions questions, string timeSpend)
        {
            tbl_Txn_Test_Result objQuest = new tbl_Txn_Test_Result();
            tbl_Question_Choice objQuestChoice = new tbl_Question_Choice();
            tbl_Txn_Question_Duration_Map objMap = new tbl_Txn_Question_Duration_Map();
            
            using (var objContext = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = objContext.Database.BeginTransaction())
                {
                    try
                    {
                        
                        objQuest.CandidateId = 1;
                        objQuest.CandidateEmailId = "mk.pathak@gmail.com";
                        objQuest.TestXQId = questions.Id;
                        objQuest.ChoiceId = questions.selectedvalue;
                        var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == questions.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                        if (isRight == "true")
                        {
                            objQuest.MarkScored = objContext.tbl_Question.Where(t1 => t1.Id == questions.Id).Select(t2 => t2.Points).FirstOrDefault();
                        }
                        objContext.tbl_Txn_Test_Result.Add(objQuest);
                        objMap.TestXQId = questions.Id;
                        objMap.AnswerTime_Sec = Convert.ToInt32(timeSpend);
                        objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
                        objContext.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    catch { }
                }
            }
        }
        public void SaveAnswerTimeOut(string QuestId, string isTimeOut, string totalTime)
        {
            tbl_Txn_Test_Result objQuest = new tbl_Txn_Test_Result();
            tbl_Question_Choice objQuestChoice = new tbl_Question_Choice();
            tbl_Txn_Question_Duration_Map objMap = new tbl_Txn_Question_Duration_Map();

            using (var objContext = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = objContext.Database.BeginTransaction())
                {
                    try
                    {
                        var questions = objContext.tbl_Question.Where(a => a.Id == Convert.ToInt32(QuestId)).Select(a1 => a1).FirstOrDefault();
                        objQuest.CandidateId = 1;
                        objQuest.CandidateEmailId = "mk.pathak@gmail.com";
                        objQuest.TestXQId = questions.Id;
                        objQuest.ChoiceId = 0;
                        //var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == questions.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                        //if (isRight == "true")
                        //{
                        objQuest.MarkScored = 0;// objContext.tbl_Question.Where(t1 => t1.Id == questions.Id).Select(t2 => t2.Points).FirstOrDefault();
                        //}
                        objContext.tbl_Txn_Test_Result.Add(objQuest);
                        objMap.TestXQId = questions.Id;
                        objMap.AnswerTime_Sec = Convert.ToInt32(totalTime);
                        objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
                        objContext.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    catch { }
                }
            }
        }

        public void SaveRemaining(List<Questions> lstQuestions, string timeSpend)
        {
            tbl_Txn_Test_Result objQuest = new tbl_Txn_Test_Result();
            tbl_Question_Choice objQuestChoice = new tbl_Question_Choice();
            tbl_Txn_Question_Duration_Map objMap = new tbl_Txn_Question_Duration_Map();

            using (var objContext = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = objContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var quest in lstQuestions)
                        {
                            objQuest.CandidateId = 1;
                            objQuest.CandidateEmailId = "mk.pathak@gmail.com";
                            objQuest.TestXQId = quest.Id;
                            objQuest.ChoiceId = quest.selectedvalue;
                            var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == quest.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                            if (isRight == "true")
                            {
                                objQuest.MarkScored = objContext.tbl_Question.Where(t1 => t1.Id == quest.Id).Select(t2 => t2.Points).FirstOrDefault();
                            }
                            objContext.tbl_Txn_Test_Result.Add(objQuest);
                            objMap.TestXQId = quest.Id;
                            objMap.AnswerTime_Sec = Convert.ToInt32(timeSpend);
                            objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
                            objContext.SaveChanges();
                        }
                        dbcxtransaction.Commit();
                    }
                    catch { }
                }
            }
        }
    }
}
