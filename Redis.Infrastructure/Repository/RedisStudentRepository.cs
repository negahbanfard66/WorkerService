using Common.ObjectConvertor;
using Data.Entity.Entities;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redis.Infrastructure.Repository
{
    public class RedisStudentRepository : IRedisStudentRepository
    {
        private readonly IDistributedCache _cache;

        public RedisStudentRepository(IDistributedCache cache)
        {
            _cache = cache;
        }
        public int Add(Student entity)
        {
            var serializer = JsonConvert.SerializeObject(entity);
            var byteConvertor = Serialization.ObjectToByteArray(serializer);
            _cache.Set("RedisCache", byteConvertor);
            return 1;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
