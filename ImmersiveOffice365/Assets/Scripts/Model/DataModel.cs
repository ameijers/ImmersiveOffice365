using Office365DataHub.Data;
using Office365DataHub.Services;
using Office365DataHub.Entities;
public class DataModel : IDataModel
{
    private DataQueue queue = new DataQueue();

    private bool error = false;

    public DataQueue Queue
    { 
        get
        {
            return queue;
        }
    }

    private void InitializeConnection()
    {
        Office365DataHub.AuthenticationHelper.Instance.UseBetaAPI = false;
        Office365DataHub.AuthenticationHelper.Instance.UserLogon = true;
        Office365DataHub.AuthenticationHelper.Instance.RedirectUri = "https://www.appzinside.com";
        Office365DataHub.AuthenticationHelper.Instance.Authentication = new Office365DataHub.Authentication
        {
            ClientId = "8924f069-832c-40c0-bcb2-95f9ab328bd7",
            Scopes = new string[] { "User.Read", "User.Read.All", "People.Read", "People.Read.All" },
        };
    }

    public void OnGetRelatedPersonCompleted(RelatedPeopleRequest request)
    {
        Queue.AddToQueue(request.person, request.relatedPerson);
    }

    public void OnGetRelatedPeopleCompleted(RelatedPeopleRequest request)
    {
    }

    public void OnGetPersonCompleted(PersonRequest request)
    {
        error = true;

        if (request.expection.Error != Office365DataHub.ServiceError.NoError)
        {
            DebugInformation.Instance.Log(request.expection.Exception.Message);
            DebugInformation.Instance.Log(request.expection.Exception.StackTrace);
            DebugInformation.Instance.Log(request.expection.Exception.InnerException.Message);
        }

        Queue.AddToQueue(null, request.person);

        RelatedPeopleRequest relrequest = new RelatedPeopleRequest
        {
            person = request.person
        };

        Office365DataHub.Services.PeopleService.Instance.GetRelatedPeople(relrequest, OnGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted);
    }
    private void CreateSampleData()
    {
        PersonEntity p1 = new PersonEntity { Id = "1", FullName = "Alexander" };
        PersonEntity p2 = new PersonEntity { Id = "2", FullName = "Colin" };
        PersonEntity p3 = new PersonEntity { Id = "3", FullName = "Owen" };
        PersonEntity p4 = new PersonEntity { Id = "4", FullName = "Tessa" };
        PersonEntity p5 = new PersonEntity { Id = "5", FullName = "Terry" };
        PersonEntity p6 = new PersonEntity { Id = "6", FullName = "Micheal" };
        PersonEntity p7 = new PersonEntity { Id = "7", FullName = "Jordy" };

        Queue.AddToQueue(null, p1);
        Queue.AddToQueue(p1, p2);
        Queue.AddToQueue(p1, p3);
        Queue.AddToQueue(p1, p4);
        Queue.AddToQueue(p3, p4);
        Queue.AddToQueue(p2, p3);
        Queue.AddToQueue(p1, p5);
        Queue.AddToQueue(p5, p6);
        Queue.AddToQueue(p6, p7);
    }


    public void LoadData()
    {
        InitializeConnection();

#if UNITY_EDITOR
        CreateSampleData();
#else
        Office365DataHub.Services.PeopleService.Instance.GetCurrentUser(OnGetPersonCompleted);
#endif
    }
}
