using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    // 对象池的集合：key[资源的路径名],value:【对应的对象池】
    public Dictionary<string, Pool> allPool = new Dictionary<string, Pool>();
    
    // key:实例对象的ID编号，value：对应的资源的路径名
    public Dictionary<int, string> allInstanceIDs = new Dictionary<int, string>();
    
    public GameObject GetAObjFromPool(GameObject gameObject)
    {
        string prefabPath = gameObject.GetHashCode().ToString();
        Pool pool = null;
        if (allPool.ContainsKey(prefabPath))
        {
            pool = allPool[prefabPath];// 取出对应的Pool
        }
        else
        {
            pool = new Pool();
            pool.prefabPath = prefabPath;
            allPool.Add(prefabPath,pool);// 加入集合中
        }
        GameObject obj = pool.GetObj(gameObject);// 从池子中取出一个对象
        if (obj != null)
        {
            obj.SetActive(true);
        }
        return obj;
    }
    
    public void ReleaseAObjFromPool(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        int instanceId = obj.GetInstanceID();// 根据ID 判断是同一个对象
        if (allInstanceIDs.ContainsKey(instanceId))// 根据 ID，判断是哪一种资源对象
        {
            string path = allInstanceIDs[instanceId];
            if (allPool.ContainsKey(path))// 找到对应的对象池
            {
                Pool pool = allPool[path];
                pool.Release(obj);// 集合中存储信息的变化
                obj.SetActive(false);// 隐藏对象
            }
        }
    }
    
    public void ClearAllPool()
    {
        foreach (Pool pool in allPool.Values)
        {
            pool.Clear();
        }
    
        allPool.Clear();
        allInstanceIDs.Clear();
    }
}