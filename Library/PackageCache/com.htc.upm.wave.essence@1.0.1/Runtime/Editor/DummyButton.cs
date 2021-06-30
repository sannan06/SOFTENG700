// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Wave.Native;

#if UNITY_EDITOR
namespace Wave.Essence.Editor
{
	public class DummyButton : MonoBehaviour
	{
		private static string LOG_TAG = "DummyButton";
		private void INFO(string msg) { Log.i(LOG_TAG, msg, true); }
		private void DEBUG(string msg)
		{
			if (Log.EnableDebugLog)
				Log.d(LOG_TAG, msg, true);
		}

		/// <summary>
		/// Refered from InputHelpers.Button of the package com.unity.xr.interaction.toolkit
		/// </summary>
		public enum XButton
		{
			None = 0,
			MenuButton,
			Trigger,
			Grip,
			TriggerPressed,
			GripPressed,
			PrimaryButton,
			PrimaryTouch,
			SecondaryButton,
			SecondaryTouch,
			Primary2DAxisTouch,
			Primary2DAxisClick,
			Secondary2DAxisTouch,
			Secondary2DAxisClick,
			PrimaryAxis2DUp,
			PrimaryAxis2DDown,
			PrimaryAxis2DLeft,
			PrimaryAxis2DRight,
			SecondaryAxis2DUp,
			SecondaryAxis2DDown,
			SecondaryAxis2DLeft,
			SecondaryAxis2DRight
		};

		/// <summary>
		/// Wave defined buttons.
		/// </summary>
		public enum WButton
		{
			Unavailable = WVR_InputId.WVR_InputId_Alias1_System,
			Menu = WVR_InputId.WVR_InputId_Alias1_Menu,
			Grip = WVR_InputId.WVR_InputId_Alias1_Grip,
			DPadUp = WVR_InputId.WVR_InputId_Alias1_DPad_Up,
			DPadRight = WVR_InputId.WVR_InputId_Alias1_DPad_Right,
			DPadDown = WVR_InputId.WVR_InputId_Alias1_DPad_Down,
			DPadLeft = WVR_InputId.WVR_InputId_Alias1_DPad_Left,
			VolumeUp = WVR_InputId.WVR_InputId_Alias1_Volume_Up,
			VolumeDown = WVR_InputId.WVR_InputId_Alias1_Volume_Down,
			Bumper = WVR_InputId.WVR_InputId_Alias1_Bumper,
			A_X = WVR_InputId.WVR_InputId_Alias1_A,
			B_Y = WVR_InputId.WVR_InputId_Alias1_B,
			Back = WVR_InputId.WVR_InputId_Alias1_Back,
			Enter = WVR_InputId.WVR_InputId_Alias1_Enter,
			Touchpad = WVR_InputId.WVR_InputId_Alias1_Touchpad,
			Trigger = WVR_InputId.WVR_InputId_Alias1_Trigger,
			Thumbstick = WVR_InputId.WVR_InputId_Alias1_Thumbstick
		}

		public WButton GetEButtonsType(WVR_InputId button)
		{
			WButton btn_type = WButton.Unavailable;
			switch (button)
			{
				case WVR_InputId.WVR_InputId_Alias1_Menu:
					btn_type = WButton.Menu;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Grip:
					btn_type = WButton.Grip;
					break;
				case WVR_InputId.WVR_InputId_Alias1_DPad_Up:
					btn_type = WButton.DPadUp;
					break;
				case WVR_InputId.WVR_InputId_Alias1_DPad_Right:
					btn_type = WButton.DPadRight;
					break;
				case WVR_InputId.WVR_InputId_Alias1_DPad_Down:
					btn_type = WButton.DPadDown;
					break;
				case WVR_InputId.WVR_InputId_Alias1_DPad_Left:
					btn_type = WButton.DPadLeft;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Volume_Up:
					btn_type = WButton.VolumeUp;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Volume_Down:
					btn_type = WButton.VolumeDown;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Bumper:
					btn_type = WButton.Bumper;
					break;
				case WVR_InputId.WVR_InputId_Alias1_A:
					btn_type = WButton.A_X;
					break;
				case WVR_InputId.WVR_InputId_Alias1_B:
					btn_type = WButton.B_Y;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Back:
					btn_type = WButton.Back;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Enter:
					btn_type = WButton.Enter;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Touchpad:
					btn_type = WButton.Touchpad;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Trigger:
					btn_type = WButton.Trigger;
					break;
				case WVR_InputId.WVR_InputId_Alias1_Thumbstick:
					btn_type = WButton.Thumbstick;
					break;
				default:
					btn_type = WButton.Unavailable;
					break;
			}

			return btn_type;
		}

