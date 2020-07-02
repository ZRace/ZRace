using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class TrackRandomSceneList : MonoBehaviour
    {
        [SerializeField] protected int indexNumberToTrack = 0;
        [SerializeField] protected Image[] images = new Image[] { };
        [SerializeField] protected Text[] texts = new Text[] { };
        protected UICoreLogic logic;
        protected SceneOption option;

        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();
        }

        protected virtual void FixedUpdate()
        {
            option = logic.GetRandomSceneNumber(indexNumberToTrack);
            SetText(option.sceneName);
            SetSprite(option.sceneSprite);
        }
        protected virtual void SetText(string inputText)
        {
            if (string.IsNullOrEmpty(inputText)) return;
            foreach (Text text in texts)
            {
                text.text = inputText;
            }
        }
        protected virtual void SetSprite(Sprite newImage)
        {
            if (newImage == null) return;
            foreach (Image image in images)
            {
                image.sprite = newImage;
            }
        }
    }
}