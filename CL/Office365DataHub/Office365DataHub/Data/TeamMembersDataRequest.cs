using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub.Data
{
    public class TeamMembersDataRequest
    {
        public TeamEntity team = null;
        public PersonEntityCollection members = new PersonEntityCollection();
    }
}
