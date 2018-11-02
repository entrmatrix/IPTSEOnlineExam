using IPTSEOnlineExam.BLL.Models;
using IPTSEOnlineExam.Common;
using IPTSEOnlineExam.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTSEOnlineExam.BLL
{
    public class AdminBLL 
    {
        public List<Test> GetExam()
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            return objEntities.tbl_Test.Where(n => (bool)n.IsActive).
                Select(m => new Test { TestId = m.TestId, Name = m.Name, Description = m.Description, DurationInMinutes = m.DurationInMinutes }).ToList();
        }

        public List<Questions> GetQuestions()
        {
            IPTSE_EXAMEntities db = new IPTSE_EXAMEntities();
            return db.tbl_Question.Select(m => new Questions
            {
                Id = m.Id,
                IsActive = m.IsActive,
                QuestionText = m.QuestionText,
                QuestionCategoryId = m.QuestionCategoryId,
                TestId = m.tbl_Test_Question_Map.FirstOrDefault().TestId
            }).ToList();//.Include(m => m.questionsChoice).ToList();//.Include(q => q.Exam_tbl);

        }

        public Questions GetQuestionDetails(int? id)
        {
            using (IPTSE_EXAMEntities db = new IPTSE_EXAMEntities())
            {
                tbl_Question ques = db.tbl_Question.Find(id);
                if (ques == null)
                {
                    return null;
                }
                else
                {
                    return new Questions
                    {
                        Id = ques.Id,
                        QuestionText = ques.QuestionText,
                        IsActive = ques.IsActive,
                        QuestionCategoryId = ques.QuestionCategoryId,
                        questionsChoice = ques.tbl_Question_Choice.Select(m => new QuestionsChoice
                        {
                            Id = m.Id,
                            ChoiceText = m.ChoiceText,
                            IsActive = m.IsActive,
                            IsAnswer = m.IsAnswer,
                            Question_Id = m.Question_Id
                        }).ToList(),
                        TestId = ques.tbl_Test_Question_Map.Where(m => m.QuestionId == ques.Id).FirstOrDefault().TestId
                    };
                }
            }

            //if (questions_tbl == null)
            //{
            //    return HttpNotFound();
            //}
        }

        public bool ValidAnswer(Questions questions_tbl)
        {
            bool isValid = true;

            isValid = questions_tbl.questionsChoice.Any(m => m.IsAnswer) ? true : false;

            return isValid;
        }

        public List<QuestionCategory> GetQuestionCategory()
        {
            IPTSE_EXAMEntities objEntities = new IPTSE_EXAMEntities();
            return objEntities.tbl_Question_Category.Select(m => new QuestionCategory
            {
                Category = m.Category,
                Id = m.Id
            }).ToList();
        }

        public void AddQuestion(Questions in_question)
        {

            using (IPTSE_EXAMEntities db = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        tbl_Question question = new tbl_Question();
                        tbl_Question_Category question_Category = new tbl_Question_Category();
                        tbl_Question_Choice question_Choice = new tbl_Question_Choice();
                        //tbl_Test test = new tbl_Test();
                        tbl_Test_Question_Map test_question_Map = new tbl_Test_Question_Map();

                        question.QuestionText = in_question.QuestionText;
                        question.IsActive = in_question.IsActive;
                        question.Points = in_question.Points;
                        question.QuestionCategoryId = (int)in_question.QuestionCategoryId;
                        question.CreatedBy = "Admin";
                        question.CreatedDate = DateTime.Now;
                        db.tbl_Question.Add(question);
                        db.SaveChanges();
                        //question.Id

                        db.tbl_Test_Question_Map.Add(new tbl_Test_Question_Map
                        {
                            TestId = in_question.TestId,
                            QuestionId = question.Id,
                            IsActive = in_question.IsActive,
                            CreatedBy = "Admin",
                            CreatedDate = DateTime.Now
                        });

                        foreach (QuestionsChoice item in in_question.questionsChoice)
                        {
                            db.tbl_Question_Choice.Add(new tbl_Question_Choice
                            {
                                ChoiceText = item.ChoiceText,
                                IsActive = item.IsActive,
                                IsAnswer = item.IsAnswer,
                                Question_Id = question.Id,
                                CreatedBy = "Admin",
                                CreatedDate = DateTime.Now
                                //,tbl_Question = question
                            });
                        }
                        db.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbcxtransaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbcxtransaction.Rollback();
                        throw ex;
                    }

                }
            }
        }

        public void SaveChanges(Questions ques)
        {
            using (IPTSE_EXAMEntities db = new IPTSE_EXAMEntities())
            {
                using (var dbcxtransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        tbl_Question tbl_Ques = new tbl_Question();
                        tbl_Ques.Id = ques.Id;
                        tbl_Ques.QuestionText = ques.QuestionText;
                        tbl_Ques.QuestionCategoryId = (int)ques.QuestionCategoryId;
                        tbl_Ques.IsActive = ques.IsActive;
                        tbl_Ques.CreatedBy = "Admin";
                        tbl_Ques.CreatedDate = DateTime.Now;
                        ques.questionsChoice.ForEach(m => tbl_Ques.tbl_Question_Choice.Add(new tbl_Question_Choice
                        {
                            Id = m.Id,
                            Question_Id = m.Question_Id,
                            ChoiceText = m.ChoiceText,
                            IsAnswer = m.IsAnswer,
                            IsActive = m.IsActive,
                            CreatedBy = "Admin",
                            CreatedDate = tbl_Ques.CreatedDate
                        }));

                        db.Entry(tbl_Ques).State = EntityState.Modified;

                        tbl_Test_Question_Map map = db.tbl_Test_Question_Map.FirstOrDefault(m => m.QuestionId == ques.Id);

                        //tbl_Ques.tbl_Test_Question_Map.Add(new tbl_Test_Question_Map
                        //{
                        //    ID = map.ID,
                        //    IsActive = true,
                        //    QuestionId = ques.Id,
                        //    TestId = ques.TestId,
                        //    CreatedBy = "Admin",
                        //    CreatedDate = tbl_Ques.CreatedDate
                        //});

                        foreach (var item in tbl_Ques.tbl_Test_Question_Map)
                        {
                            item.TestId = ques.TestId;
                            db.Entry(item).State = EntityState.Modified;
                        }

                        foreach (var item in tbl_Ques.tbl_Question_Choice)
                            db.Entry(item).State = EntityState.Modified;

                        db.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbcxtransaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbcxtransaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
