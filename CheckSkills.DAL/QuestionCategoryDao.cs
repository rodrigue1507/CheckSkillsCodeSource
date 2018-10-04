using CheckSkills.Domain;
using CheckSkills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CheckSkills.DAL
{
    public class QuestionCategoryDao : IQuestionCategoryDao
    {
        //chaine de connexion base de donnée
        private readonly string _connectionString = @"Data Source=SGEW0481\FORMULAIRE;Initial Catalog=CheckSkills;Integrated Security=true";

        public string Name { get; private set; }

        public IEnumerable<QuestionCategory> GetAll()
        {

            // Use ADO.Net to DB access
            var categories = new List<QuestionCategory>();

            try
            {
                //objet de connection 
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Do work here
                    connection.Open();
                    //objet permettant de faire des requêtes SQL
                    var scriptGetAllCategory = @"
                                                SELECT 
	                                                c.Id,
	                                                c.Name 
                                                    FROM QuestionCategory c
                                                            ";

                    var sqlCommand = new SqlCommand(scriptGetAllCategory, connection);
                    //recupère les données dans la resultReader
                    var resultReader = sqlCommand.ExecuteReader(); //lecture et stockage du resultat dans resultReader
                    //parse ResultReader 
                    while (resultReader.Read())
                    {
                        var Category = new QuestionCategory()
                        {
                            Id = Convert.ToInt32(resultReader["Id"]),//recupère l'id de Category dans la database et le convertir
                            Name = resultReader["Name"].ToString(),
                        };
                        categories.Add(Category);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }

            return categories;
        }

        public IEnumerable<QuestionCategory> GetAllQuestionCategory()
        {
            return GetAll();
        }
    }

}
