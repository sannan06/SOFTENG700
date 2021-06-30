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
using UnityEngine.UI;
using Wave.Native;
using UnityEngine.XR;
#if UNITY_EDITOR
using Wave.Essence.Editor;
#endif

namespace Wave.Essence.Samples.ButtonTest
{
	public class ButtonTest : MonoBehaviour
	{
		private const string LOG_TAG = "ButtonTest";
		private void DEBUG(string msg)
		{
			if (Log.EnableDebugLog)
				Log.d(LOG_TAG, this.DeviceType + " " + msg, true);
		}

		public XR_Device DeviceType = XR_Device.Dominant;

		public GameObject Button_Touch = null;
		private Text touch_text = null;
		public GameObject Button_Axis = null;
		private Text axis_text = null;
		private Vector2 v2axis = Vector2.zero;
		public GameObject Button_Press = null;
		private Text press_text = null;
		public GameObject DPad_Button_Touch = null;
		private Text dpad_touch_text = null;
		public GameObject DPad_Button_Axis = null;
		private Text dpad_axis_text = null;
		public GameObject DPad_Button_Press = null;
		private Text dpad_press_text = null;

		void Awake()
		{
			if (this.Button_Touch != null)
			{
				touch_text = this.Button_Touch.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.Button_Touch.name);
			}
			if (this.Button_Axis != null)
			{
				axis_text = this.Button_Axis.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.Button_Axis.name);
			}
			if (this.Button_Press != null)
			{
				press_text = this.Button_Press.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.Button_Press.name);
			}
			if (this.DPad_Button_Touch != null)
			{
				dpad_touch_text = this.DPad_Button_Touch.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.DPad_Button_Touch);
			}
			if (this.DPad_Button_Axis != null)
			{
				dpad_axis_text = this.DPad_Button_Axis.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.DPad_Button_Axis);
			}
			if (this.DPad_Button_Press != null)
			{
				dpad_press_text = this.DPad_Button_Press.GetComponent<Text>();
				DEBUG("Start() Get Text of " + this.DPad_Button_Press);
			}
		}

		void Update()
		{
			UpdateTouchText();
			UpdateAxisText();
			UpdatePressText();
			updateDPadTouchText();
			//updateDPadAxisText();
			updateDPadPressText();
		}


		private bool binaryValue = false;
		private bool preAXTouch = false, preBYTouch = false, preTouchpadTouch = false, preThumbstickTouch = false;
		private void UpdateTouchText()
		{
			if (touch_text == null)
				return;

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_A, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Press))
				{
					touch_text.text = "A(X)";
					DEBUG("UpdateTouchText() A(X) is touched.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_A, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Release))
				{
					touch_text.text = "";
					DEBUG("UpdateTouchText() A(X) is untouched.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.A_X_Touch);
				if (preAXTouch != binaryValue)
				{
					preAXTouch = binaryValue;
					if (binaryValue)
					{
						touch_text.text = "A(X)";
						DEBUG("UpdateTouchText() A(X) is touched.");
					}
					else
					{
						touch_text.text = "";
						DEBUG("UpdateTouchText() A(X) is untouched.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_B, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Press))
				{
					touch_text.text = "B(Y)";
					DEBUG("UpdateTouchText() B(Y) is touched.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_B, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Release))
				{
					touch_text.text = "";
					DEBUG("UpdateTouchText() B(Y) is untouched.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.B_Y_Touch);
				if (preBYTouch != binaryValue)
				{
					preBYTouch = binaryValue;
					if (binaryValue)
					{
						touch_text.text = "B(Y)";
						DEBUG("UpdateTouchText() B(Y) is touched.");
					}
					else
					{
						touch_text.text = "";
						DEBUG("UpdateTouchText() B(Y) is untouched.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Press))
				{
					touch_text.text = "Touchpad";
					DEBUG("UpdateTouchText() Touchpad is touched.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Release))
				{
					touch_text.text = "";
					DEBUG("UpdateTouchText() Touchpad is untouched.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.TouchpadTouch);
				if (preTouchpadTouch != binaryValue)
				{
					preTouchpadTouch = binaryValue;
					if (binaryValue)
					{
						touch_text.text = "Touchpad";
						DEBUG("UpdateTouchText() Touchpad is touched.");
					}
					else
					{
						touch_text.text = "";
						DEBUG("UpdateTouchText() Touchpad is untouched.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Press))
				{
					touch_text.text = "Thumbstick";
					DEBUG("UpdateTouchText() Thumbstick is touched.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Release))
				{
					touch_text.text = "";
					DEBUG("UpdateTouchText() Thumbstick is untouched.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.ThumbstickTouch);
				if (preThumbstickTouch != binaryValue)
				{
					preThumbstickTouch = binaryValue;
					if (binaryValue)
					{
						touch_text.text = "Thumbstick";
						DEBUG("UpdateTouchText() Thumbstick is touched.");
					}
					else
					{
						touch_text.text = "";
						DEBUG("UpdateTouchText() Thumbstick is untouched.");
					}
				}
			}
		}

		private Vector2 axis2DValue = Vector2.zero;
		private float axisValue = 0;
		private void UpdateAxisText()
		{
			if (axis_text == null)
				return;

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Hold))
				{
					axis2DValue = DummyButton.GetAxis(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad);
					axis_text.text = axis2DValue.x.ToString() + ", " + axis2DValue.y.ToString();
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Hold))
				{
					axis2DValue = DummyButton.GetAxis(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick);
					axis_text.text = axis2DValue.x.ToString() + ", " + axis2DValue.y.ToString();
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Trigger, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Hold))
				{
					axis2DValue = DummyButton.GetAxis(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Trigger);
					axis_text.text = axis2DValue.x.ToString();
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Grip, WVR_InputType.WVR_InputType_Touch, DummyButton.ButtonState.Hold))
				{
					axis2DValue = DummyButton.GetAxis(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Grip);
					axis_text.text = axis2DValue.x.ToString();
				}
			}
			else
#endif
			{
				axis2DValue = WXRDevice.KeyAxis2D(this.DeviceType, XR_Axis2DButton.TouchpadAxis);
				if (!axis2DValue.Equals(Vector2.zero))
					axis_text.text = axis2DValue.x.ToString() + ", " + axis2DValue.y.ToString();

				axis2DValue = WXRDevice.KeyAxis2D(this.DeviceType, XR_Axis2DButton.ThumbstickAxis);
				if (!axis2DValue.Equals(Vector2.zero))
					axis_text.text = axis2DValue.x.ToString() + ", " + axis2DValue.y.ToString();

				axisValue = WXRDevice.KeyAxis1D(this.DeviceType, XR_Axis1DButton.TriggerAxis);
				if (axisValue != 0)
					axis_text.text = axisValue.ToString();

				axisValue = WXRDevice.KeyAxis1D(this.DeviceType, XR_Axis1DButton.GripAxis);
				if (axisValue != 0)
					axis_text.text = axisValue.ToString();
			}
		}

		private bool preMenuPress = false, preTouchpadPress = false, preTriggerPress = false, preGripPress = false, preThumbstickPress = false;
		private bool preAXPress = false, preBYPress = false;
		private void UpdatePressText()
		{
			if (press_text == null)
				return;

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Menu, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "Menu";
					DEBUG("UpdatePressText() Menu is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Menu, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() Menu is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.MenuPress);
				if (preMenuPress != binaryValue)
				{
					preMenuPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "Menu";
						DEBUG("UpdatePressText() Menu is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() Menu is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "Touchpad";
					DEBUG("UpdatePressText() Touchpad is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Touchpad, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() Touchpad is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.TouchpadPress);
				if (preTouchpadPress != binaryValue)
				{
					preTouchpadPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "Touchpad";
						DEBUG("UpdatePressText() Touchpad is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() Touchpad is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Trigger, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "Trigger";
					DEBUG("UpdatePressText() Trigger is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Trigger, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() Trigger is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.TriggerPress);
				if (preTriggerPress != binaryValue)
				{
					preTriggerPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "Trigger";
						TriggerVibration(this.DeviceType, 0.5f);
						DEBUG("UpdatePressText() Trigger is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() Trigger is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Grip, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "Grip";
					DEBUG("UpdatePressText() Grip is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Grip, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() Grip is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.GripPress);
				if (preGripPress != binaryValue)
				{
					preGripPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "Grip";
						DEBUG("UpdatePressText() Grip is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() Grip is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_A, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "A(X)";
					DEBUG("UpdatePressText() A(X) is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_A, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() A(X) is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.A_X_Press);
				if (preAXPress != binaryValue)
				{
					preAXPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "A(X)";
						DEBUG("UpdatePressText() A(X) is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() A(X) is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_B, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "B(Y)";
					DEBUG("UpdatePressText() B(Y) is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_B, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() B(Y) is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.B_Y_Press);
				if (preBYPress != binaryValue)
				{
					preBYPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "B(Y)";
						DEBUG("UpdatePressText() B(Y) is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() B(Y) is unpressed.");
					}
				}
			}

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Press))
				{
					press_text.text = "Thumbstick";
					DEBUG("UpdatePressText() Thumbstick is pressed.");
				}
				if (DummyButton.GetStatus(this.DeviceType, WVR_InputId.WVR_InputId_Alias1_Thumbstick, WVR_InputType.WVR_InputType_Button, DummyButton.ButtonState.Release))
				{
					press_text.text = "";
					DEBUG("UpdatePressText() Thumbstick is unpressed.");
				}
			}
			else
#endif
			{
				binaryValue = WXRDevice.KeyDown(this.DeviceType, XR_BinaryButton.ThumbstickPress);
				if (preThumbstickPress != binaryValue)
				{
					preThumbstickPress = binaryValue;
					if (binaryValue)
					{
						press_text.text = "Thumbstick";
						DEBUG("UpdatePressText() Thumbstick is pressed.");
					}
					else
					{
						press_text.text = "";
						DEBUG("UpdatePressText() Thumbstick is unpressed.");
					}
				}
			}
		}


		private void updateDPadTouchText()
		{
			if (dpad_touch_text == null)
				return;
		}

		private void updateDPadPressText()
		{
			if (dpad_press_text == null)
				return;
		}

		/// <summary>
		/// Trigger vibration on a device.
		/// </summary>
		/// <param name="device">The vibration device.</param>
		/// <param name="amplitude">[0, 1] The intensity of vibration.</param>
		/// <param name="duration">The vibration duration in seconds. Default 1 second.</param>
		public void TriggerVibration(XR_Device device, float amplitude, float duration = 1)
		{
			DEBUG("TriggerVibration() " + device + ", amplitude: " + amplitude + ", duration: " + duration);
			WXRDevice.SendHapticImpulse(device, amplitude, duration);
		}
	}
}
