using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeEventManager
{
    public static NativeEventManager instance;

    public event Action myEvent;
    public event Action<int> myIdEvent;

    public static NativeEventManager Instance
    {
        get
        {
            return instance != null ? instance : instance = new NativeEventManager();
        }
    }
    protected NativeEventManager()
    {
        myEvent += DoAction;
        myIdEvent += DoAction;

        myEvent.Invoke();
        myIdEvent.Invoke(23);
    }

    public void DoAction()
    {

    }

    public void DoAction(int id)
    {

    }
}
