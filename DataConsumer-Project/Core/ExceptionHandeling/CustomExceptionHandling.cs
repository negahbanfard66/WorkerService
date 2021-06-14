using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer_Project.Core.Middleware
{
    public class CustomExceptionHandling : Exception
    {
        readonly Serilog.ILogger _log = Log.ForContext<CustomExceptionHandling>();
        public CustomExceptionHandling(int code, string messages, string innerMessage)
        {
            Code = code;
            Messages = messages;
            InnerMessage = innerMessage;

            if (string.IsNullOrEmpty(InnerMessage))
                _log.Error($"Exception raised at {DateTime.Now}. Exception Message is {Messages} .");
            else
                _log.Error($"Exception raised at {DateTime.Now}. Exception Message is {Messages} . Exception innerMessage is {InnerMessage} .");
        }

        public int Code { get; }
        public string Messages { get; }
        public string InnerMessage { get; }
    }
}
