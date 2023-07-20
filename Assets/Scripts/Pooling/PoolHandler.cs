using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolHandler : Singleton<PoolHandler>
{
    public Transform PoolParent;

    [System.Serializable]
    internal struct Pool
    {
        internal Queue<PoolObject> pooledObjects;
        [SerializeField] internal PoolObject objectPrefab;
        [SerializeField] internal int poolSize;
    }

    [SerializeField]
    private Pool[] pools;

    public TaskCompletionSource<bool> IsLoading = new TaskCompletionSource<bool>();
    public bool hasLoading = false;
    private void Start()
    {
        CreatePools();
    }

    void CreatePools()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledObjects = new Queue<PoolObject>();

            for (int j = 0; j < pools[i].poolSize; j++)
            {
                PoolObject _po = Instantiate(pools[i].objectPrefab, PoolParent);
                _po.OnCreated();
            }

            if (i == pools.Length - 1)
            {
                IsLoading.SetResult(true);
                hasLoading = true;
            }

        }

        DeckManager.I.GetDeck();
        DeckManager.I.GetExtraPogs();

    }

    public T GetObject<T>() where T : PoolObject
    {
        for (int i = 0; i < pools.Length; i++)
        {

            if (typeof(T) == pools[i].objectPrefab.GetType())
            {
                if (pools[i].pooledObjects.Count == 0)
                {
                    PoolObject _po = Instantiate(pools[i].objectPrefab, transform);
                    _po.OnCreated();
                    EnqueObject(_po);
                    return GetObject<T>();
                }
                else
                {
                    T t = pools[i].pooledObjects.Dequeue() as T;
                    t.OnSpawn();
                    return t;
                }


            }

        }
        return default;
    }

    public void EnqueObject<T>(T po) where T : PoolObject
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (typeof(T) == pools[i].objectPrefab.GetType())
            {
                if (!pools[i].pooledObjects.Contains(po))
                {
                    pools[i].pooledObjects.Enqueue(po);
                }

            }
        }
    }
}
