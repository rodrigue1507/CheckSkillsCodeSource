using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CheckSkills.WebSite.ViewModels;
using CheckSkills.DAL;
using CheckSkills.Domain;
using CheckSkills.Domain.Entities;
using CheckSkills.WebSite.ViewModels.Answer;
using CheckSkills.Domain.Constants;

namespace CheckSkills.WebSite.Controllers
{
    public class QuestionController : BaseController
    {
        //proprieté du controller question en private donc accessible que dans cette class
        private IQuestionDao _questionDao;
        private IQuestionCategoryDao _categoryDao;
        private IQuestionDifficultyDao _difficultyDao;
        private IQuestionTypeDao _questionTypeDao;
        private IAnswerDao _answerDao;
        private ISurvey_QuestionDao _survey_QuestionDao;

        private const string SuccessMessage = "La creation a bien ete effectuee";



        //constructeur de questions 
        public QuestionController(IQuestionDao questionDao)
            : this(questionDao,
                 new QuestionCategoryDao(),
                 new QuestionDifficultyDao(),
                 new QuestionTypeDao(),
                 new AnswerDao(),
                 new Survey_QuestionDao())
        {

        }

        // Use this constructor for DI after
        private QuestionController(IQuestionDao questionDao, IQuestionCategoryDao questionCategoryDao,
            IQuestionDifficultyDao questionDifficultyDao,
        IQuestionTypeDao questionTypeDao,
            IAnswerDao answerDao,
            Survey_QuestionDao survey_QuestionDao)
        {
            _questionDao = questionDao; //
            _categoryDao = questionCategoryDao;
            _difficultyDao = questionDifficultyDao;
            _questionTypeDao = questionTypeDao;
            _answerDao = answerDao;
            _survey_QuestionDao = survey_QuestionDao;
        }


        // methode retournant la liste totale de question
        [HttpGet]
        public IActionResult List()
        {

            var questions = _questionDao.GetAll();

            // Convertir en view models, 
            // en recuperant seulement les données dont la vue a besoin
            var QuestionListViewModels = new List<QuestionViewModel>();
            if (questions != null && questions.Any())
            {
                foreach (var question in questions)
                {
                    QuestionListViewModels.Add(
                        new QuestionViewModel()
                        {
                            Id = question.Id,
                            CategoryName = question.Category.Name,
                            TypeName = question.Type.Name,
                            DifficultyLevel = question.Difficulty.DifficultyLevel,
                            Content = question.Content,
                        });
                }
            }
            return View(QuestionListViewModels); // return des objets questions
        }



        [HttpPost]
        public IActionResult Create(EditQuestionViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Traitement pour sauvegarder les questions

                //transformer le viewModel en une entité Question

                var question = new Question
                {
                    Category = new QuestionCategory { Id = model.CategoryId }, //
                    Content = model.Content,
                    Difficulty = new QuestionDifficulty { Id = model.DifficultyId },
                    Type = new QuestionType { Id = model.TypeId }
                };

                var questionId = _questionDao.CreateQuestion(question);

                if (questionId > 0)
                {
                    return RedirectToAction(nameof(Details), new { questionId, created = true });
                }
                else
                {
                    ModelState.AddModelError("CategoryId", "La question n'a pas été créé");
                }
            }

            AddReferenceDataToModel(model);
            return View(model);
        }


