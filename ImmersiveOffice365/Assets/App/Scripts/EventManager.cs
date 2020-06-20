using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class IDEvent : UnityEvent<int>
{
}

public class EventManager : MonoBehaviour
{
    public UnityEvent actionEvent;

    public IDEvent idEvent;
    public EventManager()
    {
    }

    public void DoAction()
    {
    }

    public void DoAction(int id)
    {

    }
}
