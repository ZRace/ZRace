using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class Counter : MonoBehaviour
    {
        public enum CounterType { CountDown, CountUp }
        [Tooltip("The text object that will display the number as it counts up/down.")]
        [SerializeField] private Text counter = null;
        [Tooltip("Do you want this to count up or down to the target amount?")]
        [SerializeField] private CounterType counterType = CounterType.CountDown;

        private bool counting = false;
        float currentNumber = 0.0f;
        float targetNumber = 0.0f;

        public void StartCounting(float amount)
        {
            targetNumber = amount;
            switch (counterType)
            {
                case CounterType.CountDown:
                    counter.text = amount.ToString();
                    currentNumber = amount;
                    break;
                case CounterType.CountUp:
                    counter.text = "0";
                    currentNumber = 0;
                    break;
            }
            counting = true;
        }

        private void Update()
        {
            if (counting == true)
            {
                switch (counterType)
                {
                    case CounterType.CountDown:
                        currentNumber -= Time.deltaTime;
                        counter.text = (currentNumber > 0) ? currentNumber.ToString("f0") : "0";
                        if (currentNumber <= 0)
                        {
                            counting = false;
                        }
                        break;
                    case CounterType.CountUp:
                        currentNumber += Time.deltaTime;
                        counter.text = (currentNumber < targetNumber) ? currentNumber.ToString("f0") : targetNumber.ToString(); ;
                        if (currentNumber >= targetNumber)
                        {
                            counting = false;
                        }
                        break;
                }
            }
        }
    }
}