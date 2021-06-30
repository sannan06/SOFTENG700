// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using Wave.Native;

#if UNITY_EDITOR
namespace Wave.Essence.Editor
{
	public class WaveEditor : MonoBehaviour
	{
		private static string LOG_TAG = "WaveEditor";
		private void DEBUG(string msg)
		{
			if (Log.EnableDebugLog)
				Log.d(LOG_TAG, msg, true);
		}

		private void INFO(string msg)
		{
			Log.i(LOG_TAG, msg, true);
		}

		private static WaveEditor instance = null;
		public static WaveEditor Instance
		{
			get
			{
				if (instance == null)
				{
					var gameObject = new GameObject("WaveEditor");
					instance = gameObject.AddComponent<WaveEditor>();
					// This object should survive all scene transitions.
					GameObject.DontDestroyOnLoad(instance);
				}
				return instance;
			}
		}

		private WVR_Event_t wvrEvent = new WVR_Event_t();
		private void HandleEvent()
		{
			WVR_DeviceType device_type = wvrEvent.device.type;
			WVR_InputId input_id = wvrEvent.input.inputId;

			switch (wvrEvent.common.type)
			{
				case WVR_EventType.WVR_EventType_ButtonPressed:
					DEBUG("HandleEvent() WVR_EventType_ButtonPressed: " + device_type + ", " + input_id);
					if (DummyButton.Instance != null)
						DummyButton.Instance.SetPressButton(device_type, input_id, true);
					break;
				case WVR_EventType.WVR_EventType_ButtonUnpressed:
					DEBUG("HandleEvent() WVR_EventType_ButtonUnpressed: " + device_type + ", " + input_id);
					if (DummyButton.Instance != null)
						DummyButton.Instance.SetPressButton(device_type, input_id, false);
					break;
				case WVR_EventType.WVR_EventType_TouchTapped:
					DEBUG("HandleEvent() WVR_EventType_TouchTapped: " + device_type + ", " + input_id);
					if (DummyButton.Instance != null)
						DummyButton.Instance.SetTouchButton(device_type, input_id, true);
					break;
				case WVR_EventType.WVR_EventType_TouchUntapped:
					DEBUG("HandleEvent() WVR_EventType_TouchUntapped: " + device_type + ", " + input_id);
					if (DummyButton.Instance != null)
						DummyButton.Instance.SetTouchButton(device_type, input_id, false);
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// WVR_DeviceType_Invalid = 0,
		/// WVR_DeviceType_HMD = 1,
		/// WVR_DeviceType_Controller_Right = 2,
		/// WVR_DeviceType_Controller_Left = 3,
		/// </summary>
		public static readonly int DeviceCount = 4;
		void Awake()
		{
			instance = this;
		}

		void Update()
		{
			if (Interop.WVR_PollEventQueue(ref wvrEvent))
				HandleEvent();
		}
	}
}
#endif
