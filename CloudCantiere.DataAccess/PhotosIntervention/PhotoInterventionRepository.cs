using CloudCantiere.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CloudCantiere.DataAccess.PhotosIntervention
{
    public class PhotoInterventionRepository : IPhotoInterventionRepository
    {
        private string _connectionString;
        public PhotoInterventionRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhotoIntervention> Get()
        {
            throw new NotImplementedException();
        }

        public PhotoIntervention Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(PhotoIntervention value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO InterventionsPhotos
                                (IdIntervention
                                ,URI)
                         VALUES
                               (@IdIntervention
                               ,@URI)
                        SELECT SCOPE_IDENTITY()";
                return connection.QueryFirstOrDefault<int>(query, value);
            }
        }

        public void Update(PhotoIntervention value)
        {
            throw new NotImplementedException();
        }

    }
}
