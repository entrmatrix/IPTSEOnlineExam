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

        public void SaveAnswer(Questions questions, login_table objUserProfile)
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
                        objQuest.CandidateId = Convert.ToInt32(objUserProfile.Id);
                        objQuest.CandidateEmailId = objUserProfile.email;
                        objQuest.TestXQId = questions.Id;
                        objQuest.ChoiceId = questions.selectedvalue;
                        objQuest.MarkScored = questions.markScored;
                        var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == questions.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                        if (isRight == "true")
                        {
                            objQuest.MarkScored = objContext.tbl_Question.Where(t1 => t1.Id == questions.Id).Select(t2 => t2.Points).FirstOrDefault();
                        }
                        objQuest.CreatedBy = objUserProfile.email;
                        objQuest.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                        objContext.tbl_Txn_Test_Result.Add(objQuest);
                        objMap.TestXQId = questions.Id;
                        objMap.AnswerTime_Sec = Convert.ToInt32(60 - Convert.ToInt32(questions.skippedTime.Substring(questions.skippedTime.Length - 2)));
                        //objMap.AnswerTime_Sec = Convert.ToInt32(timeSpend);
                        objMap.CreatedBy= objUserProfile.email;
                        objMap.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); 
                        objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
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
        public void SaveAnswerTimeOut(Questions questions, login_table objUserProfile)
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
                        objQuest.CandidateId = Convert.ToInt32(objUserProfile.Id);
                        objQuest.CandidateEmailId = objUserProfile.email;
                        objQuest.TestXQId = questions.Id;
                        objQuest.ChoiceId = 0;
                        objQuest.MarkScored = questions.markScored;
                        var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == questions.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                        if (isRight == "true")
                        {
                            objQuest.MarkScored = objContext.tbl_Question.Where(t1 => t1.Id == questions.Id).Select(t2 => t2.Points).FirstOrDefault();
                        }
                        objQuest.CreatedBy = objUserProfile.email;
                        objQuest.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                        objContext.tbl_Txn_Test_Result.Add(objQuest);
                        objMap.TestXQId = questions.Id;
                        objMap.AnswerTime_Sec = 60;
                        //objMap.AnswerTime_Sec = Convert.ToInt32(timeSpend);
                        objMap.CreatedBy = objUserProfile.email;
                        objMap.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                        objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
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

        public void SaveRemaining(Questions quest, UserProfile objUserProfile)
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
                        objQuest.CandidateId = objUserProfile.Id;
                        objQuest.CandidateEmailId = objUserProfile.email;
                        objQuest.TestXQId = quest.Id;
                        objQuest.ChoiceId = quest.selectedvalue;
                        objQuest.MarkScored = quest.markScored;
                        var isRight = objContext.tbl_Question_Choice.Where(t => t.Id == quest.selectedvalue).Select(t1 => t1.IsAnswer).ToString();
                        if (isRight == "true")
                        {
                            objQuest.MarkScored = objContext.tbl_Question.Where(t1 => t1.Id == quest.Id).Select(t2 => t2.Points).FirstOrDefault();
                        }
                        objContext.tbl_Txn_Test_Result.Add(objQuest);
                        objMap.TestXQId = quest.Id;
                        objMap.AnswerTime_Sec = objMap.AnswerTime_Sec = Convert.ToInt32(60 - Convert.ToInt32(quest.skippedTime.Substring(quest.skippedTime.Length - 2)));
                        objContext.tbl_Txn_Question_Duration_Map.Add(objMap);
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

        public bool isAnswer(int? choiceId)
        {
            Boolean isAns;
            using (var objContext = new IPTSE_EXAMEntities())
            {
                isAns = objContext.tbl_Question_Choice.Where(t => t.Id == choiceId).Select(t1 => t1.IsAnswer).FirstOrDefault();
            }
            return isAns;
        }
        public string selectedAnswer(int? choiceId)
        {
            string choiceText="";
            using (var objContext = new IPTSE_EXAMEntities())
            {
                choiceText = objContext.tbl_Question_Choice.Where(t => t.Id == choiceId).Select(t1 => t1.ChoiceText).FirstOrDefault();
            }
            return choiceText;
        }
    }
}
