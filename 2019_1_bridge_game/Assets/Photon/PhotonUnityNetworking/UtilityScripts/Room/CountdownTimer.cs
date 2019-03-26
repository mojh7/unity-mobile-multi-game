// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountdownTimer.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
// You can do this from Unity's OnDisable function for example.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Pun;

namespace Photon.Pun.UtilityScripts
{
    /*
    /// <summary>
    /// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
    /// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
    /// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
    /// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    /// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
    /// You can do this from Unity's OnDisable function for example.
    /// </summary>
    */
    /// <summary>
    /// 이것은 기본적인 CountdownTimer입니다. 타이머를 기동하기 위해서, MasterClient는 커스텀 룸 프로퍼티에 특정의 엔트리를 추가 할 수 있습니다.
    /// 속성의 이름 'StartTime'과 순간을 설명하는 실제 시작 시간이 포함되어 있으면 타이머가 시작되었습니다.
    /// 동기화 타이머를 사용하려면 PhotonNetwork.Time을 사용하는 것이 가장 좋습니다.
    /// CountdownTimerHasExpired 이벤트에 가입하려면 CountdownTimer.OnCountdownTimerHasExpired + = OnCountdownTimerIsExpired를 호출 할 수 있습니다.
    /// Unity의 OnEnable 함수에서 예를 들어. 수신 거부를 위해 단순히 CountdownTimer.OnCountdownTimerHasExpired - = OnCountdownTimerIsExpired를 호출하십시오.
    /// 예를 들어 Unity의 OnDisable 함수에서 이 작업을 수행 할 수 있습니다.
    /// </summary>
    public class CountdownTimer : MonoBehaviourPunCallbacks
    {
        public const string CountdownStartTime = "StartTime";
        /// <summary>
        /// 만료된 카운트다운 타이머 델리게이트
        /// OnCountdownTimerHasExpired delegate.
        /// </summary>
        public delegate void CountdownTimerHasExpired();
        /// <summary>
        /// 타이머가 만료하면 호출 됨.
        /// Called when the timer has expired.
        /// </summary>
        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

        private bool isTimerRunning;
        private float startTime;

        // 카운트 다운을 시각화하기위한 텍스트 구성 요소에 대한 참조
        [Header("Reference to a Text component for visualizing the countdown")]
        public Text Text;

        [Header("Countdown time in seconds")]
        public float Countdown = 5.0f;

        public void Start()
        {
            if (Text == null)
            {
                // 'Text'에 대한 참조가 설정되지 않았습니다.유효한 참조를 설정하십시오.
                Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                return;
            }
        }

        public void Update()
        {
            if (!isTimerRunning)
            {
                return;
            }

            float timer = (float)PhotonNetwork.Time - startTime;
            float countdown = Countdown - timer;

            Text.text = string.Format("Game starts in {0} seconds", countdown.ToString("n2"));

            if (countdown > 0.0f)
            {
                return;
            }

            isTimerRunning = false;

            Text.text = string.Empty;

            if (OnCountdownTimerHasExpired != null)
            {
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                isTimerRunning = true;
                startTime = (float)startTimeFromProps;
            }
        }
    }
}