		[System.Serializable]
		public class HmdButtonOption
		{
			public bool Menu = false;
			public bool VolumeUp = false;
			public bool VolumeDown = false;
			public bool Enter = true;
			public bool Back = false;

			public ulong optionValue { get; private set; }
			public void UpdateOptionValue()
			{
				optionValue = 0;
				if (Menu)
					optionValue |= 1UL << (int)WButton.Menu;
				if (VolumeUp)
					optionValue |= 1UL << (int)WButton.VolumeUp;
				if (VolumeDown)
					optionValue |= 1UL << (int)WButton.VolumeDown;
				if (Enter)
					optionValue |= 1UL << (int)WButton.Enter;
				if (Back)
					optionValue |= 1UL << (int)WButton.Back;
			}
		}

		[System.Serializable]
		public class ControllerButtonOption
		{
			public bool Menu = false;
			public bool Grip = false;
			public bool DPadUp = false;
			public bool DPadRight = false;
			public bool DPadDown = false;
			public bool DPadLeft = false;
			public bool VolumeUp = false;
			public bool VolumeDown = false;
			// public bool Bumper = false; Do NOT use Bumper due to the bumper button will be mapping to the Trigger event.
			public bool A_X = false;
			public bool B_Y = false;
			public bool Touchpad = true;
			public bool Trigger = true;
			public bool Thumbstick = false;

			public ulong optionValue { get; private set; }
			public void UpdateOptionValue()
			{
				optionValue = 0;
				if (Menu)
					optionValue |= 1UL << (int)WButton.Menu;
				if (Grip)
					optionValue |= 1UL << (int)WButton.Grip;
				if (DPadUp)
					optionValue |= 1UL << (int)WButton.DPadUp;
				if (DPadRight)
					optionValue |= 1UL << (int)WButton.DPadRight;
				if (DPadDown)
					optionValue |= 1UL << (int)WButton.DPadDown;
				if (DPadLeft)
					optionValue |= 1UL << (int)WButton.DPadLeft;
				if (VolumeUp)
					optionValue |= 1UL << (int)WButton.VolumeUp;
				if (VolumeDown)
					optionValue |= 1UL << (int)WButton.VolumeDown;
				if (A_X)
					optionValue |= 1UL << (int)WButton.A_X;
				if (B_Y)
					optionValue |= 1UL << (int)WButton.B_Y;
				if (Touchpad)
					optionValue |= 1UL << (int)WButton.Touchpad;
				if (Trigger)
					optionValue |= 1UL << (int)WButton.Trigger;
				if (Thumbstick)
					optionValue |= 1UL << (int)WButton.Thumbstick;
			}
		}

		public static WVR_DeviceType WvrDevice(XR_Device device)
		{
			if (device == XR_Device.Head)
				return WVR_DeviceType.WVR_DeviceType_HMD;
			if (device == XR_Device.Dominant)
				return WVR_DeviceType.WVR_DeviceType_Controller_Right;
			if (device == XR_Device.NonDominant)
				return WVR_DeviceType.WVR_DeviceType_Controller_Left;
			return WVR_DeviceType.WVR_DeviceType_Invalid;
		}

		[SerializeField]
		private HmdButtonOption m_HmdOptions = new HmdButtonOption();
		public HmdButtonOption HmdOptions { get { return m_HmdOptions; } set { m_HmdOptions = value; } }

