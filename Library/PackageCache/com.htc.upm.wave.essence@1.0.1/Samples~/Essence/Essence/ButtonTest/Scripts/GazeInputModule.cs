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
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Wave.Native;
using UnityEngine.XR;
#if UNITY_EDITOR
using Wave.Essence.Editor;
#endif

namespace Wave.Essence.Samples.ButtonTest
{
	public class GazeInputModule : PointerInputModule
	{
		private static string LOG_TAG = "GazeInputModule";
		private void DEBUG(string msg)
		{
			if (Log.EnableDebugLog)
				Log.d(LOG_TAG, msg, true);
		}

		public enum GazeEvent
		{
			Down = 0,
			Click = 1,
			Submit = 2
		}

		private WVR_InputId GetWVRButton(XR_BinaryButton event_button)
		{
			switch (event_button)
			{
				case XR_BinaryButton.TouchpadPress:
				case XR_BinaryButton.TouchpadTouch:
					return WVR_InputId.WVR_InputId_Alias1_Touchpad;
				case XR_BinaryButton.ThumbstickPress:
				case XR_BinaryButton.ThumbstickTouch:
					return WVR_InputId.WVR_InputId_Alias1_Thumbstick;
				case XR_BinaryButton.TriggerPress:
					return WVR_InputId.WVR_InputId_Alias1_Trigger;
				case XR_BinaryButton.MenuPress:
					return WVR_InputId.WVR_InputId_Alias1_Menu;
				case XR_BinaryButton.GripPress:
					return WVR_InputId.WVR_InputId_Alias1_Grip;
				default:
					break;
			}

			return WVR_InputId.WVR_InputId_Alias1_Touchpad;
		}

		#region External variables.
		private bool m_EnableGazeBefore = true;
		[SerializeField]
		private bool m_EnableGaze = true;
		public bool EnableGaze { get { return m_EnableGaze; } set { m_EnableGaze = value; } }

		[SerializeField]
		private GazeEvent m_InputEvent = GazeEvent.Submit;
		public GazeEvent InputEvent { get { return m_InputEvent; } set { m_InputEvent = value; } }

		private bool m_TimerControlDefault = false;
		[SerializeField]
		private bool m_TimerControl = true;
		public bool TimerControl {
			get { return m_TimerControl; }
			set {
				if (Log.gpl.Print)
					DEBUG("Timer control enabled: " + value);
				m_TimerControl = value;
				m_TimerControlDefault = value;
			}
		}

		[SerializeField]
		private float m_TimeToGaze = 2.0f;
		public float TimeToGaze { get { return m_TimeToGaze; } set { m_TimeToGaze = value; } }

		[SerializeField]
		private bool m_ButtonControl = true;
		public bool ButtonControl { get { return m_ButtonControl; } set { m_ButtonControl = value; } }

		[SerializeField]
		private List<XR_Device> m_ButtonControlDevices = new List<XR_Device>();
		public List<XR_Device> ButtonControlDevices { get { return m_ButtonControlDevices; } set { m_ButtonControlDevices = value; } }

		[SerializeField]
		private List<XR_BinaryButton> m_ButtonControlKeys = new List<XR_BinaryButton>();
		public List<XR_BinaryButton> ButtonControlKeys { get { return m_ButtonControlKeys; } set { m_ButtonControlKeys = value; } }

		private List<List<bool>> buttonState = new List<List<bool>>(), preButtonState = new List<List<bool>>();
		#endregion

		private GameObject head = null;
		private Camera eventCamera = null;
		private PhysicsRaycaster physicsRaycaster = null;

		private bool btnPressDown = false;
		private float currUnscaledTime = 0;

