using Office365DataHub.Entities;
using System.Collections.Generic;

public class DataQueueItem
{
    public BaseEntity Root;
    //public string RootId;
    public BaseEntity Refering;
}

public class DataQueue: List<DataQueueItem>
{
    public bool GetFromQueue(out BaseEntity root, out BaseEntity refering)
    {
        if (this.Count > 0)
        {
            root = this[0].Root;
            refering = this[0].Refering;

            this.RemoveAt(0);

            return true;
        }

        root = null;
        refering = null; 

        return false;
    }

    public void AddToQueue(BaseEntity root, BaseEntity refering)
    {
        Add(new DataQueueItem() { Root = root, Refering = refering });
    }
}