		[SerializeField]
		private ControllerButtonOption m_DominantOptions = new ControllerButtonOption();
		public ControllerButtonOption DominantOptions { get { return m_DominantOptions; } set { m_DominantOptions = value; } }

		[SerializeField]
		private ControllerButtonOption m_NonDominantOptions = new ControllerButtonOption();
		public ControllerButtonOption NonDominantOptions { get { return m_NonDominantOptions; } set { m_NonDominantOptions = value; } }

		private ulong m_HmdOptionValue = 0, m_DominantOptionValue = 0, m_NonDominantOptionValue = 0;
		private void UpdateOptionValues()
		{
			m_HmdOptions.UpdateOptionValue();
			m_DominantOptions.UpdateOptionValue();
			m_NonDominantOptions.UpdateOptionValue();
		}

		private WVR_InputAttribute_t[] inputAttributes_hmd;
		private List<WVR_InputId> usableButtons_hmd = new List<WVR_InputId>();

		private WVR_InputAttribute_t[] inputAttributes_Dominant;
		private List<WVR_InputId> usableButtons_dominant = new List<WVR_InputId>();

		private WVR_InputAttribute_t[] inputAttributes_NonDominant;
		private List<WVR_InputId> usableButtons_nonDominant = new List<WVR_InputId>();

		private const uint inputTableSize = (uint)WVR_InputId.WVR_InputId_Max;
		private WVR_InputMappingPair_t[] inputTableHMD = new WVR_InputMappingPair_t[inputTableSize];
		private uint inputTableHMDSize = 0;
		private WVR_InputMappingPair_t[] inputTableDominant = new WVR_InputMappingPair_t[inputTableSize];
		private uint inputTableDominantSize = 0;
		private WVR_InputMappingPair_t[] inputTableNonDominant = new WVR_InputMappingPair_t[inputTableSize];
		private uint inputTableNonDominantSize = 0;

		private static DummyButton instance = null;
		public static DummyButton Instance
		{
			get
			{
				return instance;
			}
		}

		#region MonoBehaviour overrides
		void Awake()
		{
			if (WaveEditor.Instance != null)
				INFO("Awake() WaveEditor is initialized.");

			ResetButtonState();

			if (instance == null)
				instance = this;

			UpdateOptionValues();
			m_HmdOptionValue = m_HmdOptions.optionValue;
			m_DominantOptionValue = m_DominantOptions.optionValue;
			m_NonDominantOptionValue = m_NonDominantOptions.optionValue;
		}

		void Start()
		{
			INFO("Start()");
			ResetAllInputRequest();
		}

		void Update()
		{
			SaveCurrentButtonStates();
			UpdateOptionValues();
			if (m_HmdOptionValue != m_HmdOptions.optionValue)
			{
				m_HmdOptionValue = m_HmdOptions.optionValue;
				DEBUG("Update() HMD options changed to " + m_HmdOptionValue);
				ResetInputRequest(XR_Device.Head);
			}
			if (m_DominantOptionValue != m_DominantOptions.optionValue)
			{
				m_DominantOptionValue = m_DominantOptions.optionValue;
				DEBUG("Update() Dominant options changed to " + m_DominantOptionValue);
				ResetInputRequest(XR_Device.Dominant);
			}
			if (m_NonDominantOptionValue != m_NonDominantOptions.optionValue)
			{
				m_NonDominantOptionValue = m_NonDominantOptions.optionValue;
				DEBUG("Update() Dominant options changed to " + m_NonDominantOptionValue);
				ResetInputRequest(XR_Device.NonDominant);
			}
		}
		#endregion

		public bool GetInputMappingPair(XR_Device device, ref WButton destination)
		{
			bool ret = false;
			WVR_InputId _wbtn = (WVR_InputId)destination;

			ret = GetInputMappingPair(device, ref _wbtn);
			if (ret)
				destination = GetEButtonsType(_wbtn);

			return ret;
		}

