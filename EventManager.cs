using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void EventDelegate(String eventID, object args);


public class EventManager : MonoBehaviour
{
    //Member Variables
    private static EventManager instance;
    private Dictionary<String, EventDelegate> m_Events = new Dictionary<String, EventDelegate>();


    //Initialization
    void Awake()
    { 
        if (instance == null) instance = this;
    }



    //Event Handling
    public static void Subscribe(String eventID, EventDelegate callback)
    {
        if (instance != null)
        {
            EventDelegate tempDel;
            if (instance.m_Events.TryGetValue(eventID, out tempDel))
            {
                instance.m_Events[eventID] = tempDel += callback;
            } else
            {
                instance.m_Events[eventID] = callback;
            }

        }
    }

    public static void Unsubscribe(String eventID, EventDelegate callback)
    {
        if (instance != null)
        {
            EventDelegate tempDel;
            if (instance.m_Events.TryGetValue(eventID, out tempDel))
            {
                tempDel -= callback;
                if (tempDel == null)
                {
                    instance.m_Events.Remove(eventID);
                }
                else {
                    instance.m_Events[eventID] = tempDel;
                }
            } else {
                Debug.LogWarning("An object tried to unsubscribe frome event " + eventID + " without being subscribed");
            }
        }
    }

    public static void Dispatch(String eventID, object args = null)
    {
        EventDelegate del;
        if (instance.m_Events.TryGetValue(eventID, out del))
        {
            del.Invoke(eventID, args);
        } else {
            Debug.LogWarning("Event " + eventID + " was disptched with no listeners");
        }
    }
}
