using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub.Data
{
    public class RelatedPeopleRequest
    {
        public PersonEntity person = null;
        public PersonEntity relatedPerson = null;
        public PersonEntityCollection relatedPeople = new PersonEntityCollection();
    }
}
