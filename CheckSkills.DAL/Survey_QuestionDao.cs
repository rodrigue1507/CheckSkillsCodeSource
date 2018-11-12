using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using CheckSkills.Domain.Entities;
using System.Linq;
//using CheckSkills.WebSite.ViewModels.Question;


namespace CheckSkills.DAL
{
    public class Survey_QuestionDao : ISurvey_QuestionDao
    {

        private IQuestionDao _questionDao;
        private IAnswerDao _answerDao;
        public Survey_QuestionDao()
        {
            _answerDao = new AnswerDao();
            _questionDao = new QuestionDao(_answerDao);
        }

        //chaine de connexion base de donnée
        private readonly string _connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public IEnumerable<Survey> GetAllSurvey()
        {
            // Use ADO.Net to DB access
            var surveys = new List<Survey>();
            try
            {
                //objet de connection 
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Do work here
                    connection.Open();
                    //objet permettant de faire des requêtes SQL
                    var scriptGetAllQuestion = @"
                                                SELECT 
	                                                Id,
	                                                Name,
                                                FROM Survey";

                    var sqlCommand = new SqlCommand(scriptGetAllQuestion, connection);
                    //recupère les données dans la resultReader
                    var resultReader = sqlCommand.ExecuteReader(); //lecture et stockage du resultat dans resultReader

                    //parse ResultReader 
                    while (resultReader.Read())
                    {
                        var s = new Survey()
                        {
                            Id = Convert.ToInt32(resultReader["Id"]),//recupère l'id de question dans la database et le converti
                            Name = resultReader["Name"].ToString(),
                        };
                        surveys.Add(s);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }
            return surveys;
        }

        public void DeleteQuestionsSurvey( int questionId)
        {
             using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "DELETE FROM [CheckSkills].[dbo].[Survey_Question] WHERE QuestionId = @questionId ", // stock la requete sql dans commandText.
                    Connection = sqlConnection1, // etablie la connection.
                };
                    // permet de definir les variables values dans CommandText.
                    cmd.Parameters.AddWithValue("@questionId", questionId);
               

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteQuestionSurvey(int questionId, int surveyId)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "DELETE FROM [CheckSkills].[dbo].[Survey_Question] WHERE QuestionId = @questionId  AND SurveyId = @surveyId ", // stock la requete sql dans commandText.
                    Connection = sqlConnection1, // etablie la connection.
                };
                // permet de definir les variables values dans CommandText.
                cmd.Parameters.AddWithValue("@questionId", questionId);
                cmd.Parameters.AddWithValue("@surveyId", surveyId);

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.

                cmd.ExecuteNonQuery();
            }

        }


        public IEnumerable<Question> GetSurvey_Questions(int surveyId)
        {
            IEnumerable<Question> questions = new List<Question>();
            IEnumerable<Answer> answers = new List<Answer>();


            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                //ouvre la connection à la base de donnée.


                var cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "SELECT QuestionId FROM Survey_Question WHERE SurveyId = @surveyId;", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                    Connection = sqlConnection1, // etablie la connection.
                };

                // permet de definir les variables values dans CommandText. 
                cmd.Parameters.AddWithValue("@SurveyId", surveyId);
                sqlConnection1.Open();

                var result = cmd.ExecuteReader(); // execute la requete et return l'element de la première ligne à la première colonne
                var survey_questions = new List<Survey_Question>();
                while (result.Read())
                {
                    var survey_question = new Survey_Question()
                    {
                        SurveyId = surveyId,
                        QuestionId = Convert.ToInt32(result["QuestionId"])
                    };
                    survey_questions.Add(survey_question);
                }
                if (survey_questions.Any())
                {
                    questions = _questionDao.GetAll()?.Where(q => survey_questions.Any(sq => sq.QuestionId == q.Id));
                }

            }
            return questions;
        }


        //public IEnumerable<Question> GetSurveyId(int questionId)
        //{
           


        //    using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
        //    {
        //        //ouvre la connection à la base de donnée.


        //        var cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
        //        {
        //            CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
        //            CommandText = "SELECT SurveyId FROM Survey_Question WHERE QuestionId = @questionId;", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
        //            Connection = sqlConnection1, // etablie la connection.
        //        };

        //        // permet de definir les variables values dans CommandText. 
        //        cmd.Parameters.AddWithValue("@questionId", questionId);
        //        sqlConnection1.Open();

        //        var result = cmd.ExecuteReader(); // execute la requete et return l'element de la première ligne à la première colonne

        //        return questionId;
        //}

    }

}
