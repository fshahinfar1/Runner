using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLooper : MonoBehaviour
{
    private bool initialized = false;

    private RoadCollection collection;

    private float planeWidth = 10;

    private Vector3 position;  // current road component position

    private void Awake()
    {
        if (!initialized)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        collection = new RoadCollection();
        RoadComponent component;
        int count = transform.childCount;
        for (int i=0; i < count; i++)
        {
            component = transform.GetChild(i).GetComponent<RoadComponent>();
            collection.Add(component);
            collection.AppendComponentList(component);
        }
    }

    /// <summary>
    /// place road component of the given type after last road
    /// component
    /// </summary>
    /// <param name="type">type of next road component</param>
    /// <returns>position of place road component</returns>
    public Vector3 Place(RoadType type)
    {
        RoadComponent shouldBePlacedRoad = collection.Get(type);
        RoadComponent currentRoad = collection.GetCurrentRoadComponent();
        currentRoad.SetActive(false);

        // get last placed road
        RoadComponent lastRoad = collection.GetLastRoadComponent();
        // get position of last placed road 
        position = lastRoad.GetPosition();
        // move position forwad for finding position of next road
        position.z += planeWidth;
        shouldBePlacedRoad.SetPosition(position);  // place new road after the last road
        collection.AppendComponentList(shouldBePlacedRoad);  // a new road has been placed
        shouldBePlacedRoad.Place();
        collection.Next();  // update collection current road to next road
        return position;
    }

    public RoadComponent GetLastRoad()
    {
        return collection.GetLastRoadComponent();
    }

    public RoadComponent GetRoadByIndex(int index)
    {
        return collection.GetRoadByIndex(index);
    }

    public Vector3 ResetOrigin()
    {
        Vector3 diff = collection.GetCurrentRoadComponent().GetPosition();

        foreach (RoadComponent rc in collection)
        {
            Vector3 pos = rc.GetPosition() - diff;
            rc.SetPosition(pos);
        }
        return diff;
    }

    public void InsertRoadComponent(RoadType type, int index) {
        int count = collection.CountRoadComponents();
        if (index > count) {
            throw new UnityException("Out of range!");
        }
        var toBePlaced = collection.Get(type);
        var refrenceCmpnt = collection.GetRoadByIndex(index);
        var position = refrenceCmpnt.GetPosition();
        toBePlaced.SetPosition(position);
        collection.RemoveFromList(toBePlaced);
        count--;
        RoadComponent cmpnt;
        // move components from index to the end forward
        for (int i = index; i < count; i++) {
            cmpnt = collection.GetRoadByIndex(i);
            position = cmpnt.GetPosition();
            position.z += planeWidth;
            cmpnt.SetPosition(position);
        }
        // add newly placed component to the list
        collection.InsertComponent(toBePlaced, index);
        toBePlaced.Place();
    }
}
