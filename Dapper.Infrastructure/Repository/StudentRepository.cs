using Data.Entity.Entities;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration configuration;
        public StudentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public int Add(Student entity)
        {
            entity.InsertDate = DateTime.Now;
            var sql = "Insert into Students (Name,Family,Tell,Address,InsertDate) VALUES (@Name,@Family,@Tell,@Address,@InsertDate)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Students WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }
        public async Task<IReadOnlyList<Student>> GetAllAsync()
        {
            var sql = "SELECT * FROM Students";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Student>(sql);
                return result.ToList();
            }
        }
        public async Task<Student> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Students WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Student>(sql, new { Id = id });
                return result;
            }
        }
        public async Task<int> UpdateAsync(Student entity)
        {
            entity.UpdateDate = DateTime.Now;
            var sql = "UPDATE Students SET Name = @Name, Family = @Family, Tell = @Tell, Address = @Address, UpdateDate= @UpdateDate  WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