		public bool GetInputMappingPair(XR_Device device, ref WVR_InputId destination)
		{
			// Default true in editor mode, destination will be equivallent to source.
			bool result = false;
			int index = 0;

			switch (device)
			{
				case XR_Device.Head:
					if (inputTableHMDSize > 0)
					{
						for (index = 0; index < (int)inputTableHMDSize; index++)
						{
							if (inputTableHMD[index].destination.id == destination)
							{
								destination = inputTableHMD[index].source.id;
								result = true;
							}
						}
					}
					break;
				case XR_Device.Dominant:
					if (inputTableDominantSize > 0)
					{
						for (index = 0; index < (int)inputTableDominantSize; index++)
						{
							if (inputTableDominant[index].destination.id == destination)
							{
								destination = inputTableDominant[index].source.id;
								result = true;
							}
						}
					}
					break;
				case XR_Device.NonDominant:
					if (inputTableNonDominantSize > 0)
					{
						for (index = 0; index < (int)inputTableNonDominantSize; index++)
						{
							if (inputTableNonDominant[index].destination.id == destination)
							{
								destination = inputTableNonDominant[index].source.id;
								result = true;
							}
						}
					}
					break;
				default:
					break;
			}

			return result;
		}

		private void setupButtonAttributes(XR_Device device, List<WButton> buttons, WVR_InputAttribute_t[] inputAttributes, int count)
		{
			WVR_DeviceType dev_type = WvrDevice(device);

			for (int _i = 0; _i < count; _i++)
			{
				switch (buttons[_i])
				{
					case WButton.Menu:
					case WButton.Grip:
					case WButton.DPadLeft:
					case WButton.DPadUp:
					case WButton.DPadRight:
					case WButton.DPadDown:
					case WButton.VolumeUp:
					case WButton.VolumeDown:
					case WButton.A_X:
					case WButton.B_Y:
					case WButton.Back:
					case WButton.Enter:
						inputAttributes[_i].id = (WVR_InputId)buttons[_i];
						inputAttributes[_i].capability = (uint)WVR_InputType.WVR_InputType_Button;
						inputAttributes[_i].axis_type = WVR_AnalogType.WVR_AnalogType_None;
						break;
					case WButton.Touchpad:
					case WButton.Thumbstick:
						inputAttributes[_i].id = (WVR_InputId)buttons[_i];
						inputAttributes[_i].capability = (uint)(WVR_InputType.WVR_InputType_Button | WVR_InputType.WVR_InputType_Touch | WVR_InputType.WVR_InputType_Analog);
						inputAttributes[_i].axis_type = WVR_AnalogType.WVR_AnalogType_2D;
						break;
					case WButton.Trigger:
						inputAttributes[_i].id = (WVR_InputId)buttons[_i];
						inputAttributes[_i].capability = (uint)(WVR_InputType.WVR_InputType_Button | WVR_InputType.WVR_InputType_Touch | WVR_InputType.WVR_InputType_Analog);
						inputAttributes[_i].axis_type = WVR_AnalogType.WVR_AnalogType_1D;
						break;
					default:
						break;
				}

				DEBUG("setupButtonAttributes() " + device + " (" + dev_type + ") " + buttons[_i]
					+ ", capability: " + inputAttributes[_i].capability
					+ ", analog type: " + inputAttributes[_i].axis_type);
			}
		}

		private void createHmdRequestAttributes()
		{
			INFO("createHmdRequestAttributes()");

			List<WButton> list = new List<WButton>();
			if (m_HmdOptions.Back)
				list.Add(WButton.Back);
			if (m_HmdOptions.Enter)
				list.Add(WButton.Enter);
			if (m_HmdOptions.Menu)
				list.Add(WButton.Menu);
			if (m_HmdOptions.VolumeDown)
				list.Add(WButton.VolumeDown);
			if (m_HmdOptions.VolumeUp)
				list.Add(WButton.VolumeUp);

			if (!list.Contains(WButton.Enter))
				list.Add(WButton.Enter);

			inputAttributes_hmd = new WVR_InputAttribute_t[list.Count];
			setupButtonAttributes(XR_Device.Head, list, inputAttributes_hmd, list.Count);
		}

