// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;
using Wave.Native;
using Wave.XR;
using Wave.XR.Function;

namespace Wave.Essence
{
	public static class ClientInterface
	{
		public static bool GetOrigin(ref WVR_PoseOriginModel wvrOrigin)
		{
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				wvrOrigin = WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnHead;
				return true;
			}
			else
#endif
			{
				return GetOrigin(Utils.InputSubsystem.GetTrackingOriginMode(), ref wvrOrigin);
			}
		}
		public static bool GetOrigin(in TrackingOriginModeFlags xrOrigin, ref WVR_PoseOriginModel wvrOrigin)
		{
			bool result = true;
			switch (xrOrigin)
			{
				case TrackingOriginModeFlags.Device:
					wvrOrigin = WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnHead;
					break;
				case TrackingOriginModeFlags.Floor:
					wvrOrigin = WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnGround;
					break;
				case TrackingOriginModeFlags.TrackingReference:
					wvrOrigin = WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnTrackingObserver;
					break;
				default:
					result = false;
					break;
			}

			return result;
		}

		public static string GetCurrentRenderModelName(WVR_DeviceType type)
		{
			string parameterName = "GetRenderModelName";
			IntPtr ptrParameterName = Marshal.StringToHGlobalAnsi(parameterName);
			IntPtr ptrResult = Marshal.AllocHGlobal(64);
			uint resultVertLength = 64;
			string tmprenderModelName = "";
			uint retOfRenderModel = Interop.WVR_GetParameters(type, ptrParameterName, ptrResult, resultVertLength);

			if (retOfRenderModel > 0)
				tmprenderModelName = Marshal.PtrToStringAnsi(ptrResult);

			Log.d("WaveAPIUtils", "Type: " + type + ", current render model name: " + tmprenderModelName);

			Marshal.FreeHGlobal(ptrParameterName);
			Marshal.FreeHGlobal(ptrResult);

			return tmprenderModelName;
		}
	} // class ClientInterface

	public static class Numeric
	{
		public static bool IsBoolean(string value)
		{
			try
			{
				bool i = Convert.ToBoolean(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsFloat(string value)
		{
			try
			{
				float i = Convert.ToSingle(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsNumeric(string value)
		{
			try
			{
				int i = Convert.ToInt32(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	} // class Numeric

	public class RenderFunctions
	{
		public delegate void SetPoseUsedOnSubmitDelegate([In, Out] WVR_PoseState_t[] poseState);
		private static SetPoseUsedOnSubmitDelegate setPoseUsedOnSubmit = null;
		public static SetPoseUsedOnSubmitDelegate SetPoseUsedOnSubmit
		{
			get
			{
				if (setPoseUsedOnSubmit == null)
					setPoseUsedOnSubmit = FunctionsHelper.GetFuncPtr<SetPoseUsedOnSubmitDelegate>("SetPoseUsedOnSubmit");
				return setPoseUsedOnSubmit;
			}
		}

		public delegate void NotifyQualityLevelChangeDelegate();
		private static NotifyQualityLevelChangeDelegate notifyQualityLevelChange = null;
		public static NotifyQualityLevelChangeDelegate NotifyQualityLevelChange
		{
			get
			{
				if (notifyQualityLevelChange == null)
					notifyQualityLevelChange = FunctionsHelper.GetFuncPtr<NotifyQualityLevelChangeDelegate>("NotifyQualityLevelChange");
				return notifyQualityLevelChange;
			}
		}
	} // class RenderFunctions

	public static class WXRDevice
	{
		static int m_CheckRoleFrameCount = 0;
		static WVR_DeviceType m_DefaultRole = WVR_DeviceType.WVR_DeviceType_Controller_Right;
		static bool m_IsLeftHanded = false;
		static bool IsLeftHanded {
			get {
				if (m_CheckRoleFrameCount != Time.frameCount)
				{
					m_CheckRoleFrameCount = Time.frameCount;
					WVR_DeviceType default_role = Interop.WVR_GetDefaultControllerRole();
					if (m_DefaultRole != default_role)
					{
						m_DefaultRole = default_role;
					}
					m_IsLeftHanded = (m_DefaultRole == WVR_DeviceType.WVR_DeviceType_Controller_Left ? true : false);
				}
				return m_IsLeftHanded;
			}
		}

		public static WVR_DeviceType GetRoleType(this XR_Device device, bool adaptiveHanded = false)
		{
			switch (device)
			{
				case XR_Device.Head:
					return WVR_DeviceType.WVR_DeviceType_HMD;
				case XR_Device.Dominant:
					if (adaptiveHanded)
						return IsLeftHanded ? WVR_DeviceType.WVR_DeviceType_Controller_Left : WVR_DeviceType.WVR_DeviceType_Controller_Right;
					else
						return WVR_DeviceType.WVR_DeviceType_Controller_Right;
				case XR_Device.NonDominant:
					if (adaptiveHanded)
						return IsLeftHanded ? WVR_DeviceType.WVR_DeviceType_Controller_Right : WVR_DeviceType.WVR_DeviceType_Controller_Left;
					else
						return WVR_DeviceType.WVR_DeviceType_Controller_Left;
				default:
					break;
			}

			return WVR_DeviceType.WVR_DeviceType_Invalid;
		}

		static InputDevice GetRoleDevice(XR_Device device, bool adaptiveHanded = false)
		{
			if (device == XR_Device.Dominant)
			{
				if (adaptiveHanded)
					return IsLeftHanded ? InputDevices.GetDeviceAtXRNode(XRNode.LeftHand) : InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
				else
					return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
			}
			if (device == XR_Device.NonDominant)
			{
				if (adaptiveHanded)
					return IsLeftHanded ? InputDevices.GetDeviceAtXRNode(XRNode.RightHand) : InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
				else
					return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
			}

			return InputDevices.GetDeviceAtXRNode(XRNode.Head);
		}

		enum ButtonReadType
		{
			None = 0,
			Binary,
			Axis1D,
			Axis2D
		}

		struct ButtonInfo
		{
			public ButtonInfo(InputFeatureUsage<bool> feature, ButtonReadType type)
			{
				binaryFeature = feature;
				this.type = type;
			}
			public ButtonInfo(InputFeatureUsage<float> feature, ButtonReadType type)
			{
				axis1DFeature = feature;
				this.type = type;
			}
			public ButtonInfo(InputFeatureUsage<Vector2> feature, ButtonReadType type)
			{
				axis2DFeature = feature;
				this.type = type;
			}

			public InputFeatureUsage<bool> binaryFeature;
			public InputFeatureUsage<float> axis1DFeature;
			public InputFeatureUsage<Vector2> axis2DFeature;
			public ButtonReadType type;
		}

		/**
		 *  Mapping to
		 *  public enum XR_BinaryButton
		 *  {
		 *  	MenuPress,
		 *  	GripPress,
		 *  	TouchpadPress,
		 *  	TouchpadTouch,
		 *  	TriggerPress,
		 *  	ThumbstickPress,
		 *  	ThumbstickTouch,
		 *  	A_X_Press,
		 *  	A_X_Touch,
		 *  	B_Y_Press,
		 *  	B_Y_Touch,
		 *  };
		 **/
		static InputFeatureUsage<bool>[] s_BinaryButtonFeature = new InputFeatureUsage<bool>[]
		{
			XR_Feature.MenuPress,
			XR_Feature.GripPress,
			XR_Feature.TouchpadPress,
			XR_Feature.TouchpadTouch,
			XR_Feature.TriggerPress,
			XR_Feature.ThumbstickPress,
			XR_Feature.ThumbstickTouch,
			XR_Feature.A_X_Press,
			XR_Feature.A_X_Touch,
			XR_Feature.B_Y_Press,
			XR_Feature.B_Y_Touch,
		};

		public static bool KeyDown(XR_Device device, XR_BinaryButton button, bool adaptiveHanded = false)
		{
			bool isDown = false;

			if ((int)button >= s_BinaryButtonFeature.Length || (int)button < 0)
			{
				throw new ArgumentException("[Wave.Essence.WXRDevice.KeyDown] The value of <button> is out or the supported range.");
			}

			InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
			if (input_device.isValid)
			{
				InputFeatureUsage<bool> feature = s_BinaryButtonFeature[(int)button];
				if (input_device.TryGetFeatureValue(feature, out bool value))
					isDown = value;
			}

			return isDown;
		}

		/**
		 *  Mapping to
		 *  public enum XR_Axis1DButton
		 *  {
		 *		TriggerAxis = 0,
		 *		GripAxis,
		 *	};
		 **/
		static InputFeatureUsage<float>[] s_Axis1DButtonFeature = new InputFeatureUsage<float>[]
		{
			XR_Feature.TriggerAxis,
			XR_Feature.GripAxis,
		};

		public static float KeyAxis1D(XR_Device device, XR_Axis1DButton button, bool adaptiveHanded = false)
		{
			float axis = 0;

			if ((int)button >= s_Axis1DButtonFeature.Length || (int)button < 0)
			{
				throw new ArgumentException("[Wave.Essence.WXRDevice.KeyAxis1D] The value of <button> is out or the supported range.");
			}

			InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
			if (input_device.isValid)
			{
				InputFeatureUsage<float> feature = s_Axis1DButtonFeature[(int)button];
				if (input_device.TryGetFeatureValue(feature, out float value))
					axis = value;
			}

			return axis;
		}

		/**
		 *  Mapping to
		 *  public enum XR_Axis2DButton
		 *  {
		 *		TouchpadAxis = 0,
		 *		ThumbstickAxis,
		 *	};
		 **/
		static InputFeatureUsage<Vector2>[] s_Axis2DButtonFeature = new InputFeatureUsage<Vector2>[]
		{
			XR_Feature.TouchpadAxis,
			XR_Feature.ThumbstickAxis,
		};

		public static Vector2 KeyAxis2D(XR_Device device, XR_Axis2DButton button, bool adaptiveHanded = false)
		{
			Vector2 axis = Vector2.zero;

			if ((int)button >= s_Axis2DButtonFeature.Length || (int)button < 0)
			{
				throw new ArgumentException("[Wave.Essence.WXRDevice.KeyAxis2D] The value of <button> is out or the supported range.");
			}

			InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
			if (input_device.isValid)
			{
				InputFeatureUsage<Vector2> feature = s_Axis2DButtonFeature[(int)button];
				if (input_device.TryGetFeatureValue(feature, out Vector2 value))
					axis = value;
			}

			return axis;
		}

		static readonly HapticCapabilities emptyHapticCapabilities = new HapticCapabilities();
		public static bool TryGetHapticCapabilities(XR_Device device, out HapticCapabilities hapticCaps, bool adaptiveHanded = false)
		{
			hapticCaps = emptyHapticCapabilities;

			InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
			if (input_device.isValid)
				return input_device.TryGetHapticCapabilities(out hapticCaps);

			return false;
		}

		static HapticCapabilities m_HapticCaps = new HapticCapabilities();
		public static bool SendHapticImpulse(XR_Device device, float amplitude, float duration, bool adaptiveHanded = false)
		{
			if (TryGetHapticCapabilities(device, out m_HapticCaps, adaptiveHanded))
			{
				if (m_HapticCaps.supportsImpulse)
				{
					InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
					if (input_device.isValid)
					{
						amplitude = Mathf.Clamp(amplitude, 0, 1);
						return input_device.SendHapticImpulse(0, amplitude, duration);
					}
				}
			}

			return false;
		}

		public static bool IsTracked(XR_Device device, bool adaptiveHanded = false)
		{
#if UNITY_EDITOR
			if (Application.isEditor)
				return true;
			else
#endif
			{
				bool tracked = false;

				InputDevice input_device = GetRoleDevice(device, adaptiveHanded);
				if (input_device.isValid)
				{
					if (input_device.TryGetFeatureValue(XR_Feature.ValidPose, out bool value))
						tracked = value;
				}

				return tracked;
			}
		}
	} // class WXRDevice

	public static class ApplicationScene
	{
		static int m_CurrFrameCount = 0;
		private static bool m_IsFocused = false;
		/// <summary> Means the system focus is captured by current scene or not. </summary>
		public static bool IsFocused {
			get {
				if (m_CurrFrameCount != Time.frameCount)
				{
					m_CurrFrameCount = Time.frameCount;
					m_IsFocused = (Interop.WVR_IsInputFocusCapturedBySystem() ? false : true);
				}
				return m_IsFocused;
			}
		}
	}
}
