using NWH.NUI;
using UnityEditor;

namespace NWH.VehiclePhysics2.Multiplayer
{
    [CustomEditor(typeof(PhotonMultiplayerVehicle))]
    [CanEditMultipleObjects]
    public class PhotonMultiplayerVehicleEditor : NUIEditor
    {
        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI())
            {
                return false;
            }

            PhotonMultiplayerVehicle pmv = target as PhotonMultiplayerVehicle;
            ;
            if (pmv == null)
            {
                drawer.EndEditor();
                return false;
            }

            drawer.Field("sendRate");
            drawer.Field("serializationRate");
            drawer.Info(
                "'Observe option' field of Photon View is not settable through scripting so make sure it is not set to 'Off'.",
                MessageType.Warning);
            if (drawer.Button("Setup"))
            {
                pmv.Setup();
            }

            drawer.EndEditor(this);
            return true;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }
    }
}