		private void createDominantRequestAttributes()
		{
			INFO("createDominantRequestAttributes()");

			List<WButton> list = new List<WButton>();
			if (m_DominantOptions.Menu)
				list.Add(WButton.Menu);
			if (m_DominantOptions.Grip)
				list.Add(WButton.Grip);
			if (m_DominantOptions.DPadLeft)
				list.Add(WButton.DPadLeft);
			if (m_DominantOptions.DPadUp)
				list.Add(WButton.DPadUp);
			if (m_DominantOptions.DPadRight)
				list.Add(WButton.DPadRight);
			if (m_DominantOptions.DPadDown)
				list.Add(WButton.DPadDown);
			if (m_DominantOptions.VolumeUp)
				list.Add(WButton.VolumeUp);
			if (m_DominantOptions.VolumeDown)
				list.Add(WButton.VolumeDown);
			if (m_DominantOptions.A_X)
				list.Add(WButton.A_X);
			if (m_DominantOptions.B_Y)
				list.Add(WButton.B_Y);
			if (m_DominantOptions.Touchpad)
				list.Add(WButton.Touchpad);
			if (m_DominantOptions.Trigger)
				list.Add(WButton.Trigger);
			if (m_DominantOptions.Thumbstick)
				list.Add(WButton.Thumbstick);

			inputAttributes_Dominant = new WVR_InputAttribute_t[list.Count];
			setupButtonAttributes(XR_Device.Dominant, list, inputAttributes_Dominant, list.Count);
		}

		private void createNonDominantRequestAttributes()
		{
			INFO("createNonDominantRequestAttributes()");

			List<WButton> list = new List<WButton>();
			if (m_DominantOptions.Menu)
				list.Add(WButton.Menu);
			if (m_DominantOptions.Grip)
				list.Add(WButton.Grip);
			if (m_DominantOptions.DPadLeft)
				list.Add(WButton.DPadLeft);
			if (m_DominantOptions.DPadUp)
				list.Add(WButton.DPadUp);
			if (m_DominantOptions.DPadRight)
				list.Add(WButton.DPadRight);
			if (m_DominantOptions.DPadDown)
				list.Add(WButton.DPadDown);
			if (m_DominantOptions.VolumeUp)
				list.Add(WButton.VolumeUp);
			if (m_DominantOptions.VolumeDown)
				list.Add(WButton.VolumeDown);
			if (m_DominantOptions.A_X)
				list.Add(WButton.A_X);
			if (m_DominantOptions.B_Y)
				list.Add(WButton.B_Y);
			if (m_DominantOptions.Touchpad)
				list.Add(WButton.Touchpad);
			if (m_DominantOptions.Trigger)
				list.Add(WButton.Trigger);
			if (m_DominantOptions.Thumbstick)
				list.Add(WButton.Thumbstick);

			inputAttributes_NonDominant = new WVR_InputAttribute_t[list.Count];
			setupButtonAttributes(XR_Device.NonDominant, list, inputAttributes_NonDominant, list.Count);
		}

		public bool IsButtonAvailable(XR_Device device, WButton button)
		{
			return IsButtonAvailable(device, (WVR_InputId)button);
		}

		public bool IsButtonAvailable(XR_Device device, WVR_InputId button)
		{
			if (device == XR_Device.Head)
				return this.usableButtons_hmd.Contains(button);
			if (device == XR_Device.Dominant)
				return this.usableButtons_dominant.Contains(button);
			if (device == XR_Device.NonDominant)
				return this.usableButtons_nonDominant.Contains(button);

			return false;
		}

