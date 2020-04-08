using System.Collections.Generic;

namespace Office365DataHub.Entities
{
    public class PersonRequest
    {
        public string id = "";
        public PersonEntity person = null;

        public ServiceException expection = new ServiceException();
    }
}
