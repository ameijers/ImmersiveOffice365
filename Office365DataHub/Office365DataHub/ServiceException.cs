using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub
{
    public class ServiceException
    {
        public ServiceException()
        {
            Error = ServiceError.NoError;
            Exception = null;
        }

        public Exception Exception { get; set; }

        public ServiceError Error { get; set; }
    }
}
