using Office365DataHub.Entities;
using Office365DataHub.Data;

namespace Office365DataHub.Services
{
    public class TeamService : Singleton<TeamService>
    {
        public delegate void OnGetTeamCompleted(TeamEntity team);
        public delegate void OnGetTeamsCompleted(TeamDataRequest request);
        public delegate void OnGetTeamMembersCompleted(TeamMembersDataRequest request);
        public delegate void OnAddTeamMemberCompleted(TeamMemberRequest request);
        public delegate void OnRemoveTeamMemberCompleted(TeamMemberRequest request);

        public void AddTeamMember(TeamMemberRequest request, OnAddTeamMemberCompleted onAddTeamMemberCompleted)
        {
        }

        public void GetTeamMembers(TeamMembersDataRequest request, OnGetTeamMembersCompleted onGetTeamMembersCompleted)
        {
        }

        public void GetJoinedTeams(TeamDataRequest request, OnGetTeamsCompleted onGetTeamsCompleted)
        {
        }
        public void GetTeam(string teamId, OnGetTeamCompleted onGetTeamCompleted)
        {
        }

    }
}