		#region PointerInputModule overrides. 
		private bool mEnabled = false;
		private GazePointer pointer = null;
		protected override void OnEnable()
		{
			if (!mEnabled)
			{
				base.OnEnable();

				// 0. Disable the existed StandaloneInputModule.
				StandaloneInputModule sim = eventSystem.GetComponent<StandaloneInputModule>();
				if (sim != null)
					sim.enabled = false;

				// 1. Set up necessary components for Gaze input.
				head = Camera.main.gameObject;
				if (head != null)
				{
					eventCamera = head.GetComponent<Camera>();
					eventCamera.stereoTargetEye = StereoTargetEyeMask.None;
					DEBUG("OnEnable() Found event camera " + (eventCamera != null ? eventCamera.gameObject.name : "null"));
					physicsRaycaster = head.GetComponent<PhysicsRaycaster>();
					if (physicsRaycaster == null)
					{
						physicsRaycaster = head.AddComponent<PhysicsRaycaster>();
						DEBUG("OnEnable() Added PhysicsRaycaster " + (physicsRaycaster != null ? physicsRaycaster.gameObject.name : "null"));
					}
					pointer = head.GetComponentInChildren<GazePointer>();
					if (pointer == null)
					{
						GameObject gameObject = new GameObject("Gaze Pointer");
						gameObject.transform.SetParent(head.transform, false);
						pointer = gameObject.AddComponent<GazePointer>();
						DEBUG("OnEnable() Added pointer " + (pointer != null ? pointer.gameObject.name : "null"));
					}
				}

				// 2. Show the Gaze pointer.
				m_EnableGazeBefore = m_EnableGaze;
				if (m_EnableGaze)
					ActivateMeshDrawer(true);

				// 3. Initialize the button states.
				buttonState.Clear();
				for (int d = 0; d < m_ButtonControlDevices.Count; d++)
				{
					List<bool> dev_list = new List<bool>();
					for (int k = 0; k < m_ButtonControlKeys.Count; k++)
					{
						dev_list.Add(false);
					}
					buttonState.Add(dev_list);
				}

				preButtonState.Clear();
				for (int d = 0; d < m_ButtonControlDevices.Count; d++)
				{
					List<bool> dev_list = new List<bool>();
					for (int k = 0; k < m_ButtonControlKeys.Count; k++)
					{
						dev_list.Add(false);
					}
					preButtonState.Add(dev_list);
				}

				// 4. Record the initial Gaze control method.
				m_TimerControlDefault = m_TimerControl;

				mEnabled = true;
			}
		}

		protected override void OnDisable()
		{
			if (mEnabled)
			{
				DEBUG("OnDisable()");
				base.OnDisable();

				ActivateMeshDrawer(false);
				pointer = null;

				ExitAllObjects();

				mEnabled = false;
			}
		}

		private bool ValidateParameters()
		{
			if (head == null || eventCamera == null)
				return false;

			if (!isFocused)
				return false;

			return true;
		}
		private bool isFocused = false;
		private GameObject prevRaycastedObject = null;
		public override void Process()
		{
			if (isFocused != ApplicationScene.IsFocused)
			{
				isFocused = ApplicationScene.IsFocused;
				// Do not gaze if focus is cpatured by system.
				if (!isFocused)
				{
					DEBUG("Process() focus is captured by system, exit all objects.");
					ExitAllObjects();
				}
				else
				{
					DEBUG("Process() get focus, reset the timer.");
					gazeTime = Time.unscaledTime;
				}
			}

			if (m_EnableGazeBefore != m_EnableGaze)
			{
				m_EnableGazeBefore = m_EnableGaze;
				ActivateMeshDrawer(m_EnableGaze);
			}

			if (m_EnableGaze)
			{
				if (!ValidateParameters())
					return;

				// 1. Timer control or button control.
				GazeControl();

				// 2. Graphic raycast and physics raycast.
				prevRaycastedObject = GetRaycastedObject();
				HandleRaycast();

				// 3. Update the timer & pointer state. Send the Gaze event.
				GazeHandling();
			}
		}
		#endregion

		private void ActivateMeshDrawer(bool active)
		{
			if (pointer == null)
				return;

			MeshRenderer mr = pointer.gameObject.GetComponentInChildren<MeshRenderer>();
			if (mr != null && mr.enabled != active)
			{
				DEBUG("ActivateMeshDrawer() " + (active ? "Enable " : "Disable ") + "the ring mesh.");
				mr.enabled = active;
			}
			else
			{
				if (Log.gpl.Print)
					Log.e(LOG_TAG, "ActivateMeshDrawer() Oooooooooooops! No MeshRenderer of " + pointer.gameObject.name);
			}
		}

		private void GazeControl()
		{
			m_TimerControl = m_TimerControlDefault;
			if (!WXRDevice.IsTracked(XR_Device.Dominant) && !WXRDevice.IsTracked(XR_Device.NonDominant))
				m_TimerControl = true;
		}


