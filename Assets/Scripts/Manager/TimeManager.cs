using System;
using CommonFramework.Runtime;
using UnityEngine;

namespace Manager
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;//时间变量
        public Enums.Season gameSeason = Enums.Season.Spring;//季节
        private int monthInSeason = 3;//一个季节3个月份
        private float SecondThreshold = 0.01f;//秒数阈值
        private int secondScale = 59;//秒进制
        private int minuteScale = 59;//分钟进制
        private int hourScale = 23;//小时进制
        private int dayScale = 30;//月的进制
        private int seasonScale = 3;//季节进制

        public bool TimePause;//时间的暂停
        private float Timer;//计时器

        private void Awake()
        {
            InitTime();
        }

        private void Update()
        {
            if (!TimePause)
            {
                Timer = Time.deltaTime;
                if (Timer >= SecondThreshold)
                {
                    Timer -= SecondThreshold;
                    UpDataTime();
                }
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        private void UpDataTime()
        {
            gameSecond++;
            if (gameSecond > secondScale)
            {
                gameMinute++;
                gameSecond = 0;
                if (gameMinute > minuteScale)
                {
                    gameHour++;
                    gameMinute = 0;
                    if (gameHour > hourScale)
                    {
                        gameDay++;
                        gameHour = 0;
                        if (gameDay > dayScale)
                        {
                            gameMonth++;
                            gameDay = 1;
                            if (gameMonth > 12)
                            {
                                gameMonth = 1;
                            }

                            monthInSeason--;
                            if (monthInSeason == 0)
                            {
                                monthInSeason = 3;
                                int seasonNumber = (int)gameSeason;
                                seasonNumber++;
                                if (seasonNumber > seasonScale)
                                {
                                    seasonNumber = 0;
                                    gameYear++;
                                }

                                gameSeason = (Enums.Season)seasonNumber;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化游戏时间
        /// </summary>
        private void InitTime()
        {
            gameSecond = 0;
            gameMinute = 0;
            gameHour = 0;
            gameDay = 1;
            gameMonth = 2;
            gameYear = 2024;
            gameSeason = Enums.Season.Spring;
        }
    }
}
