using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer{
    public enum Event
    {
        CoinCollection,
        PlayerFaceHit,
    }

    private static Observer instance;
    private Dictionary<Event, Dictionary<int, System.Action>> reserveBook;
    private int _id=1;

    private Observer()
    {
        reserveBook = new Dictionary<Event, Dictionary<int, System.Action>>();
        Observer.instance = this;
    }
    public static Observer GetInstance()
    {
        if(instance == null)
        {
            instance = new Observer();
        }
        return instance;
    }
    public int Register(Event e, System.Action a)
    {
        int id = GetNextId();
        if (!reserveBook.ContainsKey(e))
        {
            reserveBook.Add(e, new Dictionary<int, System.Action>());
        }
        reserveBook[e].Add(id, a);
        return id;
    }
    public void Unregister(Event e, int id)
    {
        if (reserveBook.ContainsKey(e))
        {
            reserveBook[e].Remove(id);
        }
    }
    public void Trigger(Event e)
    {
        Debug.Log("Trigger: " + e.ToString());

        if(!reserveBook.ContainsKey(e))
        {
            return;
        }
        System.Action a = null;
        foreach(int id in reserveBook[e].Keys)
        {
            a = reserveBook[e][id];
            try
            {
                if (a == null)
                {
                    Unregister(e, id);
                }
                else
                {
                    a.Invoke();
                }
            }
            catch(System.Exception er)
            {
                Debug.LogException(er);
            }
        }
    }
    private int GetNextId()
    {
        return _id++;
    }
}

public class ObserverBroker
{
    private Dictionary<string, Receipt> registrationRoll;
    private Observer oi;

    private struct Receipt
    {
        public Observer.Event ev;
        public int id;
        public Receipt(Observer.Event e, int i)
        {
            this.ev = e;
            this.id = i;
        }
    }

    public ObserverBroker()
    {
        registrationRoll = new Dictionary<string, Receipt>();
        oi = Observer.GetInstance();
    }
    public void Register(Observer.Event e, System.Action action, string name)
    {
        int id = oi.Register(e, action);
        registrationRoll.Add(name, new Receipt(e, id));
    }
    public void Unregister(string name)
    {
        if (registrationRoll.ContainsKey(name))
        {
            Receipt r = registrationRoll[name];
            registrationRoll.Remove(name);
            oi.Unregister(r.ev, r.id);
        }
        else
        {
            throw new System.Exception("name was not registered before");
        }
    }

}
