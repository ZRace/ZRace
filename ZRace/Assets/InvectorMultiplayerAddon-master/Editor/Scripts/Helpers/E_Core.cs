using System.IO;
using UnityEngine;

namespace CBGames.Editors
{
    public class E_Core : MonoBehaviour
    {   
        public static string e_guiSkinPath = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}CBSkin.guiskin", Path.DirectorySeparatorChar);
        public static string e_guiTitleImgPath = string.Format("InvectorMultiplayer{0}Editor{0}Images{0}CBTitle.png", Path.DirectorySeparatorChar);
        public static string e_guiBoxPath = string.Format("InvectorMultiplayer{0}Editor{0}Images{0}Box.png", Path.DirectorySeparatorChar);
        public static string e_invectorMPTitle = string.Format("{1}{0}Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}InvectorMultiTransp.png", Path.DirectorySeparatorChar, Directory.GetCurrentDirectory());

        public static string h_cameraPath = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Camera_Icon.png", Path.DirectorySeparatorChar);
        public static string h_genericIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Generic_Icon.png", Path.DirectorySeparatorChar);
        public static string h_networkIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Network_Icon.png", Path.DirectorySeparatorChar);
        public static string h_playerIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Player_Icon.png", Path.DirectorySeparatorChar);
        public static string h_spawnPointIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}SpawnPoint.png", Path.DirectorySeparatorChar);
        public static string h_respawnPointIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}RespawnPoint.png", Path.DirectorySeparatorChar);
        public static string h_textChatIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}TextChat_Icon.png", Path.DirectorySeparatorChar);
        public static string h_sceneEntranceIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}SceneEnterPoint.png", Path.DirectorySeparatorChar);
        public static string h_sceneExitIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}SceneTransition.png", Path.DirectorySeparatorChar);
        public static string h_voiceIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Voice_Icon.png", Path.DirectorySeparatorChar);
        public static string h_playerlistIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}PlayerList_Icon.png", Path.DirectorySeparatorChar);
        public static string h_deathCameraIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}DeathCam_Icon.png", Path.DirectorySeparatorChar);
        public static string h_floatingBarIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}FloatingBar_Icon.png", Path.DirectorySeparatorChar);
        public static string h_uiIcon = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}UI_Icon.png", Path.DirectorySeparatorChar);

        public static string i_b_chest = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}Chest.png", Path.DirectorySeparatorChar);
        public static string i_b_head = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}Head.png", Path.DirectorySeparatorChar);
        public static string i_b_lfoot = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LFoot.png", Path.DirectorySeparatorChar);
        public static string i_b_lhand = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LHand.png", Path.DirectorySeparatorChar);
        public static string i_b_llleg = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LLLeg.png", Path.DirectorySeparatorChar);
        public static string i_b_llarm = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LLArm.png", Path.DirectorySeparatorChar);
        public static string i_b_luarm = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LUArm.png", Path.DirectorySeparatorChar);
        public static string i_b_luleg = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}LULeg.png", Path.DirectorySeparatorChar);
        public static string i_b_neck = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}Neck.png", Path.DirectorySeparatorChar);
        public static string i_b_rfoot = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RFoot.png", Path.DirectorySeparatorChar);
        public static string i_b_rhand = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RHand.png", Path.DirectorySeparatorChar);
        public static string i_b_rlarm = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RLArm.png", Path.DirectorySeparatorChar);
        public static string i_b_ruarm = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RUArm.png", Path.DirectorySeparatorChar);
        public static string i_b_rlleg = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RLLeg.png", Path.DirectorySeparatorChar);
        public static string i_b_ruleg = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}RULeg.png", Path.DirectorySeparatorChar);
        public static string i_b_spine = string.Format("Assets{0}InvectorMultiplayer{0}Editor{0}Images{0}Bones{0}Spine.png", Path.DirectorySeparatorChar);
    }
}
