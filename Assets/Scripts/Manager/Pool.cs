using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public string prefabPath;

    public List<GameObject> usingList = new List<GameObject>();// 正在使用列表
    public List<GameObject> freeList = new List<GameObject>();// 空闲列表

    public GameObject GetObj(GameObject prefab)// 从对象池获取一个对象
    {
        GameObject obj = null;
        if (freeList.Count > 0)// 空闲列表有对象
        {
            obj = freeList[0];
            usingList.Add(obj);// 加入使用列表
            freeList.RemoveAt(0);// 从空闲列表中删除
        }
        else// 空闲列表没有对象
        {
            // 克隆一个对象
            obj = GameObject.Instantiate(prefab);
            PoolManager.Instance.allInstanceIDs.Add(obj.GetInstanceID(), prefabPath);
            usingList.Add(obj);
        }
        return obj;
    }

    public void Release(GameObject obj)// 回收
    {
        usingList.Remove(obj);// 使用列表删除
        freeList.Add(obj);// 空闲列表添加这个对象
    }

    public void Clear()
    {
        foreach (var item in usingList)
        {
            GameObject.Destroy(item);
        }
        foreach (var item in freeList)
        {
            GameObject.Destroy(item);
        }
        usingList.Clear();
        freeList.Clear();
    }
}