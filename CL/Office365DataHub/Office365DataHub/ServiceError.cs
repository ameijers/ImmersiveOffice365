using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub
{
    public enum ServiceError
    {
        NoError = 0,
        AuthenticationClientError = 1,
        UserAlreadyExists = 10,
        UserError = 11,
        UnknownError = 100
    }
}
