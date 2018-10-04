using CheckSkills.Domain;
using CheckSkills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CheckSkills.DAL
{
    public class QuestionTypeDao : IQuestionTypeDao
    {
        //chaine de connexion base de donnée
        private readonly string _connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public string Name { get; private set; }

        public IEnumerable<QuestionType> GetAll()
        {

            // Use ADO.Net to DB access
            var questionTypes = new List<QuestionType>();

            try
            {
                //objet de connection 
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Do work here
                    connection.Open();
                    //objet permettant de faire des requêtes SQL
                    var scriptGetAllQuestionType = @"
                                                SELECT 
	                                                qt.Id,
	                                                qt.Name
                                                    FROM QuestionType qt
                                                            ";

                    var sqlCommand = new SqlCommand(scriptGetAllQuestionType, connection);
                    //recupère les données dans la resultReader
                    var resultReader = sqlCommand.ExecuteReader(); //lecture et stockage du resultat dans resultReader
                    //parse ResultReader 
                    while (resultReader.Read())
                    {
                        var QuestionType = new QuestionType()
                        {
                            Id = Convert.ToInt32(resultReader["Id"]),//recupère l'id de QuestionType dans la database et le converti
                            Name = resultReader["Name"].ToString(),
                        };
                        questionTypes.Add(QuestionType);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }

            return questionTypes;

        } 
    }
}
