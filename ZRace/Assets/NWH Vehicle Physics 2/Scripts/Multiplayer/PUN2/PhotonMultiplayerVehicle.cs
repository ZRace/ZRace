using NWH.VehiclePhysics2.Sound.SoundComponents;
using Photon.Pun;
using UnityEngine;

namespace NWH.VehiclePhysics2.Multiplayer
{
    /// <summary>
    /// Adds multi-player functionality to a vehicle through Photon Unity Networking 2.
    /// </summary>
    [RequireComponent(typeof(PhotonRigidbodyView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(VehicleController))]
    public class PhotonMultiplayerVehicle : MonoBehaviour, IPunObservable
    {
        public int sendRate = 25;
        public int serializationRate = 25;
        private PhotonRigidbodyView _photonRigidbodyView;
        private PhotonTransformView _photonTransformView;
        private PhotonLiveRemote _photonLiveRemote;
        private PhotonPuntuation _photonPuntuation;
        private PhotonView _photonView;


        private VehicleController _vehicleController;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Steering
                stream.SendNext(_vehicleController.input.Vertical);
                stream.SendNext(_vehicleController.input.Horizontal);

                // Sound
                foreach (SoundComponent sc in _vehicleController.soundManager.components)
                {
                    stream.SendNext(sc.GetVolume());
                    stream.SendNext(sc.GetPitch());
                }

                stream.SendNext(_vehicleController.effectsManager.lightsManager.GetByteState());
            }
            else
            {
                // Steering
                _vehicleController.input.autoSettable = false;
                _vehicleController.input.Vertical = (float)stream.ReceiveNext();
                _vehicleController.input.Horizontal = (float) stream.ReceiveNext();

                // Sound
                foreach (SoundComponent sc in _vehicleController.soundManager.components)
                {
                    sc.SetVolume((float) stream.ReceiveNext());
                    sc.SetPitch((float) stream.ReceiveNext());
                }

                //_vehicleController.effectsManager.lightsManager.SetStatesFromByte((byte) stream.ReceiveNext());
            }
        }

        public void Setup()
        {
            _photonView = GetComponent<PhotonView>();
            _photonRigidbodyView = GetComponent<PhotonRigidbodyView>();
            _photonTransformView = GetComponent<PhotonTransformView>();
            _photonLiveRemote = GetComponent<PhotonLiveRemote>();
            _photonPuntuation = GetComponent<PhotonPuntuation>();

			_photonView.ObservedComponents.Clear();
			_photonView.ObservedComponents.Add(_photonRigidbodyView);
            _photonView.ObservedComponents.Add(_photonTransformView);
            _photonView.ObservedComponents.Add(_photonLiveRemote);
            _photonView.ObservedComponents.Add(_photonPuntuation);


            _photonView.ObservedComponents.Add(this);
        }

        private void Awake()
        {
            _vehicleController = GetComponent<VehicleController>();

            Setup();

            PhotonNetwork.SendRate = sendRate;
            PhotonNetwork.SerializationRate = serializationRate;

            if (_photonView.IsMine)
            {
                _vehicleController.multiplayerInstanceType = VehicleController.MultiplayerInstanceType.Local;
            }
            else
            {
                _vehicleController.multiplayerInstanceType = VehicleController.MultiplayerInstanceType.Remote;
            }
        }
    }
}