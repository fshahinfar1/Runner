using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollection: IEnumerable
{
    Dictionary<RoadType, List<RoadComponent>> collection;
    List<RoadComponent> placedRoadsByOrder; 

    public RoadCollection()
    {
        collection = new Dictionary<RoadType, List<RoadComponent>>();
        placedRoadsByOrder = new List<RoadComponent>();
    }

    public void Add(RoadComponent road)
    {
        RoadType type = road.GetRoadType();
        if (!collection.ContainsKey(type)) {
            collection[type] = new List<RoadComponent>();
        }
        var list = collection[type];
        list.Add(road);
    }

    public RoadComponent Get(RoadType type)
    {
        if (collection.ContainsKey(type))
        {
            var list = collection[type];
            var component = list[0];
            list.RemoveAt(0);
            list.Add(component);  // move component to the end of the list
            return component;
        }
        return null;
    }

    public RoadComponent GetCurrentRoadComponent()
    {
        return placedRoadsByOrder[0];
    }

    public void AppendComponentList(RoadComponent component)
    {
        var type = component.GetRoadType();
        if (collection.ContainsKey(type))
        {
            placedRoadsByOrder.Add(component);
        }
        else
        {
            Debug.LogError("RoadCollection:Place: collection does not have this type");
        }
    }

    public void InsertComponent(RoadComponent component, int index) {
        placedRoadsByOrder.Insert(index, component);
    }

    public int CountRoadComponents() {
        return placedRoadsByOrder.Count;
    }

    public RoadComponent GetLastRoadComponent()
    {
        int count = placedRoadsByOrder.Count;
        if (count > 0)
        {
            var component = placedRoadsByOrder[count - 1];
            return component;
        }
        return null;
    }

    public void Next()
    {
        placedRoadsByOrder.RemoveAt(0);
        if (placedRoadsByOrder.Count == 0)
        {
            Debug.LogError("RoadCollection:Next: no next road component");
        }
    }

    public RoadComponent GetRoadByIndex(int index)
    {
        int count = placedRoadsByOrder.Count;
        if (count > index && index > -1)
        {
            return placedRoadsByOrder[index];
        }
        return null;
    }

    public IEnumerator GetEnumerator()
    {
        return placedRoadsByOrder.GetEnumerator();
    }

    public void RemoveFromList(RoadComponent compnt) {
        int id = compnt.GetId();
        int count = placedRoadsByOrder.Count;
        int otherId;
        for (int i = 0; i < count; i++) {
            otherId = placedRoadsByOrder[i].GetId();
            if (otherId == id) {
                placedRoadsByOrder.RemoveAt(i);
                break;
            }
        }
    }

    public void Rotate(int count) {
        int size = placedRoadsByOrder.Count;
        count = count % size;
        Debug.Log(count);
        RoadComponent[] memory = new RoadComponent[count];
        for (int i = 0; i < count; i++)
            memory[i] = placedRoadsByOrder[i];
        for (int i = count; i < size; i++)
            placedRoadsByOrder[i-count] = placedRoadsByOrder[i];
        int offest = size - count;
        for (int i = 0; i < count; i++)
            placedRoadsByOrder[offest + i] = memory[i];
    }

    public int IndexOf(RoadComponent compnt) {
        int size = placedRoadsByOrder.Count;
        for (int i = 0; i < size; i++) {
            if (placedRoadsByOrder[i] == compnt)
                return i;
        }
        return -1;
    }
}
