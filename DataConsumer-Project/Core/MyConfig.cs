using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer_Project.Core
{
    

    
    public class MyConfig
    {
        private readonly IConfiguration _Configuration;
        public MyConfig(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public Logging logging { get; set; }
        public RabbitMqSettings rabbitMqSettings { get; set; }

    }



    public class Logging
    {
        public LogLevel logLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
    }

    public class RabbitMqSettings
    {
        public string Connection { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
