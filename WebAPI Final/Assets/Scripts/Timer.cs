using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace V966
{
    [System.Serializable]
    public class Timer
    {
        private float maxTime;
        private float timeLeft;
        private bool started = false;



        public Timer(float mT)
        {
            maxTime = mT;
            timeLeft = maxTime;
        }

        public void resetTimer()
        {
            started = false;
            timeLeft = maxTime;
        }

        public void setTimer(float nMT)
        {
            maxTime = nMT;
        }

        public void startTimer()
        {
            resetTimer();
            started = true;
        }

        public void zeroTimer()
        {
            timeLeft = 0;
        }

        public void resumeTimer()
        {
            started = true;
        }

        public void pauseTimer()
        {
            started = false;
        }

        public void timerUpdate()
        {
            if (started)
                timeLeft -= Time.deltaTime;
        }

        public float getTimeLeft()
        {
            return timeLeft;
        }

        public float getPercent()
        {
            return timeLeft / maxTime;
        }

        public bool checkTimer()
        {
            if (timeLeft <= 0)
            {
                started = false;
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}