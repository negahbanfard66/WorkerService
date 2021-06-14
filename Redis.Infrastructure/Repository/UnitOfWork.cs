using Data.Repository.Interfaces;

namespace Redis.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IRedisStudentRepository redisStudentsRepository)
        {
            RedisStudents = redisStudentsRepository;
        }

        public IStudentRepository Students { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IRedisStudentRepository RedisStudents { get; set; }
    }
}
