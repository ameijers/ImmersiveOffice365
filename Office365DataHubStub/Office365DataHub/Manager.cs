namespace Office365DataHub
{
    public class Manager : Singleton<Manager>
    {
        public delegate void OnGetSomeInformationCompleted(string result);

        public void GetSomeInformation(string url, OnGetSomeInformationCompleted onGetSomeInformationCompleted)
        {
        }
    }
}
