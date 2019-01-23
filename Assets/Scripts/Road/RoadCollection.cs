using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollection: IEnumerable
{
    RoadType currentRoad;
    Dictionary<RoadType, RoadComponent> collection;
    List<RoadType> placedRoadsByOrder; 

    public RoadCollection(RoadComponent currentRoad)
    {
        collection = new Dictionary<RoadType, RoadComponent>();
        placedRoadsByOrder = new List<RoadType>();

        RoadType type = currentRoad.GetRoadType();

        Add(currentRoad);
        SetCurrent(type);
        Place(type);
    }

    public void Add(RoadComponent road)
    {
        RoadType type = road.GetRoadType();
        if (collection.ContainsKey(type))
        {
            Debug.LogError("RoadCollection:Add: road of this type existed");
            return;
        }
        collection.Add(type, road);
    }

    public RoadComponent Get(RoadType type)
    {
        if (collection.ContainsKey(type))
        {
            return collection[type];
        }
        return null;
    }

    private void SetCurrent(RoadType type)
    {
        if (collection.ContainsKey(type))
        {
            currentRoad = type;
        }
        else
        {
            Debug.LogError("RoadCollection:SetCurrent: collection does not have this type");
        }
    }

    public RoadComponent GetCurrentRoadComponent()
    {
        return collection[currentRoad];
    }

    public void Place(RoadType type)
    {
        if (collection.ContainsKey(type))
        {
            placedRoadsByOrder.Add(type);
        }
        else
        {
            Debug.LogError("RoadCollection:Place: collection does not have this type");
        }
    }

    public RoadComponent GetLastRoadComponent()
    {
        int count = placedRoadsByOrder.Count;
        if (count > 0)
        {
            RoadType type = placedRoadsByOrder[count - 1];
            return collection[type];
        }
        return null;
    }

    public void Next()
    {
        placedRoadsByOrder.RemoveAt(0);
        if (placedRoadsByOrder.Count > 0)
        {
            currentRoad = placedRoadsByOrder[0];
        }
        else
        {
            Debug.LogError("RoadCollection:Next: no next road component");
        }
    }

    public RoadComponent GetRoadByIndex(int index)
    {
        int count = placedRoadsByOrder.Count;
        if (count > index)
        {
            RoadType type = placedRoadsByOrder[index];
            return collection[type];
        }
        return null;
    }

    public IEnumerator GetEnumerator()
    {
        return collection.Values.GetEnumerator();
    }
}
