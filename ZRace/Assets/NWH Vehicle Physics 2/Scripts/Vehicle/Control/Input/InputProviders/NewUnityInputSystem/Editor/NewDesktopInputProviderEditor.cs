using NWH.NUI;
using NWH.VehiclePhysics2.Input;
using UnityEditor;

namespace NWH.VehiclePhysics.Input
{
    [CustomEditor(typeof(NewDesktopInputProvider))]
    public class NewDesktopInputProviderEditor : NUIEditor
    {
        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI())
            {
                return false;
            }
            
            drawer.Info("Input settings for Unity's new input system can be changed by modifying 'VehicleInputActions' " +
                        "file (double click on it to open).");

            drawer.EndEditor(this);
            return true;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }
    }
}