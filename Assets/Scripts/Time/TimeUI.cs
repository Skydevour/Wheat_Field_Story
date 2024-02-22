using System;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Events.TimeEvents;

public class TimeUI : MonoBehaviour
{
   [SerializeField] private Image dayNightImage;//白天晚上的图片
   [SerializeField] private Text dataText;//日期
   [SerializeField] private Text timeText;//时间
   [SerializeField] private Sprite[] daySprites;//白天和晚上的图片
   [SerializeField] private RectTransform pointer;//指针

   private void OnEnable()
   {
      EventCenter.StartListenToEvent<GameMinuteEvent>(OnGameMinute);
      EventCenter.StartListenToEvent<GameHourEvent>(OnGameHour);
   }

   private void OnDisable()
   {
      EventCenter.StopListenToEvent<GameMinuteEvent>(OnGameMinute);
      EventCenter.StopListenToEvent<GameHourEvent>(OnGameHour);
   }

   /// <summary>
   /// 根据hour来选择指针
   /// </summary>
   /// <param name="hour"></param>
   private void SwitchPointer(int hour)
   {
      Vector3 pointerAngle = new Vector3(0, 0, -(hour / 24f * 180));
      pointer.DORotate(pointerAngle, 1f, RotateMode.Fast);
   }
   
   private void OnGameMinute(GameMinuteEvent gameMinuteEvent)
   {
      timeText.text = gameMinuteEvent.Hour.ToString("00") + ":" + gameMinuteEvent.Minute.ToString("00") + ":" +
                      gameMinuteEvent.Second.ToString("00");
   }
   
   private void OnGameHour(GameHourEvent gameHourEvent)
   {
      dataText.text = gameHourEvent.Year + "年" + gameHourEvent.Month.ToString("00") + "月" +
                      gameHourEvent.Day.ToString("00") + "日";
      SwitchPointer(gameHourEvent.Hour);
      if (gameHourEvent.Hour>=8&&gameHourEvent.Hour<=18)
      {
         dayNightImage.sprite = daySprites[0];
      }
      else
      {
         dayNightImage.sprite = daySprites[1];
      }
   }
}