		private void SetHmdInputRequest()
		{
			this.usableButtons_hmd.Clear();

			WVR_DeviceType dev_type = WvrDevice(XR_Device.Head);
			bool _ret = Interop.WVR_SetInputRequest(dev_type, this.inputAttributes_hmd, (uint)this.inputAttributes_hmd.Length);
			if (_ret)
			{
				inputTableHMDSize = Interop.WVR_GetInputMappingTable(dev_type, inputTableHMD, DummyButton.inputTableSize);
				if (inputTableHMDSize > 0)
				{
					for (int _i = 0; _i < (int)inputTableHMDSize; _i++)
					{
						if (inputTableHMD[_i].source.capability != 0)
						{
							this.usableButtons_hmd.Add(inputTableHMD[_i].destination.id);
							DEBUG("SetHmdInputRequest() " + dev_type
								+ " button: " + inputTableHMD[_i].source.id + "(capability: " + inputTableHMD[_i].source.capability + ")"
								+ " is mapping to HMD input ID: " + inputTableHMD[_i].destination.id);
						}
						else
						{
							DEBUG("SetHmdInputRequest() " + dev_type
								+ " source button " + inputTableHMD[_i].source.id + " has invalid capability.");
						}
					}
				}
			}
		}

		private void SetDominantInputRequest()
		{
			this.usableButtons_dominant.Clear();

			WVR_DeviceType dev_type = WvrDevice(XR_Device.Dominant);
			bool _ret = Interop.WVR_SetInputRequest(dev_type, this.inputAttributes_Dominant, (uint)this.inputAttributes_Dominant.Length);
			if (_ret)
			{
				inputTableDominantSize = Interop.WVR_GetInputMappingTable(dev_type, inputTableDominant, DummyButton.inputTableSize);
				if (inputTableDominantSize > 0)
				{
					for (int _i = 0; _i < (int)inputTableDominantSize; _i++)
					{
						if (inputTableDominant[_i].source.capability != 0)
						{
							this.usableButtons_dominant.Add(inputTableDominant[_i].destination.id);
							DEBUG("SetDominantInputRequest() " + dev_type
								+ " button: " + inputTableDominant[_i].source.id + "(capability: " + inputTableDominant[_i].source.capability + ")"
								+ " is mapping to Dominant input ID: " + inputTableDominant[_i].destination.id);
						}
						else
						{
							DEBUG("SetDominantInputRequest() " + dev_type
								+ " source button " + inputTableDominant[_i].source.id + " has invalid capability.");
						}
					}
				}
			}
		}

		private void SetNonDominantInputRequest()
		{
			this.usableButtons_nonDominant.Clear();

			WVR_DeviceType dev_type = WvrDevice(XR_Device.NonDominant);
			bool _ret = Interop.WVR_SetInputRequest(dev_type, this.inputAttributes_NonDominant, (uint)this.inputAttributes_NonDominant.Length);
			if (_ret)
			{
				inputTableNonDominantSize = Interop.WVR_GetInputMappingTable(dev_type, inputTableNonDominant, DummyButton.inputTableSize);
				if (inputTableNonDominantSize > 0)
				{
					for (int _i = 0; _i < (int)inputTableNonDominantSize; _i++)
					{
						if (inputTableNonDominant[_i].source.capability != 0)
						{
							this.usableButtons_nonDominant.Add(inputTableNonDominant[_i].destination.id);
							DEBUG("SetNonDominantInputRequest() " + dev_type
								+ " button: " + inputTableNonDominant[_i].source.id + "(capability: " + inputTableNonDominant[_i].source.capability + ")"
								+ " is mapping to NonDominant input ID: " + inputTableNonDominant[_i].destination.id);
						}
						else
						{
							DEBUG("SetNonDominantInputRequest() " + dev_type
								+ " source button " + inputTableNonDominant[_i].source.id + " has invalid capability.");
						}
					}
				}
			}
		}

		private void ResetInputRequest(XR_Device device)
		{
			DEBUG("ResetInputRequest() " + device);
			switch (device)
			{
				case XR_Device.Head:
					createHmdRequestAttributes();
					SetHmdInputRequest();
					break;
				case XR_Device.Dominant:
					createDominantRequestAttributes();
					SetDominantInputRequest();
					break;
				case XR_Device.NonDominant:
					createNonDominantRequestAttributes();
					SetNonDominantInputRequest();
					break;
				default:
					break;
			}
		}

