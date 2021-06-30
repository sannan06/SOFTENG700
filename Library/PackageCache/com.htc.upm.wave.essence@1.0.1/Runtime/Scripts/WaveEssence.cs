// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC\u2019s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using UnityEngine.XR;
using Wave.Native;
using Wave.Essence.Events;

namespace Wave.Essence
{
	public class WaveEssence : MonoBehaviour
	{
		private const string LOG_TAG = "Wave.XR.WaveEssence";
		private void DEBUG(string msg)
		{
			if (Log.EnableDebugLog)
			{
				Log.d(LOG_TAG, msg, true);
			}
		}

		private static WaveEssence instance = null;
		public static WaveEssence Instance
		{
			get
			{
				if (instance == null)
				{
					var gameObject = new GameObject("WaveEssence");
					instance = gameObject.AddComponent<WaveEssence>();
					// This object should survive all scene transitions.
					GameObject.DontDestroyOnLoad(instance);
				}
				return instance;
			}
		}

		#region Wave Native Interface
		WVR_Event_t wvrEmptyEvent = new WVR_Event_t();
		#endregion

		#region XR Interface
		public bool IsLeftHanded { get; private set; } = false;
		private XRNode GetLeftHandedNode(XR_Device device)
		{
			if (device == XR_Device.Head)
				return XRNode.CenterEye;
			if (device == XR_Device.Dominant)
				return (this.IsLeftHanded ? XRNode.LeftHand : XRNode.RightHand);
			if (device == XR_Device.NonDominant)
				return (this.IsLeftHanded ? XRNode.RightHand : XRNode.LeftHand);
			return XRNode.HardwareTracker;
		}
		#endregion


		void Awake()
		{
			instance = this;
		}

		void Start()
		{
			Log.i(LOG_TAG, "Start() Check the device default role.");
			OnRoleChange(wvrEmptyEvent);

		}

		void OnApplicationPause(bool pauseStatus)
		{
			Log.i(LOG_TAG, "OnApplicationPause() pauseStatus: " + pauseStatus, true);
			if (!pauseStatus)
			{
				Log.i(LOG_TAG, "OnApplicationPause() Check the device default role in resume.");
				OnRoleChange(wvrEmptyEvent);
			}
		}

		private void OnEnable()
		{
			SystemEvent.Listen(WVR_EventType.WVR_EventType_DeviceRoleChanged, OnRoleChange);
		}

		private void OnDisable()
		{
			SystemEvent.Remove(WVR_EventType.WVR_EventType_DeviceRoleChanged, OnRoleChange);
		}

		private void OnRoleChange(WVR_Event_t systemEvent)
		{
			WVR_DeviceType default_role = Interop.WVR_GetDefaultControllerRole();
			this.IsLeftHanded = (default_role == WVR_DeviceType.WVR_DeviceType_Controller_Left ? true : false);
			DEBUG("OnRoleChange() Left handed mdoe ? " + this.IsLeftHanded);
		}

		void Update()
		{
			/**
			 * Get the InputFeature of center eye (reference as head).
			 **/
			/**
			 * Get the InputFeature of left hand.
			 **/
			/**
			 * Get the InputFeature of right hand.
			 **/

			/**
			 * Polling Wave Native status.
			 **/
		}   // Update

		/*
		#region Button
		public bool GetButton(XR_Device device, XR_BinaryButton button)
		{
			bool value = false;
			switch (button)
			{
				case XR_BinaryButton.GripPress:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.GripPressFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.MenuPress:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.MenuPressFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.ThumbstickPress:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.ThumbstickPressFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.ThumbstickTouch:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.ThumbstickTouchFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.TouchpadPress:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.TouchpadPressFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.TouchpadTouch:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.TouchpadTouchFeature, out value))
						value = false;
					break;
				case XR_BinaryButton.TriggerPress:
					if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.TriggerPressFeature, out value))
						value = false;
					break;
				default:
					value = false;
					break;
			}

			return value;
		}
		#endregion

		public bool IsPoseValid(XR_Device device)
		{
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				return true;
			}
			else
#endif
			{
				bool value = false;
				if (!InputDevices.GetDeviceAtXRNode(GetLeftHandedNode(device)).TryGetFeatureValue(XR_Feature.ValidPoseFeature, out value))
					value = false;
				return value;
			}
		}

		private HapticCapabilities hapticCaps = new HapticCapabilities();
		/// <summary>
		/// Trigger vibration on a device.
		/// </summary>
		/// <param name="device">The vibration device.</param>
		/// <param name="amplitude">[0, 1] The intensity of vibration.</param>
		/// <param name="duration">The vibration duration in seconds. Default 1 second.</param>
		public void TriggerVibration(XR_Device device, float amplitude, float duration = 1)
		{
			XRNode deviceNode = GetLeftHandedNode(device);
			amplitude = Mathf.Clamp(amplitude, 0, 1);
			DEBUG("TriggerVibration() " + device + ", amplitude: " + amplitude + ", duration: " + duration);

			if (InputDevices.GetDeviceAtXRNode(deviceNode).TryGetHapticCapabilities(out hapticCaps))
			{
				DEBUG("TriggerVibration() Haptic capabilities --\n"
					+ "numChannels: " + hapticCaps.numChannels + "\n"
					+ "supportsImpulse: " + hapticCaps.supportsImpulse + "\n"
					+ "supportsBuffer: " + hapticCaps.supportsBuffer + "\n"
					+ "bufferFrequencyHz: " + hapticCaps.bufferFrequencyHz + "\n"
					+ "bufferMaxSize: " + hapticCaps.bufferMaxSize + "\n"
					+ "bufferOptimalSize: " + hapticCaps.bufferOptimalSize);

				if (hapticCaps.supportsImpulse)
					InputDevices.GetDeviceAtXRNode(deviceNode).SendHapticImpulse(0, amplitude, duration);
			}
		}
		*/
	}
}