		#region Raycast
		private void HandleRaycast()
		{
			ResetPointerEventData();
			GraphicRaycast();
			PhysicsRaycast();

			EnterExitGraphicObject();
			EnterExitPhysicsObject();
		}

		private PointerEventData pointerData = null;
		private Vector3 gazeScreenPos2D = Vector2.zero;
		private RaycastResult firstRaycastResult = new RaycastResult();
		private void ResetPointerEventData()
		{
			if (pointerData == null)
			{
				pointerData = new PointerEventData(eventSystem);
				pointerData.pointerCurrentRaycast = new RaycastResult();
			}

			pointerData.Reset();
			// center of screen
			gazeScreenPos2D.x = 0.5f * Screen.width;
			gazeScreenPos2D.y = 0.5f * Screen.height;
			pointerData.position = gazeScreenPos2D;
			firstRaycastResult.Clear();
			pointerData.pointerCurrentRaycast = firstRaycastResult;
		}

		private GameObject GetRaycastedObject()
		{
			if (pointerData != null)
				return pointerData.pointerCurrentRaycast.gameObject;

			return null;
		}

		private void SendPointerEvent(GameObject target)
		{
			DEBUG("SendPointerEvent() selected " + target.name);
			if (m_InputEvent == GazeEvent.Click)
			{
				ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerClickHandler);
				pointerData.clickTime = currUnscaledTime;
			}
			else if (m_InputEvent == GazeEvent.Down)
			{
				// like "mouse" action, press->release soon, do NOT keep the pointerPressRaycast cause do NOT need to controll "down" object while not gazing.
				pointerData.pressPosition = pointerData.position;
				pointerData.pointerPressRaycast = pointerData.pointerCurrentRaycast;

				var _pointerDownGO = ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerDownHandler);
				ExecuteEvents.ExecuteHierarchy(_pointerDownGO, pointerData, ExecuteEvents.pointerUpHandler);
			}
			else if (m_InputEvent == GazeEvent.Submit)
			{
				ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.submitHandler);
			}
		}

		List<RaycastResult> graphicRaycastResultsGaze = new List<RaycastResult>();
		List<GameObject> graphicRaycastObjectsGaze = new List<GameObject>(), preGraphicRaycastObjectsGaze = new List<GameObject>();
		List<GameObject> graphicRaycastObjectsTmp = new List<GameObject>(), preGraphicRaycastObjectsTmp = new List<GameObject>();
		private void GraphicRaycast()
		{
			Profiler.BeginSample("Find GraphicRaycaster for Gaze.");
			GraphicRaycaster[] _graphic_raycasters = GameObject.FindObjectsOfType<GraphicRaycaster>();
			Profiler.EndSample();

			graphicRaycastObjectsGaze.Clear();

			for (int i = 0; i < _graphic_raycasters.Length; i++)
			{
				if (_graphic_raycasters[i].gameObject != null && _graphic_raycasters[i].gameObject.GetComponent<Canvas>() != null)
					_graphic_raycasters[i].gameObject.GetComponent<Canvas>().worldCamera = eventCamera;
				else
					continue;

				_graphic_raycasters[i].Raycast(pointerData, graphicRaycastResultsGaze);
				if (graphicRaycastResultsGaze.Count == 0)
					continue;

				for (int g = 0; g < graphicRaycastResultsGaze.Count; g++)
					graphicRaycastObjectsGaze.Add(graphicRaycastResultsGaze[g].gameObject);

				firstRaycastResult = FindFirstRaycast(graphicRaycastResultsGaze);
				graphicRaycastResultsGaze.Clear();

				// Found graphic raycasted object!
				if (firstRaycastResult.gameObject != null)
				{
					if (firstRaycastResult.worldPosition == Vector3.zero)
					{
						firstRaycastResult.worldPosition = GetIntersectionPosition(
							firstRaycastResult.module.eventCamera,
							firstRaycastResult
						);
					}

					float new_dist =
						Mathf.Abs(
							firstRaycastResult.worldPosition.z -
							firstRaycastResult.module.eventCamera.transform.position.z);
					float origin_dist =
						Mathf.Abs(
							pointerData.pointerCurrentRaycast.worldPosition.z -
							firstRaycastResult.module.eventCamera.transform.position.z);


					bool _changeCurrentRaycast = false;
					// Raycast to nearest (z-axis) target.
					if (pointerData.pointerCurrentRaycast.gameObject == null)
					{
						_changeCurrentRaycast = true;
					}
					else
					{
						/*
						DEBUG ("GraphicRaycast() "
							+ ", raycasted: " + firstRaycastResult.gameObject.name
							+ ", raycasted position: " + firstRaycastResult.worldPosition
							+ ", distance: " + new_dist
							+ ", sorting order: " + firstRaycastResult.sortingOrder
							+ ", origin target: " +
							(pointerData.pointerCurrentRaycast.gameObject == null ?
								"null" :
								pointerData.pointerCurrentRaycast.gameObject.name)
							+ ", origin position: " + pointerData.pointerCurrentRaycast.worldPosition
							+ ", origin distance: " + origin_dist
							+ ", origin sorting order: " + pointerData.pointerCurrentRaycast.sortingOrder);
						*/
						if (origin_dist > new_dist)
						{
							DEBUG("GraphicRaycast() "
							+ pointerData.pointerCurrentRaycast.gameObject.name
							+ ", position: " + pointerData.pointerCurrentRaycast.worldPosition
							+ ", distance: " + origin_dist
							+ " is farer than "
							+ firstRaycastResult.gameObject.name
							+ ", position: " + firstRaycastResult.worldPosition
							+ ", new distance: " + new_dist);

							_changeCurrentRaycast = true;
						}
						else if (origin_dist == new_dist)
						{
							int _so_origin = pointerData.pointerCurrentRaycast.sortingOrder;
							int _so_result = firstRaycastResult.sortingOrder;

							if (_so_origin < _so_result)
							{
								DEBUG("GraphicRaycast() "
								+ pointerData.pointerCurrentRaycast.gameObject.name
								+ " sorting order: " + _so_origin + " is smaller than "
								+ firstRaycastResult.gameObject.name
								+ " sorting order: " + _so_result);

								_changeCurrentRaycast = true;
							}
						}
					}

					if (_changeCurrentRaycast)
					{
						pointerData.pointerCurrentRaycast = firstRaycastResult;
						pointerData.position = firstRaycastResult.screenPosition;
					}
				}
			}
		}

		private void EnterExitGraphicObject()
		{
			graphicRaycastObjectsTmp = graphicRaycastObjectsGaze;
			preGraphicRaycastObjectsTmp = preGraphicRaycastObjectsGaze;

			if (graphicRaycastObjectsTmp.Count == 0 && preGraphicRaycastObjectsTmp.Count == 0)
				return;

			for (int i = 0; i < graphicRaycastObjectsTmp.Count; i++)
			{
				if (!preGraphicRaycastObjectsTmp.Contains(graphicRaycastObjectsTmp[i]))
				{
					ExecuteEvents.Execute(graphicRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerEnterHandler);
					DEBUG("EnterExitGraphicObject() enter: " + graphicRaycastObjectsTmp[i]);
				}
			}

			for (int i = 0; i < preGraphicRaycastObjectsTmp.Count; i++)
			{
				if (!graphicRaycastObjectsTmp.Contains(preGraphicRaycastObjectsTmp[i]))
				{
					ExecuteEvents.Execute(preGraphicRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerExitHandler);
					DEBUG("EnterExitGraphicObject() exit: " + preGraphicRaycastObjectsTmp[i]);
				}
			}

			preGraphicRaycastObjectsTmp.Clear();
			for (int i = 0; i < graphicRaycastObjectsTmp.Count; i++)
			{
				preGraphicRaycastObjectsTmp.Add(graphicRaycastObjectsTmp[i]);
			}

			graphicRaycastObjectsGaze = graphicRaycastObjectsTmp;
			preGraphicRaycastObjectsGaze = preGraphicRaycastObjectsTmp;
		}

		List<RaycastResult> physicsRaycastResultsGaze = new List<RaycastResult>();
		List<GameObject> physicsRaycastObjectsGaze = new List<GameObject>(), prePhysicsRaycastObjectsGaze = new List<GameObject>();
		List<GameObject> physicsRaycastObjectsTmp = new List<GameObject>(), prePhysicsRaycastObjectsTmp = new List<GameObject>();
		private void PhysicsRaycast()
		{
			physicsRaycastObjectsGaze.Clear();
			physicsRaycastResultsGaze.Clear();

			Profiler.BeginSample("PhysicsRaycaster.Raycast() Gaze.");
			physicsRaycaster.Raycast(pointerData, physicsRaycastResultsGaze);
			Profiler.EndSample();

			for (int i = 0; i < physicsRaycastResultsGaze.Count; i++)
				physicsRaycastObjectsGaze.Add(physicsRaycastResultsGaze[i].gameObject);

			firstRaycastResult = FindFirstRaycast(physicsRaycastResultsGaze);

			/*if (firstRaycastResult.module != null)
			{
				DEBUG ("PhysicsRaycast() device: " + event_controller.device + ", camera: " + firstRaycastResult.module.eventCamera + ", first result = " + firstRaycastResult);
			}*/

			if (firstRaycastResult.gameObject != null)
			{
				if (firstRaycastResult.worldPosition == Vector3.zero)
				{
					firstRaycastResult.worldPosition = GetIntersectionPosition(
						firstRaycastResult.module.eventCamera,
						firstRaycastResult
					);
				}

				float new_dist =
					Mathf.Abs(
						firstRaycastResult.worldPosition.z -
						firstRaycastResult.module.eventCamera.transform.position.z);
				float origin_dist =
					Mathf.Abs(
						pointerData.pointerCurrentRaycast.worldPosition.z -
						firstRaycastResult.module.eventCamera.transform.position.z);

				if (pointerData.pointerCurrentRaycast.gameObject == null || origin_dist > new_dist)
				{
					/*
					DEBUG ("PhysicsRaycast()" +
						", raycasted: " + firstRaycastResult.gameObject.name +
						", raycasted position: " + firstRaycastResult.worldPosition +
						", new_dist: " + new_dist +
						", origin target: " +
						(pointerData.pointerCurrentRaycast.gameObject == null ?
							"null" :
							pointerData.pointerCurrentRaycast.gameObject.name) +
						", origin position: " + pointerData.pointerCurrentRaycast.worldPosition +
						", origin distance: " + origin_dist);
					*/
					pointerData.pointerCurrentRaycast = firstRaycastResult;
					pointerData.position = firstRaycastResult.screenPosition;
				}
			}
		}

		private void EnterExitPhysicsObject()
		{
			physicsRaycastObjectsTmp = physicsRaycastObjectsGaze;
			prePhysicsRaycastObjectsTmp = prePhysicsRaycastObjectsGaze;

			if (physicsRaycastObjectsTmp.Count == 0 && prePhysicsRaycastObjectsTmp.Count == 0)
				return;

			for (int i = 0; i < physicsRaycastObjectsTmp.Count; i++)
			{
				if (!prePhysicsRaycastObjectsTmp.Contains(physicsRaycastObjectsTmp[i]))
				{
					ExecuteEvents.Execute(physicsRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerEnterHandler);
					DEBUG("EnterExitPhysicsObject() enter: " + physicsRaycastObjectsTmp[i]);
				}
			}

			for (int i = 0; i < prePhysicsRaycastObjectsTmp.Count; i++)
			{
				if (!physicsRaycastObjectsTmp.Contains(prePhysicsRaycastObjectsTmp[i]))
				{
					ExecuteEvents.Execute(prePhysicsRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerExitHandler);
					DEBUG("EnterExitPhysicsObject() exit: " + prePhysicsRaycastObjectsTmp[i]);
				}
			}

			prePhysicsRaycastObjectsTmp.Clear();
			for (int i = 0; i < physicsRaycastObjectsTmp.Count; i++)
			{
				prePhysicsRaycastObjectsTmp.Add(physicsRaycastObjectsTmp[i]);
			}

			physicsRaycastObjectsGaze = physicsRaycastObjectsTmp;
			prePhysicsRaycastObjectsGaze = prePhysicsRaycastObjectsTmp;
		}

		private void ExitAllObjects()
		{
			prePhysicsRaycastObjectsTmp = prePhysicsRaycastObjectsGaze;
			preGraphicRaycastObjectsTmp = preGraphicRaycastObjectsGaze;

			if (prePhysicsRaycastObjectsTmp.Count == 0 && preGraphicRaycastObjectsTmp.Count == 0)
				return;

			for (int i = 0; i < prePhysicsRaycastObjectsTmp.Count; i++)
			{
				ExecuteEvents.Execute(prePhysicsRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerExitHandler);
				DEBUG("ExitAllObjects() exit: " + prePhysicsRaycastObjectsTmp[i]);
			}

			prePhysicsRaycastObjectsTmp.Clear();

			for (int i = 0; i < preGraphicRaycastObjectsTmp.Count; i++)
			{
				ExecuteEvents.Execute(preGraphicRaycastObjectsTmp[i], pointerData, ExecuteEvents.pointerExitHandler);
				DEBUG("ExitAllObjects() exit: " + preGraphicRaycastObjectsTmp[i]);
			}

			preGraphicRaycastObjectsTmp.Clear();

			prePhysicsRaycastObjectsGaze = prePhysicsRaycastObjectsTmp;
			preGraphicRaycastObjectsGaze = preGraphicRaycastObjectsTmp;
		}

		/**
		 * @brief get intersection position in world space
		 **/
		private Vector3 GetIntersectionPosition(Camera cam, RaycastResult raycastResult)
		{
			// Check for camera
			if (cam == null)
			{
				return Vector3.zero;
			}

			float intersectionDistance = raycastResult.distance + cam.nearClipPlane;
			Vector3 intersectionPosition = cam.transform.position + cam.transform.forward * intersectionDistance;
			return intersectionPosition;
		}
		#endregion


		private float gazeTime = 0.0f;
		private void GazeHandling()
		{
			// The gameobject to which raycast positions
			var curr_raycasted_obj = GetRaycastedObject();
			bool isInteractive = (curr_raycasted_obj != null);//pointerData.pointerPress != null || ExecuteEvents.GetEventHandler<IPointerClickHandler>(curr_raycasted_obj) != null;

			bool sendEvent = false;

			currUnscaledTime = Time.unscaledTime;
			if (prevRaycastedObject != curr_raycasted_obj)
			{
				DEBUG("prevRaycastedObject: "
					+ (prevRaycastedObject != null ? prevRaycastedObject.name : "null")
					+ ", curr_raycasted_obj: "
					+ (curr_raycasted_obj != null ? curr_raycasted_obj.name : "null"));
				if (curr_raycasted_obj != null)
					gazeTime = currUnscaledTime;
			}
			else
			{
				if (curr_raycasted_obj != null)
				{
					if (m_TimerControl)
					{
						if (currUnscaledTime - gazeTime > m_TimeToGaze)
						{
							sendEvent = true;
							gazeTime = currUnscaledTime;
						}
						float rate = ((currUnscaledTime - gazeTime) / m_TimeToGaze) * 100;
						if (pointer != null)
						{
							pointer.RingPercent = isInteractive ? (int)rate : 0;
						}
					}

					if (m_ButtonControl)
					{
						if (!m_TimerControl)
						{
							if (pointer != null)
								pointer.RingPercent = 0;
						}

						UpdateButtonStates();
						if (btnPressDown)
						{
							sendEvent = true;
							this.gazeTime = currUnscaledTime;
						}
					}
				}
				else
				{
					if (pointer != null)
						pointer.RingPercent = 0;
				}
			}

			// Standalone Input Module information
			pointerData.delta = Vector2.zero;
			pointerData.dragging = false;

			DeselectIfSelectionChanged(curr_raycasted_obj, pointerData);

			if (sendEvent)
			{
				SendPointerEvent(curr_raycasted_obj);
			}
		} // GazeHandling()

		private void UpdateButtonStates()
		{
			btnPressDown = false;

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				for (int d = 0; d < m_ButtonControlDevices.Count; d++)
				{
					for (int k = 0; k < m_ButtonControlKeys.Count; k++)
					{
						btnPressDown |= DummyButton.GetStatus(
							m_ButtonControlDevices[d],
							GetWVRButton(m_ButtonControlKeys[k]),
							WVR_InputType.WVR_InputType_Button,
							DummyButton.ButtonState.Press);
					}
				}
			}
			else
#endif
			{
				for (int d = 0; d < m_ButtonControlDevices.Count; d++)
				{
					for (int k = 0; k < m_ButtonControlKeys.Count; k++)
					{
						preButtonState[d][k] = buttonState[d][k];
						buttonState[d][k] = WXRDevice.KeyDown(m_ButtonControlDevices[d], m_ButtonControlKeys[k]);

						if (!preButtonState[d][k] && buttonState[d][k])
						{
							btnPressDown = true;
							return;
						}
					}
				}
			}
		} // UpdateButtonStates()
	}
}
