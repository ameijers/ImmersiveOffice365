using Microsoft.Graph;
using Office365DataHub.Data;
using Office365DataHub.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Office365DataHub.Services
{
    public class PeopleService : Singleton<PeopleService>
    {
        public delegate void OnGetPersonCompleted(Entities.PersonRequest request);
        public delegate void OnGetPhotoCompleted(PhotoDetail photo);
        public delegate void OnGetRelatedPersonCompleted(RelatedPeopleRequest request);
        public delegate void OnGetRelatedPeopleCompleted(RelatedPeopleRequest request);

        public void GetCurrentUser(OnGetPersonCompleted onGetPersonCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetPersonAsync(new Entities.PersonRequest(), onGetPersonCompleted));
        }

        public void GetPerson(Entities.PersonRequest request, OnGetPersonCompleted onGetPersonCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetPersonAsync(request, onGetPersonCompleted));
        }

        public async Task GetPersonAsync(Entities.PersonRequest request, OnGetPersonCompleted onGetPersonCompleted)
        {
            var graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            if (AuthenticationHelper.Instance.ServiceException.Error == ServiceError.AuthenticationClientError)
            {
                request.expection = AuthenticationHelper.Instance.ServiceException;
                onGetPersonCompleted(request);
                return;
            }

            if (graphClient != null)
            {
                try
                {
                    User user = request.id == "" ? await graphClient.Me.Request().GetAsync() : await graphClient.Users[request.id].Request().GetAsync();

                    if (request.id == "")
                    {
                        request.id = user.Id;
                    }

                    request.person = new PersonEntity()
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
                        PhotoDetail = await GetPhotoAsync(request.id)
                    };
                }
                catch(Exception ex)
                {
                    request.expection.Error = ServiceError.UserError;
                    request.expection.Exception = ex;
                }
            }

            onGetPersonCompleted(request);
        }

        public void GetPhoto(string userId, OnGetPhotoCompleted onGetPhotoCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetPhotoAsync(userId, onGetPhotoCompleted));
        }

        public async Task GetPhotoAsync(string userId, OnGetPhotoCompleted onGetPhotoCompleted)
        {
            onGetPhotoCompleted(await GetPhotoAsync(userId));
        }

        public async Task<PhotoDetail> GetPhotoAsync(string userId)
        {
            PhotoDetail detail;

            var graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            try
            {
                ProfilePhoto photo = await graphClient.Users[userId].Photo.Request().GetAsync();

                Stream photoStream = await graphClient.Users[userId].Photo.Content.Request().GetAsync();

                using (MemoryStream ms = new MemoryStream())
                {
                    photoStream.CopyTo(ms);
                    detail.Photo = ms.ToArray();
                }

                detail.Width = photo.Width;
                detail.Height = photo.Height;
            }
            catch (Exception)
            {
                detail.Width = 0;
                detail.Height = 0;
                detail.Photo = null;
            }

            return detail;
        }

        public void GetRelatedPeople(RelatedPeopleRequest request, OnGetRelatedPersonCompleted onGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted onGetRelatedPeopleCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetRelatedPeopleASync(request, onGetRelatedPersonCompleted, onGetRelatedPeopleCompleted) );
        }

        public async Task GetRelatedPeopleASync(RelatedPeopleRequest request, OnGetRelatedPersonCompleted onGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted onGetRelatedPeopleCompleted)
        {
            List<Person> persons = new List<Person>();

            var graphClient = AuthenticationHelper.Instance.GetAuthenticatedClient();

            string filter = "personType/class eq 'Person' and personType/subclass eq 'OrganizationUser'";

            if (graphClient != null)
            {
                if (request.person.Id == "")
                {
                    IUserPeopleCollectionPage people = await graphClient.Me.People.Request().Filter(filter).GetAsync();
                    persons.AddRange(people);
                }
                else
                {
                    try
                    { 
                        IUserPeopleCollectionPage people = await graphClient.Users[request.person.Id].People.Request().Filter(filter).GetAsync();
                        persons.AddRange(people);
                    }
                    catch (Exception)
                    {
                    }
                }

            }

            foreach (Person person in persons)
            {
                switch (person.PersonType.Class)
                {
                    case "Person":
                        PhotoDetail detail = await GetPhotoAsync(person.Id);

                        PersonEntity data = new PersonEntity()
                        {
                            FullName = person.DisplayName,
                            Surname = person.Surname,
                            GivenName = person.GivenName,
                            JobTitle = person.JobTitle,
                            Department = person.Department,
                            OfficeLocation = person.OfficeLocation,
                            PhoneNumber = person.Phones.Any() ? person.Phones.First().Number : "",
                            EmailAddress = person.ScoredEmailAddresses.Any() ? person.ScoredEmailAddresses.First().Address : "",
                            Id = person.Id,
                            PhotoDetail = detail
                        };

                        request.relatedPerson = data;
                        onGetRelatedPersonCompleted(request);
                        request.relatedPeople.Add(data);
                        break;

                    case "Group":
                        break;
                }
            }

            request.relatedPerson = null;
            onGetRelatedPeopleCompleted(request);
        }

    }
}
