using System;
using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;

public class AnimatorSwitch : MonoBehaviour
{
   [SerializeField] private Animator[] playAnimators;//所有的animator
   [SerializeField] private SpriteRenderer holdItem;//举起的物品
   [SerializeField] private List<Data.AnimatorType> animatorTypes;//各个部位以及所对应的状态
   private Dictionary<string, Animator> animatorsDic = new Dictionary<string, Animator>();//animator字典

   private void OnEnable()
   {
      EventCenter.StartListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoad);
      EventCenter.StartListenToEvent<ItemSelectedEvent>(OnItemSelected);
      EventCenter.StartListenToEvent<HarvestAtPlayer>(OnHarvestAtPlayer);
   }

   private void OnDisable()
   {
      EventCenter.StopListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoad);
      EventCenter.StopListenToEvent<ItemSelectedEvent>(OnItemSelected);
      EventCenter.StopListenToEvent<HarvestAtPlayer>(OnHarvestAtPlayer);
   }

   private void Awake()
   {
      foreach (var playAnimator in playAnimators)
      {
         animatorsDic.Add(playAnimator.name,playAnimator);
      }
   }

   private void SwitchAnimatorState(Enums.PartType partType)
   {
      foreach (var animatorType in animatorTypes)
      {
         if (animatorType.PartType == partType)
         {
            animatorsDic[animatorType.PartName.ToString()].runtimeAnimatorController = animatorType.OverrideController;
         }
      }
   }

   /// <summary>
   /// 更新物品图片
   /// </summary>
   /// <param name="itemSprite"></param>
   /// <returns></returns>
   private IEnumerator ShowItem(Sprite itemSprite)
   {
      holdItem.sprite = itemSprite;
      holdItem.enabled = true;
      yield return new WaitForSeconds(1);
      holdItem.enabled = false;
   }

   #region EventCenter

   /// <summary>
   /// 场景卸载前事件
   /// </summary>
   /// <param name="beforeSceneUnLoadEvent"></param>
   private void OnBeforeSceneUnLoad(BeforeSceneUnLoadEvent beforeSceneUnLoadEvent)
   {
      holdItem.enabled = false;
      SwitchAnimatorState(Enums.PartType.None);
   }
   
   /// <summary>
   /// 选中物品事件
   /// </summary>
   /// <param name="itemSelectedEvent"></param>
   private void OnItemSelected(ItemSelectedEvent itemSelectedEvent)
   {
      Enums.PartType currentType = itemSelectedEvent.ItemDetails.ItemType switch
      {
         Enums.ItemType.Seed => Enums.PartType.Carry,//举起商品
         Enums.ItemType.Commodity => Enums.PartType.Carry,//举起商品
         Enums.ItemType.HoeTool => Enums.PartType.Hoe,//握住工具
         _ => Enums.PartType.None//默认状态
      };
      if (itemSelectedEvent.IsSelected==false)
      {
         currentType = Enums.PartType.None;
         holdItem.enabled = false;
      }
      else
      {
         if (currentType==Enums.PartType.Carry)
         {
            holdItem.sprite = itemSelectedEvent.ItemDetails.ItemIcon;
            holdItem.enabled = true;
         }
         else
         {
            holdItem.enabled = false;
         }
      }
      SwitchAnimatorState(currentType);
   }

   /// <summary>
   /// 在人物头顶生成果实
   /// </summary>
   /// <param name="evt"></param>
   private void OnHarvestAtPlayer(HarvestAtPlayer evt)
   {
      Sprite itemSprite = PlayerBagManager.Instance.GetItemDetails(evt.ID).ItemIcon;
      if (!holdItem.enabled)
      {
         StartCoroutine(ShowItem(itemSprite));
      }
   }
   #endregion
}
