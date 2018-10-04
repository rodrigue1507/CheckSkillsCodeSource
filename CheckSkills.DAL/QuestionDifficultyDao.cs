
using CheckSkills.Domain.Entities;
using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace CheckSkills.DAL
{
    public class QuestionDifficultyDao : IQuestionDifficultyDao
    {
        //chaine de connexion base de donnée
        private string connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public string Name { get; private set; }

        public IEnumerable<QuestionDifficulty> GetAll()
        {

            // Use ADO.Net to DB access
            var Difficulties = new List<QuestionDifficulty>();

            try
            {
                //objet de connection 
                using (var connection = new SqlConnection(connectionString))
                {
                    // Do work here
                    connection.Open();
                    //objet permettant de faire des requêtes SQL
                    var scriptGetAllDifficulty = @"
                                                SELECT 
	                                                d.Id,
                                                    d.QuestionDifficultyLevel
	                                                FROM QuestionDifficulty d
                                                ";

                    var sqlCommand = new SqlCommand(scriptGetAllDifficulty, connection);
                    //recupère les données dans la resultReader
                    var resultReader = sqlCommand.ExecuteReader(); //lecture et stockage du resultat dans resultReader
                    //parse ResultReader 
                    while (resultReader.Read())
                    {
                        var Difficulty = new QuestionDifficulty()
                        {
                            Id = Convert.ToInt32(resultReader["Id"]),//recupère l'id de Difficulty dans la database et le converti

                            DifficultyLevel = resultReader["QuestionDifficultyLevel"].ToString(),
                        };
                        Difficulties.Add(Difficulty);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }

            return Difficulties;
        }
    }
}

