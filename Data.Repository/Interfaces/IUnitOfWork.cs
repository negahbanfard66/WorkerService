namespace Data.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IStudentRepository Students { get; set; }

        public IRedisStudentRepository RedisStudents { get; set; }
    }
}
