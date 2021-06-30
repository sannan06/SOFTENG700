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
using UnityEngine.XR;

namespace Wave.Essence
{
	public static class XR_Feature
	{
		// A binary value representing the Menu press state.
		public static InputFeatureUsage<bool> MenuPress = CommonUsages.menuButton;
		// A binary value representing the Grip press state.
		public static InputFeatureUsage<bool> GripPress = CommonUsages.gripButton;
		// A float value representing the Grip axis.
		public static InputFeatureUsage<float> GripAxis = CommonUsages.grip;
		// A binary value representing the Button A(X) press state.
		public static InputFeatureUsage<bool> A_X_Press = CommonUsages.primaryButton;
		// A binary value representing the Button A(X) touch state.
		public static InputFeatureUsage<bool> A_X_Touch = CommonUsages.primaryTouch;
		// A binary value representing the Button B(Y) press state.
		public static InputFeatureUsage<bool> B_Y_Press = CommonUsages.secondaryButton;
		// A binary value representing the Button B(Y) touch state.
		public static InputFeatureUsage<bool> B_Y_Touch = CommonUsages.secondaryTouch;

		// A binary value representing the Touchpad press state.
		public static InputFeatureUsage<bool> TouchpadPress = CommonUsages.primary2DAxisClick;
		// A binary value representing the Touchpad touch state.
		public static InputFeatureUsage<bool> TouchpadTouch = CommonUsages.primary2DAxisTouch;
		// A Vector2 value representing the Touchpad axis.
		public static InputFeatureUsage<Vector2> TouchpadAxis = CommonUsages.primary2DAxis;

		// A binary value representing the Trigger press state.
		public static InputFeatureUsage<bool> TriggerPress = CommonUsages.triggerButton;
		// A float value representing the Trigger axis.
		public static InputFeatureUsage<float> TriggerAxis = CommonUsages.trigger;

		// A binary value representing the Thumbstick press state.
		public static InputFeatureUsage<bool> ThumbstickPress = CommonUsages.secondary2DAxisClick;
		// A binary value representing the Thumbstick touch state.
		public static InputFeatureUsage<bool> ThumbstickTouch = CommonUsages.secondary2DAxisTouch;
		// A Vector2 value representing the Thumbstick axis.
		public static InputFeatureUsage<Vector2> ThumbstickAxis = CommonUsages.secondary2DAxis;

		// A binary feature that represents if the device is currently tracking properly.  True means fully tracked, false means either partially or not tracked.
		public static InputFeatureUsage<bool> ValidPose = CommonUsages.isTracked;
	}

	public enum XR_BinaryButton
	{
		MenuPress = 0,
		GripPress,
		TouchpadPress,
		TouchpadTouch,
		TriggerPress,
		ThumbstickPress,
		ThumbstickTouch,
		A_X_Press,
		A_X_Touch,
		B_Y_Press,
		B_Y_Touch,
	};

	public enum XR_Axis1DButton
	{
		TriggerAxis = 0,
		GripAxis,
	};

	public enum XR_Axis2DButton
	{
		TouchpadAxis = 0,
		ThumbstickAxis,
	};

	public enum XR_Device
	{
		Head = 1,
		Dominant = 2,
		NonDominant = 3
	};

	public enum XR_Hand
	{
		Dominant = XR_Device.Dominant,
		NonDominant = XR_Device.NonDominant
	}
}
