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

        RoadComponent component = transform.GetChild(0).GetComponent<RoadComponent>();
        collection = new RoadCollection(component);

        int count = transform.childCount;
        for (int i=1; i<count; i++)
        {
            component = transform.GetChild(i).GetComponent<RoadComponent>();
            collection.Add(component);
            collection.Place(component.GetRoadType());
        }
    }

    /// <summary>
    /// place road component of the given type after this road
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
        collection.Place(type);  // a new road has been placed
        shouldBePlacedRoad.SetActive(true);
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
}
