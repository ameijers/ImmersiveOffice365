using Office365DataHub;
using Office365DataHub.Entities;
using Office365DataHub.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Office365DataHub.Data;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Office365DataHubTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            AuthenticationHelper.Instance.UseBetaAPI = false;
            AuthenticationHelper.Instance.UserLogon = true;
            AuthenticationHelper.Instance.RedirectUri = "https://www.appzinside.com";

            AuthenticationHelper.Instance.Authentication.ClientId = "8924f069-832c-40c0-bcb2-95f9ab328bd7";
            AuthenticationHelper.Instance.Authentication.Scopes = new string[] {
                "User.Read",
                "Mail.Send",
                "People.Read",
                "People.Read.All",
                "User.Read.All",
                "Sites.Read.All",
                "Files.ReadWrite",
                "Group.Read.All",
                //"Group.ReadWrite.All",
                "Tasks.Read",
                "Calendars.Read",
                "Calendars.Read.Shared",
                "Directory.Read.All"
            };

            PeopleService.Instance.GetCurrentUser(OnGetCurrentUser);

            TeamService.Instance.GetTeam("da69c998-dcbd-4ac7-a825-ca3dda5298e9", OnGetTeamCompleted);


        }

        public void OnGetCurrentUser(PersonRequest request)
        {
            // get joined teams
            TeamDataRequest tdrequest = new TeamDataRequest();
            tdrequest.person = request.person;
            TeamService.Instance.GetJoinedTeams(tdrequest, OnGetTeams);

            // get related people
            RelatedPeopleRequest relatedRequest = new RelatedPeopleRequest();
            relatedRequest.person = request.person;
            PeopleService.Instance.GetRelatedPeople(relatedRequest, OnGetRelatedPerson, OnGetRelatedPeople);
        }

        public void OnGetTeamCompleted(TeamEntity team)
        {
            // adding Alexander prive
            TeamMemberRequest request = new TeamMemberRequest();
            request.team = team;
            request.person = new PersonEntity { Id = "700f01e1-52d4-40ff-b099-f2f02f17c39c" };

            TeamService.Instance.AddTeamMember(request, OnAddTeamMember);
        }

        public void OnAddTeamMember(TeamMemberRequest request)
        {
        }


        public void OnGetTeams(TeamDataRequest request)
        {
            foreach (TeamEntity team in request.teams)
            {
                TeamMembersDataRequest membersRequest = new TeamMembersDataRequest();
                membersRequest.team = team;
                TeamService.Instance.GetTeamMembers(membersRequest, OnGetTeamMembers);
                Debug.WriteLine(string.Format("Team:{0}, {1}", team.DisplayName, team.Id));
            }
        }

        public void OnGetTeamMembers(TeamMembersDataRequest request)
        {
            foreach (PersonEntity person in request.members)
            {
                Debug.WriteLine(string.Format("{0}:{1}", person.FullName, person.Id));
            }
        }

        void OnGetRelatedPerson(RelatedPeopleRequest request)
        {
            Debug.WriteLine(string.Format("Person {0} is related to Person {1}", request.relatedPerson.FullName, request.person.FullName));
        }

        void OnGetRelatedPeople(RelatedPeopleRequest request)
        {
            foreach (PersonEntity person in request.relatedPeople)
            {
                Debug.WriteLine(string.Format("Person {0} is related to Person {1}", person.FullName, request.person.FullName));
            }
        }
    }
}
