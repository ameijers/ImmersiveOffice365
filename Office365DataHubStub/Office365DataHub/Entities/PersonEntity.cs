using System.Collections.Generic;

namespace Office365DataHub.Entities
{
    public class PersonEntity : BaseEntity
    {
        public string FullName = "";
        public string Surname = "";
        public string GivenName = "";
        public string JobTitle = "";
        public string Department = "";
        public string OfficeLocation = "";
        public string PhoneNumber = "";
        public string EmailAddress = "";

        public PhotoDetail PhotoDetail;
    }

    public class PersonEntityCollection : List<PersonEntity>
    {

        public bool PersonExists(string personId)
        {
            foreach (PersonEntity person in this)
            {
                if (person.Id == personId)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