		public void ResetAllInputRequest()
		{
			DEBUG("ResetAllInputRequest()");
			ResetInputRequest(XR_Device.Head);
			ResetInputRequest(XR_Device.Dominant);
			ResetInputRequest(XR_Device.NonDominant);
		}

		#region Button State
		public enum ButtonState
		{
			Press,
			Hold,
			Release,
		}
		private bool[,] pressButtons = new bool[WaveEditor.DeviceCount, (int)WVR_InputId.WVR_InputId_Max];
		private bool[,] prePressButtons = new bool[WaveEditor.DeviceCount, (int)WVR_InputId.WVR_InputId_Max];
		private bool[,] touchButtons = new bool[WaveEditor.DeviceCount, (int)WVR_InputId.WVR_InputId_Max];
		private bool[,] preTouchButtons = new bool[WaveEditor.DeviceCount, (int)WVR_InputId.WVR_InputId_Max];
		public void SetPressButton(WVR_DeviceType device, WVR_InputId id, bool state)
		{
			pressButtons[(int)device, (int)id] = state;
		}
		public void SetTouchButton(WVR_DeviceType device, WVR_InputId id, bool state)
		{
			touchButtons[(int)device, (int)id] = state;
		}

		private void ResetButtonState()
		{
			for (int device_index = 0; device_index < WaveEditor.DeviceCount; device_index++)
			{
				for (int button_index = 0; button_index < (int)WVR_InputId.WVR_InputId_Max; button_index++)
				{
					pressButtons[device_index, button_index] = false;
					prePressButtons[device_index, button_index] = false;
					touchButtons[device_index, button_index] = false;
					preTouchButtons[device_index, button_index] = false;
				}
			}
		}
		private void SaveCurrentButtonStates()
		{
			for (int device_index = 0; device_index < WaveEditor.DeviceCount; device_index++)
			{
				for (int button_index = 0; button_index < (int)WVR_InputId.WVR_InputId_Max; button_index++)
				{
					prePressButtons[device_index, button_index] = pressButtons[device_index, button_index];
					preTouchButtons[device_index, button_index] = touchButtons[device_index, button_index];
				}
			}
		}
		public bool ButtonPress(XR_Device device, WVR_InputId id)
		{
			return (!prePressButtons[(int)WvrDevice(device), (int)id] && pressButtons[(int)WvrDevice(device), (int)id]);
		}
		public bool ButtonHold(XR_Device device, WVR_InputId id)
		{
			return (prePressButtons[(int)WvrDevice(device), (int)id] && pressButtons[(int)WvrDevice(device), (int)id]);
		}
		public bool ButtonRelease(XR_Device device, WVR_InputId id)
		{
			return (prePressButtons[(int)WvrDevice(device), (int)id] && !pressButtons[(int)WvrDevice(device), (int)id]);
		}
		public bool ButtonTouch(XR_Device device, WVR_InputId id)
		{
			return (!preTouchButtons[(int)WvrDevice(device), (int)id] && touchButtons[(int)WvrDevice(device), (int)id]);
		}
		public bool ButtonTouching(XR_Device device, WVR_InputId id)
		{
			return (preTouchButtons[(int)WvrDevice(device), (int)id] && touchButtons[(int)WvrDevice(device), (int)id]);
		}
		public bool ButtonUntouch(XR_Device device, WVR_InputId id)
		{
			return (preTouchButtons[(int)WvrDevice(device), (int)id] && !touchButtons[(int)WvrDevice(device), (int)id]);
		}

		private WVR_Axis_t wvrAxis;
		private Vector2 m_Axis = Vector2.zero;
		public Vector2 ButtonAxis(XR_Device device, WVR_InputId id)
		{
			if (!ButtonTouching(device, id))
				return Vector2.zero;

			wvrAxis = Interop.WVR_GetInputAnalogAxis(WvrDevice(device), id);
			m_Axis.x = wvrAxis.x;
			m_Axis.y = wvrAxis.y;
			return m_Axis;
		}

