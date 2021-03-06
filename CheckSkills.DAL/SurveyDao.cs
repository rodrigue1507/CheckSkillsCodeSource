﻿using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace CheckSkills.DAL
{
    public class SurveyDao : ISurveyDao
    {
        //chaine de connexion base de donnée
        private readonly string _connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public string Name { get; private set; }



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
	                                                SurveyEvaluation,
                                                   CreationDate
                                                FROM Survey ";

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
                            SurveyEvaluation = resultReader["SurveyEvaluation"].ToString(),
                        };

                        if (DateTime.TryParse(resultReader["CreationDate"]?.ToString(), out var creationDate))
                        {
                            s.CreationDate = creationDate;
                        }
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



        public void CreateSurvey(string name, List<int> questionIds)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                sqlConnection1.Open(); //ouvre la connection à la base de donnée.
                var transaction = sqlConnection1.BeginTransaction();

                try
                {
                    var cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                    {
                        CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                        CommandText = "INSERT Survey (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                        Connection = sqlConnection1, // etablie la connection.
                        Transaction = transaction
                    };

                    // permet de definir les variables values dans CommandText. 
                    cmd.Parameters.AddWithValue("@Name", name);

                    var result = cmd.ExecuteScalar(); // execute la requete et return l'element de la première ligne à la première colonne

                    if (result != null && int.TryParse(result.ToString(), out var surveyId)) // convertit result.ToString() en int et le stock dans surveyId
                    {
                        foreach (var questionId in questionIds)
                        {
                            var cmd2 = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                            {
                                CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                                CommandText = "INSERT Survey_Question (SurveyId,QuestionId) VALUES (@SurveyId,@QuestionId);", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                                Connection = sqlConnection1, // etablie la connection.
                                Transaction = transaction
                            };

                            // permet de definir les variables values dans CommandText. 
                            cmd2.Parameters.AddWithValue("@SurveyId", surveyId);
                            cmd2.Parameters.AddWithValue("@QuestionId", questionId);

                            cmd2.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }





        public int UpdateSurvey(Survey q)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "UPDATE Question SET Name = @Name, @SurveyEvaluation = @SurveyEvaluation, WHERE Id = @Id", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                    Connection = sqlConnection1, // etablie la connection.

                };


                // permet de definir les variables values dans CommandText.
                cmd.Parameters.AddWithValue("@Id", q.Id);

                cmd.Parameters.AddWithValue("@Name", q.Name);
                cmd.Parameters.AddWithValue("@SurveyEvaluation", q.SurveyEvaluation);

                sqlConnection1.Open(); //ouvre la connection à la base de donnée.

                var result = cmd.ExecuteNonQuery(); // execute et retoune la premier ligne
                if (result > 0)
                {
                    return q.Id;
                }

                return 0;
            }
        }

        public void DeleteSurvey(int surveyId)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                sqlConnection1.Open();
                var transaction = sqlConnection1.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                    {
                        CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                        CommandText = "DELETE FROM Survey WHERE Id = @Id", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                        Connection = sqlConnection1, // etablie la connection.
                        Transaction = transaction
                    };

                    //ouvre la connection à la base de donnée.
                                           // permet de definir les variables values dans CommandText.
                    cmd.Parameters.AddWithValue("@Id", surveyId);

                    var result = cmd.ExecuteNonQuery();

                    if (result > null && int.TryParse(result.ToString(), out var s)) // convertit result.ToString() en int et le stock dans s
                    {
                       
                            var cmd2 = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                            {
                                CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                                CommandText = "DELETE FROM Survey_Question WHERE SurveyId = @sId;", // stock la requete sql dans commandText. SCOPE_IDENTITY renvoie l'Id de  la question inseré.
                                Connection = sqlConnection1, // etablie la connection.
                                Transaction = transaction
                            };

                            // permet de definir les variables values dans CommandText. 
                            cmd2.Parameters.AddWithValue("@sId", surveyId);

                            cmd2.ExecuteNonQuery();
                        
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }



        public Survey SelectSurveyInfo(int surveyId)
        {
            using (SqlConnection sqlConnection1 = new SqlConnection(_connectionString)) // using permet de refermer la connection après ouverture
            {
                SqlCommand cmd = new SqlCommand  // objet cmd me permet d'exécuter des requêtes SQL
                {
                    CommandType = CommandType.Text, // methode permettant de definir le type de commande (text = une commande sql; Storeprocedure= le nom de la procedure stockée; TableDirect= le nom d'une table.
                    CommandText = "SELECT Name, SurveyEvaluation, CreationDate FROM Survey Where Id = @Id",
                    Connection = sqlConnection1, // etablie la connection.
                };

                sqlConnection1.Open();

                cmd.Parameters.AddWithValue("@Id", surveyId);

                var resultReader = cmd.ExecuteReader();
                resultReader.Read();
                var s = new Survey()
                {
                    Name = resultReader["Name"].ToString(),
                    SurveyEvaluation = resultReader["SurveyEvaluation"]?.ToString(),
                };
                if (DateTime.TryParse(resultReader["CreationDate"]?.ToString(), out var creationDate))
                {
                    s.CreationDate = creationDate;
                }

                return s;

            }
        }
    }
}