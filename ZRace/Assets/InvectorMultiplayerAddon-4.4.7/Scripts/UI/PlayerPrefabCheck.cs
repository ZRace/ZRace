using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.UI
{
    public class PlayerPrefabCheck : MonoBehaviour
    {
        [SerializeField] private ExampleUI exampleUI = null;
        [SerializeField] private int mustHavePlayers = 1;

        void Awake()
        {
            if (exampleUI.players.Length < mustHavePlayers)
            {
                gameObject.SetActive(false);
            }
        }
    }
}