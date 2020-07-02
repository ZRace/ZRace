using CBGames.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class EnableIfChatConnected : MonoBehaviour
    {
        [SerializeField] protected Button[] buttons = new Button[] { };
        [SerializeField] protected GameObject[] gameObjects = new GameObject[] { };
        [SerializeField] protected bool invertActions = false;
        protected bool _isenabled = false;

        protected virtual void Update()
        {
            if (_isenabled == false)
            {
                if (NetworkManager.networkManager && NetworkManager.networkManager.GetChabox() &&
                    NetworkManager.networkManager.GetChabox().IsConnectedToDataChannel())
                {
                    _isenabled = true;
                    EnableButton((invertActions == true) ? false : true);
                }
            }
            else if (NetworkManager.networkManager.GetChabox().IsConnectedToDataChannel() == false)
            {
                _isenabled = false;
                EnableButton((invertActions == true) ? true : false);
            }
        }

        protected virtual void EnableButton(bool isEnabled)
        {
            foreach(Button button in buttons)
            {
                button.interactable = isEnabled;
            }
            foreach(GameObject go in gameObjects)
            {
                go.SetActive(isEnabled);
            }
        }
    }
}