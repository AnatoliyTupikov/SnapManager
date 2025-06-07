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

        public DBConfigurationException(string message, int ErrorCode) : base(message) 
        {
            this.ErrorCode = ErrorCode;
        }
    }
}
