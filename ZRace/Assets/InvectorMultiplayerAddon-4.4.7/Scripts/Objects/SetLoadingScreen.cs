using CBGames.UI;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.Objects {
    public class SetLoadingScreen : MonoBehaviour
    {
        [SerializeField] protected List<Sprite> LoadingImages = new List<Sprite>();
        [SerializeField] protected string LoadingTitle = "";
        [SerializeField] protected List<string> LoadingDescriptions = new List<string>();

        protected ExampleUI ui;
        protected UICoreLogic logic;

        public virtual void SetLoadingScreenItems()
        {
            logic = FindObjectOfType<UICoreLogic>();
            if (logic != null)
            {
                logic.loadingImages = (LoadingImages.Count > 0) ? LoadingImages : logic.loadingImages;
                logic.loadingTitle = LoadingTitle;
                logic.loadingDesc = (LoadingDescriptions.Count > 0) ? LoadingDescriptions : logic.loadingDesc;
            }
            else
            {
                ui = FindObjectOfType<ExampleUI>();
                if (ui != null)
                {
                    if (LoadingImages.Count > 0) ui.SetLoadingImages(LoadingImages);
                    ui.SetLoadingTitleText(LoadingTitle);
                    ui.ResetLoadingBar();
                    if (LoadingDescriptions.Count > 0) ui.SetLoadingDescriptionText(LoadingDescriptions);
                }
            }
        }

        public virtual void EnableLoadingScreen()
        {
            SetLoadingScreenItems();
            if (logic != null)
            {
                logic.EnableLoadingPage(true);
            }
            else
            {
                ui.EnableLoadingPage(true);
            }
        }
    }
}