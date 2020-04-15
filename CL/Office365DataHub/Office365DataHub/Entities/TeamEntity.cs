using System.Collections.Generic;

namespace Office365DataHub.Entities
{
    public class TeamEntity : BaseEntity
    {
        public string DisplayName = "";
        public string Description = "";
        public bool? IsArchived = false;
    }

    public class TeamEntityCollection : List<TeamEntity>
    {
    }
}
