using Microsoft.Graph;
using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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
            System.Threading.Tasks.Task.Run(
                () => AddTeamMemberAsync(request, onAddTeamMemberCompleted) );
        }

        public async Task AddTeamMemberAsync(TeamMemberRequest request, OnAddTeamMemberCompleted onAddTeamMemberCompleted)
        {
            GraphServiceClient graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            if (graphClient != null)
            {
                User user = await graphClient.Users[request.person.Id].Request().GetAsync();

                try
                {
                    switch(request.role)
                    {
                        case TeamMemberRole.Owner:
                            await graphClient.Groups[request.team.Id].Owners.References.Request().AddAsync(user);
                            break;

                        default:
                            await graphClient.Groups[request.team.Id].Members.References.Request().AddAsync(user);
                           break;
                    }
                }
                catch (Exception ex)
                {
                    request.expection = new ServiceException
                    {
                        Error = ServiceError.UserAlreadyExists,
                        Exception = ex
                    };
                }
            }

            onAddTeamMemberCompleted(request);
        }


        public void GetTeamMembers(TeamMembersDataRequest request, OnGetTeamMembersCompleted onGetTeamMembersCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetTeamMembersAsync(request, onGetTeamMembersCompleted) );
        }

        public async Task GetTeamMembersAsync(TeamMembersDataRequest request, OnGetTeamMembersCompleted onGetTeamMembersCompleted)
        {
            GraphServiceClient graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            if (graphClient != null)
            {
                PersonEntityCollection persons = new PersonEntityCollection();

                var teamMembers = await graphClient.Groups[request.team.Id].Members.Request().GetAsync();

                foreach (var teamMember in teamMembers)
                {
                    User user = await graphClient.Users[teamMember.Id].Request().GetAsync();

                    request.members.Add(new PersonEntity
                    {
                        FullName = user.DisplayName,
                        Surname = user.Surname,
                        GivenName = user.GivenName,
                        JobTitle = user.JobTitle,
                        Department = user.Department,
                        OfficeLocation = user.OfficeLocation,
                        PhoneNumber = user.MobilePhone,
                        EmailAddress = user.Mail,
                        Id = user.Id,
                        PhotoDetail = await PeopleService.Instance.GetPhotoAsync(user.Id)
                    });
                }

                onGetTeamMembersCompleted(request);
            }
        }

        public void GetJoinedTeams(TeamDataRequest request, OnGetTeamsCompleted onGetTeamsCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetJoinedTeamsAsync(request, onGetTeamsCompleted) );
        }

        public async Task GetJoinedTeamsAsync(TeamDataRequest request, OnGetTeamsCompleted onGetTeamsCompleted)
        {
            GraphServiceClient graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            if (graphClient != null)
            {
                TeamEntityCollection teamList = new TeamEntityCollection();

                var teams = await graphClient.Users[request.person.Id].JoinedTeams.Request().GetAsync();

                foreach (var team in teams)
                {
                    request.teams.Add(new TeamEntity
                    {
                        Id = team.Id,
                        DisplayName = team.PrimaryChannel != null ? team.PrimaryChannel.DisplayName : "Undefined",
                        Description = team.PrimaryChannel != null ? team.PrimaryChannel.Description : "",
                        IsArchived = team.IsArchived

                    });
                }

                onGetTeamsCompleted(request);
            }
        }

        public void GetTeam(string teamId, OnGetTeamCompleted onGetTeamCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetTeamAsync(teamId, onGetTeamCompleted) );
        }

        public async Task GetTeamAsync(string teamId, OnGetTeamCompleted onGetTeamCompleted)
        {
            GraphServiceClient graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            if (graphClient != null)
            {
                var group = await graphClient.Groups[teamId].Request().GetAsync();

                var team = new TeamEntity
                {
                    Id = group.Id,
                    DisplayName = group.DisplayName,
                    Description = group.Description,
                    IsArchived = group.IsArchived

                };

                onGetTeamCompleted(team);
            }
        }
    }
}
