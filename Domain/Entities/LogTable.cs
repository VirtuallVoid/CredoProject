using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LogTable : BaseEntity
    {
        public string OperationName { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public DateTime OperationDate { get; set; }

        public LogTable(string operationName, string requestJson, DateTime operationDate)
        {
            OperationName = operationName;
            RequestJson = requestJson;
            OperationDate = operationDate;
        }
    }

}
