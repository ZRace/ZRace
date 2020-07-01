using CBGames.Core;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CBGames.UI
{
    public enum CounterStartType { Immediately, OnCall };
    public enum NumberType { WholeNumber, FullTime, AbbreviatedTime, Raw }

    public class GenericCountDown : MonoBehaviour
    {
        [Tooltip("Only perform the UnityEvents if you are the owner/not the owner.")]
        [SerializeField] protected bool useRoomOwnerShip = false;
        [Tooltip("If you are the owner then only perform the OnZero event.")]
        [SerializeField] protected bool ifIsOwner = false;
        [Tooltip("The time the timer will start counting down from.")]
        [SerializeField] protected float startTime = 6.0f;
        [Tooltip("How fast to count down? Higher = faster, Lower = Slower")]
        [SerializeField] protected float countSpeed = 1.0f;
        [Tooltip("If you want to countdown based off the time on the photon server. Great for " +
            "having everyone keep the same time. However this is not responsible for starting " +
            "everyone at the same time. Just counting down at the same speed!")]
        [SerializeField] protected bool syncWithPhotonServer = false;
        [Tooltip("When do you want to start counting?\n" +
            "Immediately=The OnStart call will trigger the counting sequence.\n" +
            "OnCall=An outside source will have to call the StartCounting function.")]
        [SerializeField] protected CounterStartType startType = CounterStartType.OnCall;
        [Tooltip("How do you want the number to be displayed? \n" +
            "WholeNumber = Display in integer format\n\n" +
            "FullTime = Display like 00:00:00\n\n" +
            "AbbreviatedTime = Display like 00:00\n\n" +
            "Raw = Display the raw float value")]
        [SerializeField] protected NumberType numberType = NumberType.WholeNumber;
        [Tooltip("(Optional)Texts to overwrite and show the current counted number.")]
        [SerializeField] protected Text[] texts = new Text[] { };
        [Tooltip("(Optional) The audio source to play the tick clip sound.")]
        [SerializeField] protected AudioSource soundSource = null;
        [Tooltip("(Optional) The audio clip to play every time a whole number goes down by 1.")]
        [SerializeField] protected AudioClip tickClip = null;
        public UnityEvent OnStartCounting = new UnityEvent();
        public UnityEvent OnStopCounting = new UnityEvent();
        public FloatUnityEvent OnNumberChange = new FloatUnityEvent();
        public UnityEvent OnZero = new UnityEvent();

        protected bool _startCounting;
        protected bool _invoked = false;
        [HideInInspector] [SerializeField] protected float _time = 0.0f;
        protected float _actualTime = 0.0f;
        protected float _prevTime = 0.0f;
        protected TimeSpan _ts;
        protected string _timeString;
        protected float _startTime = 0;
        protected double _syncedStartTime = 0;

        #region EditorVars
        [HideInInspector] public bool showUnityEvents = false;
        #endregion
        protected virtual void Start()
        {
            if (startType == CounterStartType.Immediately)
            {
                StartCounting();
            }
            if (soundSource != null && tickClip != null)
            {
                soundSource.clip = tickClip;
            }
        }
        public virtual void SetTime(float incomingTime)
        {
            _time = incomingTime;
        }
        public virtual void SetStartTime(float timeToStart)
        {
            startTime = timeToStart;
        }
        public virtual void SubtractTime(float subtractTime)
        {
            //This function is great for syncronizing the starting time between networked players.
            //EX: send w/ PhotonNetwork.Time in rpc call.
            //    Recieve that time make another PhotonNetwork.Time call and subtract the difference here.
            //EX: SubtractTime(PhotonNetwork.Time - receivedRPCTime);
            _time -= subtractTime;
        }
        public virtual void SetSyncTime(double startTime)
        {
            _syncedStartTime = startTime;
        }
        public virtual void SetClip(AudioClip clip)
        {
            tickClip = clip;
        }
        public virtual void SetAudioSource(AudioSource source)
        {
            soundSource = source;
        }
        public virtual void StartCounting()
        {
            if (_startCounting == true || enabled == false) return;
            _time = startTime;
            if (_syncedStartTime == 0 && syncWithPhotonServer == true)
            {
                _syncedStartTime = PhotonNetwork.Time;
            }
            _actualTime = _time;
            _prevTime = _time;
            _startCounting = true;
            OnStartCounting.Invoke();
        }
        public virtual void StopCounting()
        {
            if (_startCounting == false) return;
            _invoked = false;
            _time = startTime;
            _actualTime = _time;
            _prevTime = _time;
            _startCounting = false;
            OnStopCounting.Invoke();
        }
        protected virtual void SetTexts(string value)
        {
            foreach(Text text in texts)
            {
                text.text = value;
            }
        }
        protected virtual void PlayAudioClip()
        {
            if (soundSource != null && soundSource.clip != null)
            {
                soundSource.Play();
            }
        }
        protected virtual void Update()
        {
            if (_startCounting == true)
            {
                if (syncWithPhotonServer == true)
                {
                    if (_startTime == 0)
                    {
                        _startTime = _time;
                    }
                    _time = _startTime - (float)(PhotonNetwork.Time - _syncedStartTime);
                }
                else
                {
                    _time -= Time.deltaTime * countSpeed;
                }
                switch (numberType)
                {
                    case NumberType.WholeNumber:
                        _actualTime = Mathf.Round(_time);
                        _timeString = _actualTime.ToString();
                        break;
                    case NumberType.FullTime:
                        _ts = TimeSpan.FromSeconds(_time);
                        _actualTime = _ts.Seconds;
                        _timeString = string.Format("{0:00}:{1:00}:{1:00}", _ts.Minutes, _ts.Seconds, _ts.Milliseconds);
                        break;
                    case NumberType.AbbreviatedTime:
                        _ts = TimeSpan.FromSeconds(_time);
                        _actualTime = _ts.Seconds;
                        _timeString = string.Format("{0:00}:{1:00}", _ts.Minutes, _ts.Seconds);
                        break;
                    case NumberType.Raw:
                        _actualTime = _time;
                        _timeString = _actualTime.ToString();
                        break;
                }
                if (_actualTime != _prevTime)
                {
                    _prevTime = _actualTime;
                    PlayAudioClip();
                    OnNumberChange.Invoke(_time);
                }
                if (texts.Length > 0)
                {
                    SetTexts(_timeString);
                }
                if (_time <= 0)
                {
                    if (_invoked == false && 
                        (useRoomOwnerShip == false || (
                            useRoomOwnerShip == true && PhotonNetwork.IsMasterClient == ifIsOwner
                            )
                         )
                    )
                    {
                        OnZero.Invoke();
                    }
                    StopCounting();
                }
            }
        }
    }
}