using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag { LASERPROJECTILE, MISSILEPROJECTILE, HITEXPLOSION, SHIPEXPLOSION }
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolObject
    {
        public PoolTag poolTag;
        public GameObject prefab;
    }
    public static ObjectPooler instance;
    public List<PoolObject> poolObjects;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        AllocateObjectPool(PoolTag.HITEXPLOSION.ToString(), PoolTag.HITEXPLOSION, 30);
        AllocateObjectPool(PoolTag.SHIPEXPLOSION.ToString(), PoolTag.SHIPEXPLOSION, 10);
    }

    public void AllocateObjectPool(string _tag, PoolTag _poolTag, int _size)
    {
        PoolObject poolObjectRef = null;
        foreach (PoolObject poolObject in poolObjects)
        {
            if (poolObject.poolTag == _poolTag) poolObjectRef = poolObject;
        }
        if (poolObjectRef == null)
        {
            Debug.LogError("PoolObject reference not found.");
            return;
        }

        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < _size; i++)
        {
            GameObject obj = Instantiate(poolObjectRef.prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        poolDictionary.Add(_tag, objectPool);
    }

    public void DeallocateObjectPool(string _tag)
    {
        if (!poolDictionary.ContainsKey(_tag))
        {
            Debug.LogError("Pool with tag " + _tag + "doesn't exist.");
            return;
        }
        foreach (GameObject obj in poolDictionary[_tag])
        {
            Destroy(obj);
        }
        poolDictionary.Remove(_tag);
    }

    public GameObject SpawnFromPool(string _tag, Vector3 _position, Quaternion _rotation)
    {
        if (!poolDictionary.ContainsKey(_tag))
        {
            Debug.LogError("Pool with tag " + _tag + "doesn't exist.");
            return null;
        }
        GameObject objToSpawn = poolDictionary[_tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = _position;
        objToSpawn.transform.rotation = _rotation;
        poolDictionary[_tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
