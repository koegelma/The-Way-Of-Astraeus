using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag { LASERPROJECTILE, MISSILEPROJECTILE, SMALLHITEXPLOSION, LARGEHITEXPLOSION, SHIPEXPLOSION, DAMAGEUI, BULLETPROJECTILE, PLASMAPROJECTILE, COIN }
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
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    private void Start()
    {
        AllocateObjectPool(PoolTag.SMALLHITEXPLOSION.ToString(), PoolTag.SMALLHITEXPLOSION, 30);
        AllocateObjectPool(PoolTag.LARGEHITEXPLOSION.ToString(), PoolTag.LARGEHITEXPLOSION, 15);
        AllocateObjectPool(PoolTag.SHIPEXPLOSION.ToString(), PoolTag.SHIPEXPLOSION, 10);
        AllocateObjectPool(PoolTag.COIN.ToString(), PoolTag.COIN, 100);
    }

   /*  public void CreatePool(string _tag, GameObject _gameObject, int _size) // not used atm
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < _size; i++)
        {
            GameObject obj = Instantiate(_gameObject);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        poolDictionary.Add(_tag, objectPool);
    } */

   /*  public void CreatePoolFromArray(string _tag, GameObject[] _gameObjects, int _size) // not used atm
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            for (int s = 0; s < _size; s++)
            {
                GameObject obj = Instantiate(_gameObjects[i]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }
        poolDictionary.Add(_tag, objectPool);
    } */

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

    public void DelayDeallocatingObjectPool(string _tag, float _secondsToWait)
    {
        StartCoroutine(DeallocateObjectPool(_tag, _secondsToWait));
    }

    public IEnumerator DeallocateObjectPool(string _tag, float _secondsToWait)
    {
        yield return new WaitForSeconds(_secondsToWait);
        if (!poolDictionary.ContainsKey(_tag))
        {
            Debug.LogError("Pool with tag " + _tag + " doesn't exist.");
            yield return null;
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
            Debug.LogError("Pool with tag " + _tag + " doesn't exist.");
            return null;
        }
        GameObject objToSpawn = poolDictionary[_tag].Dequeue();
        objToSpawn.transform.position = _position;
        objToSpawn.transform.rotation = _rotation;
        objToSpawn.SetActive(true);
        poolDictionary[_tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
