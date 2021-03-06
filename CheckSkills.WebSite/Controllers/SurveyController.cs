﻿using System;
using System.Collections.Generic;
using System.Linq;
using CheckSkills.DAL;
using CheckSkills.Domain;
using CheckSkills.Domain.Entities;
using CheckSkills.WebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace CheckSkills.WebSite.Controllers
{
    public class SurveyController : BaseController
    {
        private ISurveyDao _surveyDao;
        private IQuestionDao _questionDao;
        private IQuestionCategoryDao _categoryDao;
        private IQuestionDifficultyDao _difficultyDao;
        private IQuestionTypeDao _questionTypeDao;
        private IAnswerDao _answerDao;
        private ISurvey_QuestionDao _survey_Question;

        private const int CATEGORY_ID = 0;
        private const int TYPE_ID = 0;
        private const int DIFFICULTY_ID = 0;

        public SurveyController()
        {
            _answerDao = new AnswerDao();
            _surveyDao = new SurveyDao();
            _questionDao = new QuestionDao(_answerDao);
            _categoryDao = new QuestionCategoryDao();
            _difficultyDao = new QuestionDifficultyDao();
            _questionTypeDao = new QuestionTypeDao();
            _survey_Question = new Survey_QuestionDao();

        }




        // actions permettant de creer le formulaire.
        [HttpGet]
        public IActionResult Create()
        {
            var model = BuildSurveyViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(IEnumerable<SurveySelectedQuestionViewModel> surveySelectedQuestions)
        {
            var selectedQuestionIds = surveySelectedQuestions.Where(q => q.IsChecked).Select(ssq => ssq.Id);
            var selectedQuestionViewModels = GetSelectedQuestionViewModel(selectedQuestionIds);
            var model = new CreateConfirmationSurveyViewModel()
            {
                SurveySelectedQuestions = selectedQuestionViewModels,
                OriginalSurveySelectedQuestions = selectedQuestionIds
            };

            return View("List", model);
        }



        //action permettant d'imprimer le formulaire generer.
        [HttpGet]
        public IActionResult PrintSurvey(IEnumerable<int> surveySelectedQuestions)
        {
            var selectedQuestionViewModels = GetSelectedQuestionViewModel(surveySelectedQuestions);
            var model = new CreateConfirmationSurveyViewModel()
            {
                SurveySelectedQuestions = selectedQuestionViewModels,
                OriginalSurveySelectedQuestions = surveySelectedQuestions
            };
            return new ViewAsPdf("PrintSurvey", model);
        }


        [HttpPost]
        public IActionResult SaveSurvey(CreateConfirmationSurveyViewModel surveyModel)
        {
            if (ModelState.IsValid)
            {
                var survey = new Survey
                {
                    Name = surveyModel.Name,
                };

                try
                {
                    _surveyDao.CreateSurvey(surveyModel.Name, surveyModel.OriginalSurveySelectedQuestions.ToList());
                }
                catch (Exception exception)
                {

                }               

                return RedirectToAction(nameof(Create));
            }

            surveyModel.SurveySelectedQuestions = GetSelectedQuestionViewModel(surveyModel.OriginalSurveySelectedQuestions);           
            return View(surveyModel);
        }



        private IEnumerable<QuestionViewModel> GetSelectedQuestionViewModel(IEnumerable<int> selectedQuestionIds)
        {
            //récupérer la liste des questions selectionnés
            var questions = _questionDao.GetAll().Where(q => selectedQuestionIds.Contains(q.Id));
            var questionListViewModels = new List<QuestionViewModel>();
            var answers = _answerDao.GetAll();

            if (questions != null && questions.Any())
            {
                foreach (var question in questions)
                {
                    questionListViewModels.Add(
                        new QuestionViewModel()
                        {
                            Id = question.Id,
                            CategoryName = question.Category.Name,
                            TypeName = question.Type.Name,
                            DifficultyLevel = question.Difficulty.DifficultyLevel,
                            Content = question.Content,
                            QuestionAnswerList = answers.Where(r => r.QuestionId == question.Id).Select(r => new CreateOrUpdateQuestionAnswerViewModel
                            {
                                Id = r.Id,
                                QuestionId = r.QuestionId,
                                QuestionContent = question.Content,
                                AnswerContent = r.Content
                            }).ToList()
                        });
                }
            }
            return questionListViewModels;
        }



        //methode permettant de filtrer les questions en fonction des preferences (SurveyFilterInfo)
        [HttpPost]
        public IActionResult FilterQuestions(SurveyFilterInfoViewModel surveyFilterInfo)
        {
            var model = BuildSurveyViewModel(surveyFilterInfo);

            return View("Create", model);

        }



        public IActionResult SurveyList()
        {
            var surveys = _surveyDao.GetAllSurvey();

            var surveyListViewModels = new List<SurveyViewModel>();
            if (surveys != null && surveys.Any())
            {
                foreach (var survey in surveys)
                {
                    surveyListViewModels.Add(
                        new SurveyViewModel()
                        {
                            id = survey.Id,
                            name = survey.Name,
                            CreationDate = survey.CreationDate,
                            surveyEvaluation = survey.SurveyEvaluation
                        }
                        );
                }

            }

            return View(surveyListViewModels);
        }




        public IActionResult Delete(int s)
        {

            _surveyDao.DeleteSurvey(s);

            return RedirectToAction("SurveyList");
        }

        public IActionResult ConfirmDelete(int surveyId)
        {
            var survey = _surveyDao.GetAllSurvey().FirstOrDefault(s => s.Id == surveyId);
            var model = new SurveyViewModel()
            {
                id = survey.Id,
                name = survey.Name,
                CreationDate = survey.CreationDate,
                surveyEvaluation = survey.SurveyEvaluation
            };
            return View("confirmDelete", model);
        }

        public IActionResult ConsultSurveyDetails(int surveyId)
        {
            var getSurveyInfo = _surveyDao.SelectSurveyInfo(surveyId);
            var getSurveyQuestionLists = _survey_Question.GetSurvey_Questions(surveyId);
            var questiondblists = new List<QuestionViewModel>();
            foreach (var getSurveyQuestionList in getSurveyQuestionLists)
            {
                var QuestionAnswerLists = new List<CreateOrUpdateQuestionAnswerViewModel>();

                foreach (var QuestionAnswerList in QuestionAnswerLists)
                {
                    QuestionAnswerLists.Add(
                        new CreateOrUpdateQuestionAnswerViewModel()
                        {
                            QuestionId = getSurveyQuestionList.Id,
                            QuestionContent = getSurveyQuestionList.Content,
                            //AnswerContent = getSurveyQuestionList.Answer.Content
                        });
                };
                questiondblists.Add(
                        new QuestionViewModel()
                        {
                            Id = getSurveyQuestionList.Id,
                            Content = getSurveyQuestionList.Content,
                            QuestionTypeName = getSurveyQuestionList.Type.Name,
                            CategoryName = getSurveyQuestionList.Category.Name,
                            QuestionAnswerList = QuestionAnswerLists
                        });
            };
            var model = new SurveyDetailViewModel()
            {
                Name = getSurveyInfo.Name,
                DateCreation = getSurveyInfo.CreationDate,
                SurveyEvaluation = getSurveyInfo.SurveyEvaluation,
                SurveySelectedQuestions = questiondblists
            };

            return View(model);
        }


        //cette methode prend en parametre le filtre de l'utilisateur et retourne le model associé
        private CreateSurveyViewModel BuildSurveyViewModel(SurveyFilterInfoViewModel surveyFilterInfo = null)
        {
            var model = surveyFilterInfo == null ? new CreateSurveyViewModel()
            {
                CategoryId = CATEGORY_ID,
                DifficultyId = DIFFICULTY_ID,
                TypeId = TYPE_ID
            } :
            new CreateSurveyViewModel()
            {
                CategoryId = surveyFilterInfo.CategoryId,
                DifficultyId = surveyFilterInfo.DifficultyId,
                TypeId = surveyFilterInfo.TypeId
            };

            //dans le selectlistItem nous avons  
            model.Categories = _categoryDao.GetAllQuestionCategory().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString(),
                // Put default categoryId qs const
                Selected = o.Id == model.CategoryId
            });
            model.Difficulties = _difficultyDao.GetAll().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.DifficultyLevel,
                Value = o.Id.ToString(),
                Selected = o.Id == model.DifficultyId
            });
            model.QuestionTypes = _questionTypeDao.GetAll().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString(),
                Selected = o.Id == model.TypeId
            });

            //on recupère les questions depuis la base de donnée en fonction des différents paramètres
            var questions = _questionDao.GetAll();
            var questionListViewModels = new List<QuestionViewModel>();
            if (questions != null && questions.Any())
            {
                foreach (var question in questions.Where(q => surveyFilterInfo == null || (
                                                                  (surveyFilterInfo.CategoryId == 0 || q.Category.Id == surveyFilterInfo.CategoryId)
                                                               && (surveyFilterInfo.TypeId == 0 || q.Type.Id == surveyFilterInfo.TypeId)
                                                               && (surveyFilterInfo.DifficultyId == 0 || q.Difficulty.Id == surveyFilterInfo.DifficultyId)
                                                               )))
                {
                    questionListViewModels.Add(
                        new QuestionViewModel()
                        {
                            Id = question.Id,
                            CategoryName = question.Category.Name,
                            TypeName = question.Type.Name,
                            DifficultyLevel = question.Difficulty.DifficultyLevel,
                            Content = question.Content,
                            IsChecked = surveyFilterInfo != null && surveyFilterInfo.SurveySelectedQuestions != null && surveyFilterInfo.SurveySelectedQuestions.Any()
                            && surveyFilterInfo.SurveySelectedQuestions.Any(sq => sq.Id == question.Id && sq.IsChecked)
                        });
                }
            }

            model.SurveySelectedQuestions = questionListViewModels;

            return model;
        }

        // methode permettant de recupérer les données formulaires depuis la base.

    }
}













