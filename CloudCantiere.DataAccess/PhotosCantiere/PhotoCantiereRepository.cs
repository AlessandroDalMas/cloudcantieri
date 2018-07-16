using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CloudCantiere.Models;
using Dapper;

namespace CloudCantiere.DataAccess.PhotosCantiere
{
    public class PhotoCantiereRepository : IPhotoCantiereRepository
    {
        private string _connectionString;
        public PhotoCantiereRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhotoCantiere> Get()
        {
            throw new NotImplementedException();
        }

        public PhotoCantiere Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(PhotoCantiere value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO CantieriPhotos
                                (IdCantiere
                                ,URI)
                         VALUES
                               (@IdCantiere
                               ,@URI)
                        SELECT SCOPE_IDENTITY()";
                return connection.QueryFirstOrDefault<int>(query, value);
            }
        }

        public void Update(PhotoCantiere value)
        {
            throw new NotImplementedException();
        }
    }
}