        [HttpPost]
        public IActionResult SaveNewCreate(EditQuestionViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Traitement pour sauvegarder les questions

                //transformer le viewModel en une entité Question

                var question = new Question
                {
                    Category = new QuestionCategory { Id = model.CategoryId }, //
                    Content = model.Content,
                    Difficulty = new QuestionDifficulty { Id = model.DifficultyId },
                    Type = new QuestionType { Id = model.TypeId }
                };

                var questionId = _questionDao.CreateQuestion(question);

                if (questionId > 0)
                {
                    return RedirectToAction(nameof(Create), new { created = true });
                }
                else
                {
                    ModelState.AddModelError("CategoryId", "La question n'a pas été créé");
                }
            }
            Fail = "Echec de creation de question car un des éléments sous dessous n'a pas été precisé";
            return RedirectToAction(nameof(Create));

        }



        [HttpGet]
        public IActionResult Create(bool created = false)
        {
            var model = new EditQuestionViewModel();

            AddReferenceDataToModel(model);

            if (created)
                ViewBag.Message = SuccessMessage;

            return View(model);
        }






        [HttpGet]
        public IActionResult Edit(int questionId)
        {
            var question = _questionDao.GetBydId(questionId);

            if (question != null)
            {
                var model = new CreateOrUpdateQuestionViewModel()
                {
                    QuestionTypeEnum = (QuestionTypeEnum)question.Type.Id,
                    EditQuestionViewModel = new EditQuestionViewModel
                    {
                        Id = question.Id,
                        Content = question.Content,
                        DifficultyId = question.Difficulty.Id,
                        TypeId = question.Type.Id,

                        CategoryId = question.Category.Id
                    }
                };

                AddReferenceDataToModel(model.EditQuestionViewModel);

                if (model.QuestionTypeEnum == QuestionTypeEnum.Qcm)
                {
                    var answerDtos = _answerDao.GetAll().Where(r => r.QuestionId == questionId);
                    var Answers = answerDtos.Select(r => new CreateOrUpdateAnswerViewModel
                    {
                        Id = r.Id,
                        QuestionId = r.QuestionId,
                        AnswerContent = r.Content,
                        QuestionContent = question.Content
                    }).ToList();

                    model.EditAnswerViewModel = new EditAnswerViewModel
                    {
                        QuestionId = question.Id,
                        QuestionAnswerList = Answers
                    };
                }

                return View(model);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult EditQuestionSave(EditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Traitement pour sauvegarder les questions

                //transformer le viewModel en une entité Question

                var question = new Question
                {
                    Id = model.Id.Value,
                    Category = new QuestionCategory { Id = model.CategoryId }, //
                    Content = model.Content,
                    Difficulty = new QuestionDifficulty { Id = model.DifficultyId },
                    Type = new QuestionType { Id = model.TypeId }

                };

                var questionId = _questionDao.UpdateQuestion(question);

                if (questionId > 0)
                    return RedirectToAction("Details", new { questionId });
                else
                    ModelState.AddModelError("CategoryId", "La question n'a pas été mise à jour!");
            }


            AddReferenceDataToModel(model);

            return View("Edit", model);
        }

        public IActionResult EditAnswerSave(int answerId)
        {
            var answer = _answerDao.GetById(answerId);

            if (answer != null)
            {
                var question = _questionDao.GetBydId(answer.QuestionId);
                var AnswerViewModel = new QuestionAnswerViewModel()
                {
                    QuestionId = question.Id,
                    QuestionContent = question.Content,
                    AnswerContent = answer.Content,
                    AnswerId = answer.Id
                };
                return View("Edit", AnswerViewModel);
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Delete(int questionId)
        {
            _survey_QuestionDao.DeleteQuestionSurvey(questionId);
            _answerDao.DeleteAnswerQuestionId(questionId);
            _questionDao.DeleteQuestion(questionId);
            //_answerDao.DeleteAnswerQuestion(questionId);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult ConfirmDeleteOrNo(int questionId)
        {
            var question = _questionDao.GetAll().FirstOrDefault(q => q.Id == questionId);

            if (question != null)
            {
                var model = new ViewModels.QuestionViewModel()
                {
                    Id = question.Id,
                    Content = question.Content,
                    DifficultyLevel = question.Difficulty.DifficultyLevel,
                    CategoryName = question.Category.Name,
                    QuestionTypeName = question.Type.Name,
                };
                return View("ConfirmDeleteOrNo", model);
            }

            return RedirectToAction("DeleteError");
        }


        public IActionResult DeleteError()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Details(int questionId, bool created = false)
        {
            var question = _questionDao.GetAll().FirstOrDefault(q => q.Id == questionId);

            if (question != null)
            {
                List<CreateOrUpdateAnswerViewModel> answers = null;
                if (question.Type.Name == "QCM")
                {
                    var answerDtos = _answerDao.GetAll().Where(r => r.QuestionId == questionId);
                    answers = answerDtos.Select(r => new CreateOrUpdateAnswerViewModel
                    {
                        Id = r.Id,
                        QuestionId = r.QuestionId,
                        AnswerContent = r.Content,
                        QuestionContent = question.Content
                    }).ToList();
                }


                var model = new QuestionViewModel()
                {
                    Id = question.Id,
                    Content = question.Content,
                    DifficultyLevel = question.Difficulty.DifficultyLevel,
                    CategoryName = question.Category.Name,
                    QuestionTypeName = question.Type.Name,
                    QuestionAnswerList = answers,
                    TypeName = question.Type.Name,
                };

                if (created)
                    ViewBag.Message = SuccessMessage;

                return View(model);
            }
            return RedirectToAction("List");
        }



        [HttpGet]
        public IActionResult AddQuestionAnswer(int questionId)
        {
            var question = _questionDao.GetAll().FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                var AnswerViewModel = new QuestionAnswerViewModel()
                {
                    QuestionId = question.Id,
                    QuestionContent = question.Content
                };
                return View("AddOrUpdateQuestionAnswer", AnswerViewModel);
            }

            return RedirectToAction("Details", new { questionId });
        }

        [HttpGet]
        public IActionResult EditQuestionAnswer(int AnswerId)
        {
            var answer = _answerDao.GetById(AnswerId);

            if (answer != null)
            {
                var question = _questionDao.GetBydId(answer.QuestionId);
                var answerViewModel = new QuestionAnswerViewModel()
                {
                    QuestionId = question.Id,
                    QuestionContent = question.Content,
                    AnswerContent = answer.Content,
                    AnswerId = answer.Id
                };
                return View("AddOrUpdateQuestionAnswer", answerViewModel);
            }

            return RedirectToAction("List");
        }


        [HttpPost]
        public IActionResult AddOrUpdateQuestionAnswer(QuestionAnswerViewModel answerviewModel)
        {

            var answer = new Answer
            {
                Id = answerviewModel.AnswerId ?? 0,
                QuestionId = answerviewModel.QuestionId,
                Content = answerviewModel.AnswerContent
            };

            if (answerviewModel.AnswerId.HasValue && answerviewModel.AnswerId.Value > 0)
            {
                _answerDao.UpdateAnswer(answer);
            }
            else
            {
                var AnswerId = _answerDao.CreateAnswer(answer);
            }

            return RedirectToAction("Details", new { questionId = answerviewModel.QuestionId });
        }


        [HttpGet]
        public IActionResult DeleteQuestionAnswer(int answerId)
        {
            var answer = _answerDao.GetById(answerId);
            if (answer != null)
            {
                _answerDao.DeleteAnswer(answerId);
            }
            return RedirectToAction("Details", new { questionId = answer.QuestionId });

        }



        private void AddReferenceDataToModel(EditQuestionViewModel model)
        {
            if (model == null)
                model = new EditQuestionViewModel();

            model.Categories = _categoryDao.GetAllQuestionCategory().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()
            });
            model.Difficulties = _difficultyDao.GetAll().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.DifficultyLevel,
                Value = o.Id.ToString()
            });
            model.QuestionTypes = _questionTypeDao.GetAll().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()
            });
            model.Answers = _answerDao.GetAll().Select(o => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = o.Content,
                Value = o.Id.ToString()
            });
        }


    }
}
