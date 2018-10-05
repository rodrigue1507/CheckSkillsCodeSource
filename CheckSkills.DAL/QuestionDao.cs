using CheckSkills.Domain.Entities;
using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace CheckSkills.DAL
{
    public  class QuestionDao : IQuestionDao
    {
        private IAnswerDao _answerDao;
        public QuestionDao(IAnswerDao answerDao)
        {
            _answerDao = answerDao;
        }

        //chaine de connexion base de donnée
        private string _connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public string Name { get; private set; }

        public IEnumerable<Question> GetAll()
        {

            // Use ADO.Net to DB access
            var questions = new List<Question>();

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
	                                                q.Id,
	                                                q.QuestionCategoryId,
	                                                q.QuestionDifficultyId,
	                                                q.QuestionTypeId,
	                                                q.Content,
	                                                c.Name AS QuestionCategoryName,
	                                                d.QuestionDifficultyLevel ,
	                                                qt.Name AS QuestionTypeName
                                                FROM Question q
                                                INNER JOIN QuestionDifficulty d ON q.QuestionDifficultyId = d.Id
                                                INNER JOIN QuestionType qt ON q.QuestionTypeId = qt.Id
                                                INNER JOIN QuestionCategory c ON q.QuestionCategoryId = c.Id";

                    var sqlCommand = new SqlCommand(scriptGetAllQuestion, connection);
                    //recupère les données dans la resultReader
                    var resultReader = sqlCommand.ExecuteReader(); //lecture et stockage du resultat dans resultReader

                    //parse ResultReader 
                    var answers = _answerDao.GetAll();
                    while (resultReader.Read())
                    {
                        var question = new Question()
                        {
                            Id = Convert.ToInt32(resultReader["Id"]),//recupère l'id de question dans la database et le converti
                            Category = new QuestionCategory()
                            {
                                Id = Convert.ToInt32(resultReader["QuestionCategoryId"]),
                                Name = resultReader["QuestionCategoryName"].ToString()
                            },
                            Content = resultReader["Content"].ToString(),

                            Difficulty = new QuestionDifficulty()
                            {
                                Id = Convert.ToInt32(resultReader["QuestionDifficultyId"]),
                                DifficultyLevel = resultReader["QuestionDifficultyLevel"].ToString()
                            },
                            Type = new QuestionType()
                            {
                                Id = Convert.ToInt32(resultReader["QuestionTypeId"]),
                                Name = resultReader["QuestionTypeName"].ToString()
                            },

                        };
                        question.Answers = answers?.Where(a => a.QuestionId == question.Id);
                        questions.Add(question);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }
            return questions;
        }


        public  Question GetBydId(int questionId)
        {
            // Use ADO.Net to DB access


            //objet de connection 
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString))
            {
                // Do work here
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = @"
                                    SELECT 
	                                    q.Id,
	                                    q.QuestionCategoryId,
	                                    q.QuestionDifficultyId,
	                                    q.QuestionTypeId,
	                                    q.Content,
	                                    c.Name AS QuestionCategoryName,
	                                    d.QuestionDifficultyLevel ,
	                                    qt.Name AS QuestionTypeName,
                                        a.content AS AnwserContent
                                    FROM Question q
                                    INNER JOIN QuestionDifficulty d ON q.QuestionDifficultyId = d.Id
                                    INNER JOIN QuestionType qt ON q.QuestionTypeId = qt.Id
                                    INNER JOIN QuestionCategory c ON q.QuestionCategoryId = c.Id
                                    INNER JOIN Answer a ON q.Id = a.QuestionId
                                    WHERE q.Id = @questionId",
                    Connection = sqlConnection1, // etablie la connection.
                };
                cmd.Parameters.AddWithValue("@questionId", questionId);
                sqlConnection1.Open();

                var result = cmd.ExecuteReader();

                result.Read();

                var question = new Question()
                {
                    Id = Convert.ToInt32(result["Id"]),//recupère l'id de question dans la database et le converti
                    Category = new QuestionCategory()
                    {
                        Id = Convert.ToInt32(result["QuestionCategoryId"]),
                        Name = result["QuestionCategoryName"].ToString()
                    },
                    Content = result["Content"].ToString(),

                    Difficulty = new QuestionDifficulty()
                    {
                        Id = Convert.ToInt32(result["QuestionDifficultyId"]),
                        DifficultyLevel = result["QuestionDifficultyLevel"].ToString()
                    },
                    Type = new QuestionType()
                    {
                        Id = Convert.ToInt32(result["QuestionTypeId"]),
                        Name = result["QuestionTypeName"].ToString()
                    }
                };
                question.Answers = _answerDao.GetByQuestionId(question.Id);
                return question;
            }
        }
        

        //methode permettant d'insérer une question dans la base de donnée
        public int CreateQuestion(Question q)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "INSERT Question (QuestionCategoryId, QuestionDifficultyId, QuestionTypeId,Content) VALUES (@CategoryId,@DifficultyId, @QuestionTypeID,@Content); SELECT SCOPE_IDENTITY();", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                    Connection = sqlConnection1, // etablie la connection.
                };


                // permet de definir les variables values dans CommandText. 
                cmd.Parameters.AddWithValue("@CategoryId", q.Category.Id);
                cmd.Parameters.AddWithValue("@DifficultyId", q.Difficulty.Id);
                cmd.Parameters.AddWithValue("@QuestionTypeID", q.Type.Id);
                cmd.Parameters.AddWithValue("@Content", q.Content);

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.

                var result = cmd.ExecuteScalar(); // execute la requete et return l'element de la première ligne à la première colonne

                if (result != null && int.TryParse(result.ToString(), out var questionId)) // convertit result.ToString() en int et le stock dans questionId
                {
                    return questionId;
                }

                return 0;
            }


        }

        public int UpdateQuestion(Question q)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "UPDATE Question SET QuestionCategoryId = @CategoryId , QuestionDifficultyId = @DifficultyId, QuestionTypeId = @QuestionTypeID,Content=@Content WHERE Id = @QuestionId", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                    Connection = sqlConnection1, // etablie la connection.
                };


                // permet de definir les variables values dans CommandText.
                cmd.Parameters.AddWithValue("@QuestionId", q.Id);
                cmd.Parameters.AddWithValue("@CategoryId", q.Category.Id);
                cmd.Parameters.AddWithValue("@DifficultyId", q.Difficulty.Id);
                cmd.Parameters.AddWithValue("@QuestionTypeID", q.Type.Id);
                cmd.Parameters.AddWithValue("@Content", q.Content);

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.

                var result = cmd.ExecuteNonQuery(); // execute et retoune la premier ligne
                if (result > 0) 
                {
                    return q.Id;
                }

                return 0;
            }
        }


        public void DeleteQuestion(int questionId)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "DELETE FROM Question WHERE Id = @QuestionId", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                    Connection = sqlConnection1, // etablie la connection.
                };


                // permet de definir les variables values dans CommandText.
                cmd.Parameters.AddWithValue("@QuestionId", questionId);

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.
 
                cmd.ExecuteNonQuery();
            }
        }


    }
}
