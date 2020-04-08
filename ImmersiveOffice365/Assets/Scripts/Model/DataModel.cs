using Office365DataHub.Data;
using Office365DataHub.Services;
using Office365DataHub.Entities;
public class DataModel : IDataModel
{
    private DataQueue queue = new DataQueue();

    public DataQueue Queue
    { 
        get
        {
            return queue;
        }
    }

    public void OnGetRelatedPersonCompleted(RelatedPeopleRequest request)
    {
    }

    public void OnGetRelatedPeopleCompleted(RelatedPeopleRequest request)
    {
    }

    public void OnGetPersonCompleted(PersonRequest request)
    {
        RelatedPeopleRequest relrequest = new RelatedPeopleRequest();
        relrequest.person = request.person;

        PeopleService.Instance.GetRelatedPeople(relrequest, OnGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted);
    }

    public void LoadData()
    {
        PeopleService.Instance.GetCurrentUser(OnGetPersonCompleted);
    }
}
