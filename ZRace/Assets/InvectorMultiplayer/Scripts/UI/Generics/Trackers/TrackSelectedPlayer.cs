using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class TrackSelectedPlayer : MonoBehaviour
    {
        [SerializeField] protected Text[] texts = new Text[] { };
        protected UICoreLogic logic;
        protected string playerName = "";
        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();
        }

        protected virtual void FixedUpdate()
        {
            SetText(logic.GetSetPlayer());
        }
        protected virtual void SetText(GameObject player)
        {
            if (player == null) return;
            playerName = player.name;

            foreach(Text text in texts)
            {
                text.text = playerName;
            }
        }
    }
}