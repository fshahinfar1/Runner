using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool <T> {

    public GameObject[] pool;
    public T[] component;
    int totalCount;
	
    /// <summary>
    /// Fill the pool with GameObjects Instanciated from tmplate.
    /// gameobjects will be placed as a child of root object in the scene hierachy
    /// </summary>
    /// <param name="template">template from which to create a new gameobject</param>
    /// <param name="count">how many game objects to create</param>
    /// <param name="root">parent of created gameobjects</param>
    public void MakePool(GameObject template, int count, Transform root)
    {
        if(template == null)
        {
            throw new UnityException("template cannot be null");
        }
        totalCount = count;
        pool = new GameObject[count];
        component = new T[count];
        for (int i = 0; i < count; i++)
        {
            pool[i] = Object.Instantiate(template);
            pool[i].transform.SetParent(root, false);
            component[i] = pool[i].GetComponent<T>();
        }
    }

    /// <summary>
    /// manipulate pool objects with this function
    /// all game object in range [0, index) will be set active
    /// and given action is applied to them
    /// game object in range [index, totalCount) 
    /// will be set deactive
    /// </summary>
    /// <param name="index">shows the index up to which game objects should be active
    /// if it is set to negative value all gameobject will be deactive
    /// and if it is set to a value larger than count of objects all objects
    /// will be set active</param>
    /// <param name="action"> this action is applied to all game object which should 
    /// be active [in range[0, index)]</param>
    public void SetActiveFalseFrom (int index, System.Action<T, int> action)
    {
        if (pool == null)
        {
            throw new UnityException("Pool is not created yet [use MakePool]");
        }
        if(index > totalCount)
        {
            index = totalCount;
        }
        else if(index < 0)
        {
            index = 0;
        }
        for (int i = 0; i < index; i++)
        {
            pool[i].SetActive(true);
            action(component[i], i);
        }
        for (int i = index; i < totalCount; i++)
        {
            pool[i].SetActive(false);
        }
    }

    /// <summary>
    /// Access components in the pool individualy
    /// by index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetIndex(int index)
    {
        if (pool == null)
        {
            throw new UnityException("Pool is not created yet [use MakePool]");
        }
        if (index > totalCount)
        {
            index = totalCount;
        }
        else if (index < 0)
        {
            index = 0;
        }
        return component[index];
    }
}
