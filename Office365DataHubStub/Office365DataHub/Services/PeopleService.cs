using Office365DataHub.Data;
using Office365DataHub.Entities;

namespace Office365DataHub.Services
{
    public class PeopleService : Singleton<PeopleService>
    {
        public delegate void OnGetPersonCompleted(PersonRequest request);
        public delegate void OnGetPhotoCompleted(PhotoDetail photo);
        public delegate void OnGetRelatedPersonCompleted(RelatedPeopleRequest request);
        public delegate void OnGetRelatedPeopleCompleted(RelatedPeopleRequest request);

        public void GetCurrentUser(OnGetPersonCompleted onGetPersonCompleted)
        {
        }

        public void GetPerson(PersonRequest request, OnGetPersonCompleted onGetPersonCompleted)
        {
        }

        public void GetPhoto(string userId, OnGetPhotoCompleted onGetPhotoCompleted)
        {
        }

        public void GetRelatedPeople(RelatedPeopleRequest request, OnGetRelatedPersonCompleted onGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted onGetRelatedPeopleCompleted)
        {
        }

    }
}
