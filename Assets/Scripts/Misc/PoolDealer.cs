using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolDealer
{
    class Pool
    {
        public int size;
        public int readyIndex;
        public List<object> pool;
    }

    private static PoolDealer instance = new PoolDealer();
    public static PoolDealer Instance { get { return instance; } }

    Dictionary<string, Pool> table;

    private PoolDealer()
    {
        int types = 2;
        table = new Dictionary<string, Pool>(types);
    }

    public void CreatePool<T>(string poolType, int count,
        bool isArray = false, int dimention = 0) where T : new()
    {
        Pool p = new Pool();
        p.readyIndex = 0;
        p.size = count;
        p.pool = new List<object>(count);
        if (isArray)
        {
            for (int i = 0; i < count; i++)
                p.pool.Add(new T[dimention]);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                p.pool.Add(new T());
            }
        }
        table[poolType] = p;
    }

    public object Get(string t)
    {
        Pool p = table[t];
        object o = p.pool[p.readyIndex];
        p.readyIndex = (p.readyIndex + 1) % p.size;

        return o;
    }
}
