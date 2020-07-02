using CBGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class BroadcastReceiver : MonoBehaviour
    {
        [Tooltip("The ScrollView object in this UI.")]
        [SerializeField] private GameObject scrollView = null;
        [Tooltip("The content object of the scrollView.")]
        [SerializeField] private GameObject content = null;
        [Tooltip("How long to have the message be visible before it is removed.")]
        [SerializeField] private float messageDestroyTime = 15.0f;
        [Space(10)]
        [Header("MESSAGE TYPES")]
        [Tooltip("The prefab that will be instantiated as a child of the content when " +
            "this receive a \"DEATH\" broadcast message.")]
        [SerializeField] private GameObject deathMessage = null;

        private bool isEnabled = false;

        void Awake()
        {
            scrollView.SetActive(false);
        }

        public void ReceiveBroadCastMessage(BroadCastMessage message)
        {
            isEnabled = true;
            scrollView.SetActive(true);
            if (message.speaker == "DEATH")
            {
                InstantiateDeathMessage(message.message);
            }
        }
        void InstantiateDeathMessage(string message)
        {
            GameObject msgObject = (GameObject)Instantiate(deathMessage);
            if (msgObject.GetComponent<Text>())
            {
                msgObject.GetComponent<Text>().text = message;
            }
            else
            {
                msgObject.GetComponentInChildren<Text>().text = message;
            }
            msgObject.transform.SetParent(content.transform);
            msgObject.transform.localScale = new Vector3(1, 1, 1);
            Destroy(msgObject, messageDestroyTime);
        }
        private void Update()
        {
            if (isEnabled == true)
            {
                if (content.transform.childCount < 1)
                {
                    isEnabled = false;
                    scrollView.SetActive(false);
                }
            }
        }
    }
}