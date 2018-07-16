using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CloudCantiere.Models;
using Dapper;

namespace CloudCantiere.DataAccess.Cantieri
{
    public class CantiereRepository : ICantiereRepository
    {
        private string _connectionString;
        public CantiereRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cantiere> Get()
        {
            throw new NotImplementedException();
        }

        public Cantiere Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(Cantiere value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO Cantieri
                                (Customer
                                ,Location
                                ,Email)
                         VALUES
                               (@Customer
                               ,@Location
                               ,@Email)
                        SELECT SCOPE_IDENTITY()";
                return connection.QueryFirstOrDefault<int>(query, value);
            }
        }

        public void Update(Cantiere value)
        {
            throw new NotImplementedException();
        }
    }
}
