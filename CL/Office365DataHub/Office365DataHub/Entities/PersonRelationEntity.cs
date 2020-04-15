using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub.Entities
{
    public class PersonRelationEntity : BaseEntity
    {
        public string sourceId = "";
        public string targetId = "";
    }

    public class PersonRelationCollection: List<PersonRelationEntity>
    {
        public bool RelationExists(string source, string target)
        {
            foreach (PersonRelationEntity relation in this)
            {
                if ((relation.sourceId == source && relation.targetId == target) ||
                        (relation.sourceId == target && relation.targetId == source))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
