using Office365DataHub.Entities;
using System.Collections.Generic;

public class DataQueueItem
{
    public string RootId;
    public BaseEntity Refering;
}

public class DataQueue: List<DataQueueItem>
{
    public bool GetFromQueue(out string rootId, out BaseEntity entity)
    {
        if (this.Count > 0)
        {
            rootId = this[0].RootId;
            entity = this[0].Refering;

            this.RemoveAt(0);

            return true;
        }

        rootId = "";
        entity = null; 

        return false;
    }

    public void AddToQueue(string rootId, BaseEntity entity)
    {
        Add(new DataQueueItem() { RootId = rootId, Refering = entity });
    }
}
