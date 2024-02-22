using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private PlayerBagItem itemPrefab;

    private Transform itemParent;
    private Dictionary<string, List<Data.SceneItem>> sceneItemDict = new Dictionary<string, List<Data.SceneItem>>();

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoadEvent);
        EventCenter.StartListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoadEvent);
        EventCenter.StopListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
    }

    private void OnBeforeSceneUnLoadEvent(BeforeSceneUnLoadEvent evt)
    {
        GetSceneAllItem();
    }

    private void OnAfterSceneLoadEvent(AfterSceneLoadEvent evt)
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
        RecreateAllItems(itemParent);
    }

    private void GetSceneAllItem()
    {
        List<Data.SceneItem> currentSceneItem = new List<Data.SceneItem>();
        foreach (var item in FindObjectsOfType<PlayerBagItem>())
        {
            Data.SceneItem sceneItem = new Data.SceneItem()
            {
                ItemID = item.itemId,
                Pos = new Data.SerializableV3(item.transform.position)
            };
            currentSceneItem.Add(sceneItem);
        }

        if (!sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItem);
        }
        else
        {
            sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItem;
        }
    }

    private void RecreateAllItems(Transform itemParent)
    {
        List<Data.SceneItem> currentSceneItem = new List<Data.SceneItem>();
        if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItem))
        {
            if (currentSceneItem != null)
            {
                foreach (var item in FindObjectsOfType<PlayerBagItem>())
                {
                    // 后期优化
                    // PoolManager.Instance.ReleaseAObjFromPool(item.gameObject);
                    Destroy(item.gameObject);
                }

                foreach (var item in currentSceneItem)
                {
                    // 后期优化
                    // GameObject newItem = PoolManager.Instance.GetAObjFromPool(itemPrefab.gameObject);
                    // newItem.transform.position = item.Pos.ToV3();
                    // newItem.transform.rotation = Quaternion.identity;
                    // newItem.transform.parent = itemParent;
                    // PlayerBagItem newBagItem = newItem.GetComponent<PlayerBagItem>();
                    PlayerBagItem newBagItem =
                        Instantiate(itemPrefab, item.Pos.ToV3(), Quaternion.identity, itemParent);
                    newBagItem.Init(item.ItemID);
                }
            }
        }
    }
}