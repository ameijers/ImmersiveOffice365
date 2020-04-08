using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub.Data
{ 
    public class TeamMemberRequest
    {
        public PersonEntity person = null;
        public TeamEntity team = null;
        public TeamMemberRole role = TeamMemberRole.Member;

        public ServiceException expection = new ServiceException();
    }
}
