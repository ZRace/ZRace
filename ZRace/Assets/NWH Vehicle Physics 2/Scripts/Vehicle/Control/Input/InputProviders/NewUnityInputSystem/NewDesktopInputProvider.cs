using UnityEngine;

namespace NWH.VehiclePhysics2.Input
{
    public class NewDesktopInputProvider : InputProvider
    {
        private VehicleInputActions _inputActions;
        private Vector2 _movement;
        private float _clutch;
        private float _handbrake;

        public new void Awake()
        {
            base.Awake();
            
            _inputActions = new VehicleInputActions();
            _inputActions.VehicleControls.Movement.performed += ctx => _movement = ctx.ReadValue<Vector2>();
            _inputActions.VehicleControls.Clutch.performed += ctx => _clutch = ctx.ReadValue<float>();
            _inputActions.VehicleControls.Handbrake.performed += ctx => _handbrake = ctx.ReadValue<float>();
        }

        public void OnEnable()
        {
            _inputActions.Enable();   
        }

        public void OnDisable()
        {
            _inputActions.Disable();
        }

        public override bool ChangeCamera()
        {
            return _inputActions.VehicleControls.ChangeCamera.triggered;
        }

        public override bool ChangeVehicle()
        {
            return _inputActions.VehicleControls.ChangeVehicle.triggered;
        }

        public override float Clutch()
        {
            return _clutch;
        }

        public override bool EngineStartStop()
        {
            return _inputActions.VehicleControls.EngineStartStop.triggered;
        }

        public override bool ExtraLights()
        {
            return _inputActions.VehicleControls.ExtraLights.triggered;
        }

        public override bool HighBeamLights()
        {
            return _inputActions.VehicleControls.FullBeamLights.triggered;
        }

        public override float Handbrake()
        {
            return _handbrake;
        }

        public override bool HazardLights()
        {
            return _inputActions.VehicleControls.HazardLights.triggered;
        }

        public override float Horizontal()
        {
            return _movement.x;
        }

        public override bool Horn()
        {
            return _inputActions.VehicleControls.Horn.triggered;
        }

        public override bool LeftBlinker()
        {
            return _inputActions.VehicleControls.LeftBlinker.triggered;
        }

        public override bool LowBeamLights()
        {
            return _inputActions.VehicleControls.LowBeamLights.triggered;
        }

        public override bool RightBlinker()
        {
            return _inputActions.VehicleControls.RightBlinker.triggered;
        }

        public override bool ShiftDown()
        {
            return _inputActions.VehicleControls.ShiftDown.triggered;
        }

        public override bool ShiftUp()
        {
            return _inputActions.VehicleControls.ShiftUp.triggered;
        }

        /// <summary>
        /// Used for H-shifters and direct shifting into gear on non-sequential gearboxes.
        /// </summary>
        public override int ShiftInto()
        {
            if (_inputActions.VehicleControls.ShiftIntoR1.triggered)
            {
                return -1;
            }
            else if (_inputActions.VehicleControls.ShiftInto0.triggered)
            {
                return 0;
            }
            else if (_inputActions.VehicleControls.ShiftInto1.triggered)
            {
                return 1;
            }
            else if (_inputActions.VehicleControls.ShiftInto2.triggered)
            {
                return 2;
            }
            else if (_inputActions.VehicleControls.ShiftInto3.triggered)
            {
                return 3;
            }
            else if (_inputActions.VehicleControls.ShiftInto4.triggered)
            {
                return 4;
            }
            else if (_inputActions.VehicleControls.ShiftInto5.triggered)
            {
                return 5;
            }
            else if (_inputActions.VehicleControls.ShiftInto6.triggered)
            {
                return 6;
            }
            else if (_inputActions.VehicleControls.ShiftInto7.triggered)
            {
                return 7;
            }
            else if (_inputActions.VehicleControls.ShiftInto8.triggered)
            {
                return 8;
            }

            return -999;
        }

        public override bool TrailerAttachDetach()
        {
            return _inputActions.VehicleControls.TrailerAttachDetach.triggered;
        }

        public override float Vertical()
        {
            return _movement.y;
        }

        public override bool FlipOver()
        {
            return _inputActions.VehicleControls.FlipOver.triggered;
        }

        public override bool Boost()
        {
            return _inputActions.VehicleControls.Boost.triggered;
        }

        public override bool CruiseControl()
        {
            return _inputActions.VehicleControls.CruiseControl.triggered;
        }
    }
}