		/// <summary>
		/// Get the specified button status.
		/// </summary>
		/// <param name="device">The device with specified button.</param>
		/// <param name="id">The button ID.</param>
		/// <param name="type">The button type. Press, touch or analog.</param>
		/// <param name="state">The button state. Press, hold or release.</param>
		/// <returns></returns>
		public static bool GetStatus(XR_Device device, WVR_InputId id, WVR_InputType type, ButtonState state)
		{
			if (Instance == null)
				return false;

			if (type == WVR_InputType.WVR_InputType_Button)
			{
				if (state == ButtonState.Press)
					return Instance.ButtonPress(device, id);
				if (state == ButtonState.Hold)
					return Instance.ButtonHold(device, id);
				if (state == ButtonState.Release)
					return Instance.ButtonRelease(device, id);
			}

			if (type == WVR_InputType.WVR_InputType_Touch || type == WVR_InputType.WVR_InputType_Analog)
			{
				if (state == ButtonState.Press)
					return Instance.ButtonTouch(device, id);
				if (state == ButtonState.Hold)
					return Instance.ButtonTouching(device, id);
				if (state == ButtonState.Release)
					return Instance.ButtonUntouch(device, id);
			}

			return false;
		}

		public static void IsPressed(XRNode node, uint buttonId, out bool pressed)
		{
			pressed = false;

			if (node != XRNode.CenterEye ||
				node != XRNode.RightHand ||
				node != XRNode.LeftHand)
				return;

			XR_Device xdev = XR_Device.Head;
			if (node == XRNode.RightHand)
				xdev = XR_Device.Dominant;
			if (node == XRNode.LeftHand)
				xdev = XR_Device.NonDominant;

			WVR_InputId id = WVR_InputId.WVR_InputId_0;
			switch((XButton)buttonId)
			{
				case XButton.Grip:
				case XButton.GripPressed:
					id = WVR_InputId.WVR_InputId_Alias1_Grip;
					break;
				case XButton.MenuButton:
					id = WVR_InputId.WVR_InputId_Alias1_Menu;
					break;
				case XButton.Primary2DAxisClick:
				case XButton.Primary2DAxisTouch:
					id = WVR_InputId.WVR_InputId_Alias1_Touchpad;
					break;
				case XButton.PrimaryButton:
				case XButton.PrimaryTouch:
					id = WVR_InputId.WVR_InputId_Alias1_A;
					break;
				case XButton.PrimaryAxis2DDown:
				case XButton.SecondaryAxis2DDown:
					id = WVR_InputId.WVR_InputId_Alias1_DPad_Down;
					break;
				case XButton.PrimaryAxis2DLeft:
				case XButton.SecondaryAxis2DLeft:
					id = WVR_InputId.WVR_InputId_Alias1_DPad_Left;
					break;
				case XButton.PrimaryAxis2DRight:
				case XButton.SecondaryAxis2DRight:
					id = WVR_InputId.WVR_InputId_Alias1_DPad_Right;
					break;
				case XButton.PrimaryAxis2DUp:
				case XButton.SecondaryAxis2DUp:
					id = WVR_InputId.WVR_InputId_Alias1_DPad_Up;
					break;
				case XButton.Secondary2DAxisClick:
				case XButton.Secondary2DAxisTouch:
					id = WVR_InputId.WVR_InputId_Alias1_Thumbstick;
					break;
				case XButton.SecondaryButton:
				case XButton.SecondaryTouch:
					id = WVR_InputId.WVR_InputId_Alias1_B;
					break;
				case XButton.Trigger:
				case XButton.TriggerPressed:
					id = WVR_InputId.WVR_InputId_Alias1_Trigger;
					break;
				default:
					break;
			}

			if (id == WVR_InputId.WVR_InputId_0)
				return;

			pressed = GetStatus(xdev, id, WVR_InputType.WVR_InputType_Button, ButtonState.Press);
		}

		public static Vector2 GetAxis(XR_Device device, WVR_InputId id)
		{
			if (Instance != null)
				return Instance.ButtonAxis(device, id);
			return Vector2.zero;
		}
		#endregion
	}
}
#endif
