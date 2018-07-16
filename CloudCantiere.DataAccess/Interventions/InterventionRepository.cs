using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CloudCantiere.Models;
using Dapper;

namespace CloudCantiere.DataAccess.Interventions
{
    public class InterventionRepository : IInterventionRepository
    {
        private string _connectionString;
        public InterventionRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Intervention> Get()
        {
            throw new NotImplementedException();
        }

        public Intervention Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(Intervention value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO Interventions
                                (IdType
                                ,IdCantiere
                                ,Notes
                                ,Price)
                         VALUES
                               (@IdType
                               ,@IdCantiere
                               ,@Notes
                               ,@Price)
                        SELECT SCOPE_IDENTITY()";
                return connection.QueryFirstOrDefault<int>(query, value);
            }
        }

        public void Update(Intervention value)
        {
            throw new NotImplementedException();
        }
    }
}
