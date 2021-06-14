using Data.Repository.Interfaces;

namespace Dapper.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IStudentRepository studentRepository)
        {
            Students = studentRepository;
        }

        public IStudentRepository Students { get; set; }
        public IRedisStudentRepository RedisStudents { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
