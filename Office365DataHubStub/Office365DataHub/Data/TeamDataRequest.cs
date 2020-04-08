using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub.Data
{
    public class TeamDataRequest
    {
        public TeamEntityCollection teams = new TeamEntityCollection();
        public PersonEntity person = null;
    }
}
