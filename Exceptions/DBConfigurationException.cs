using SnapManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Exceptions
{
    /// <summary>
    /// 110 - there is no db type
    /// 111 - specified the db type is not supproted
    /// 120 - there is no connection string
    /// </summary>
    internal class DBConfigurationException : Exception
    {
        
        public int ErrorCode { get; private set; }
        public DBConfigurationException() { }

        public DBConfigurationException(string message, int errorCode = 0, Severity severity = Severity.Error) : base(message) 
        {
            this.ErrorCode = errorCode;
            base.Data["Severity"] = severity;
        }
    }
}
