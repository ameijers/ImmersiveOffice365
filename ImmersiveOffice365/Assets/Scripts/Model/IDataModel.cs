using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataModel
{
     DataQueue Queue { get; }

    void LoadData();
}
