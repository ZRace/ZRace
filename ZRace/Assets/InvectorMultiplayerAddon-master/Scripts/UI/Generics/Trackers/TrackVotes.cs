using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class TrackVotes : MonoBehaviour
    {
        [SerializeField] protected int indexNumberToTrack = 0;
        [SerializeField] protected Text[] texts = new Text[] { };
        protected UICoreLogic logic;

        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();
        }

        protected virtual void FixedUpdate()
        {
            SetText(logic.GetSceneVotes(indexNumberToTrack).ToString());
        }
        protected virtual void SetText(string inputText)
        {
            foreach (Text text in texts)
            {
                text.text = inputText;
            }
        }
    }
}