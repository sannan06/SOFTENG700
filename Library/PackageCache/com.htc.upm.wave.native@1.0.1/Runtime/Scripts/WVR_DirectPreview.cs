// "WaveVR SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Wave.Native
{
#if UNITY_EDITOR && UNITY_ANDROID
	public class WVR_DirectPreview : Wave.Native.Interop.WVR_Base
	{
		private static string LOG_TAG = "WVR_DirectPreview";
		public static string wifi_ip_tmp;
		public static string wifi_ip_state = "";
		bool isLeftReady = false;
		bool isRightReady = false;
		//bool isTimeUp = false;
		//bool isClientConnected = false;
		RenderTexture rt_L;
		RenderTexture rt_R;
		IntPtr[] rt = new IntPtr[2];
		int mFPS = 60;
		long lastUpdateTime = 0;
		bool enablePreview = false;
		bool saveLog = false;
		bool saveImage = false;
		int connectType = 0;  // USB

		public enum SIM_InitError
		{
			SIM_InitError_None = 0,
			SIM_InitError_WSAStartUp_Failed = 1,
			SIM_InitError_Already_Inited = 2,
			SIM_InitError_Device_Not_Found = 3,
			SIM_InitError_Can_Not_Connect_Server = 4,
			SIM_InitError_IPAddress_Null = 5,
		}

		public enum SIM_ConnectType
		{
			SIM_ConnectType_USB = 0,
			SIM_ConnectType_Wifi = 1,
		}

		public delegate void debugcallback(string z);

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_Render_Image_S")]
		public static extern void WVR_Render_Image_S(bool isRenderImageState, bool isRenderImageStateUpdate);

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_Init_S")]
		public static extern SIM_InitError WVR_Init_S(int a, System.IntPtr ip, bool enablePreview, bool saveLogToFile, bool saveImage);

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_Quit_S")]
		public static extern void WVR_Quit_S();

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetInputButtonState_S")]
		public static extern bool WVR_GetInputButtonState_S(int WVR_DeviceType, int WVR_InputId);
		public override bool GetInputButtonState(WVR_DeviceType type, WVR_InputId id)
		{
			return WVR_GetInputButtonState_S((int)type, (int)id);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetInputTouchState_S")]
		public static extern bool WVR_GetInputTouchState_S(int WVR_DeviceType, int WVR_InputId);
		public override bool GetInputTouchState(WVR_DeviceType type, WVR_InputId id)
		{
			return WVR_GetInputTouchState_S((int)type, (int)id);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetInputAnalogAxis_S")]
		public static extern WVR_Axis_t WVR_GetInputAnalogAxis_S(int WVR_DeviceType, int WVR_InputId);
		public override WVR_Axis_t GetInputAnalogAxis(WVR_DeviceType type, WVR_InputId id)
		{
			return WVR_GetInputAnalogAxis_S((int)type, (int)id);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_IsDeviceConnected_S")]
		public static extern bool WVR_IsDeviceConnected_S(int WVR_DeviceType);
		public override bool IsDeviceConnected(WVR_DeviceType type)
		{
			return WVR_IsDeviceConnected_S((int)type);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetDegreeOfFreedom_S")]
		public static extern int WVR_GetDegreeOfFreedom_S(int WVR_DeviceType);
		public override WVR_NumDoF GetDegreeOfFreedom(WVR_DeviceType type)
		{
			if (WVR_GetDegreeOfFreedom_S((int)type) == 0)
				return WVR_NumDoF.WVR_NumDoF_3DoF;
			return WVR_NumDoF.WVR_NumDoF_6DoF;
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetDeviceBatteryPercentage_S")]
		public static extern float WVR_GetDeviceBatteryPercentage_S(int type);
		public override float GetDeviceBatteryPercentage(WVR_DeviceType type)
		{
			return WVR_GetDeviceBatteryPercentage_S((int)type);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetSyncPose_S")]
		//[return : MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.SysInt , SizeConst = 10)]
		public static extern void WVR_GetSyncPose_S(int WVR_PoseOriginModel, [In, Out] WVR_DevicePosePair_t[] poseArray, int PoseCount);
		public override void GetSyncPose(WVR_PoseOriginModel originModel, [In, Out] WVR_DevicePosePair_t[] poseArray, uint pairArrayCount)
		{
			WVR_GetSyncPose_S((int)originModel, poseArray, (int)pairArrayCount);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetParameters_S")]
		//[return : MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.SysInt , SizeConst = 10)]
		public static extern int WVR_GetParameters_S(int WVR_DeviceType, System.IntPtr pchValue, System.IntPtr retValue, uint unBufferSize);
		public override uint GetParameters(WVR_DeviceType type, IntPtr pchValue, IntPtr retValue, uint unBufferSize)
		{
			return (uint)WVR_GetParameters_S((int)type, pchValue, retValue, unBufferSize);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_PollEventQueue_S")]
		//[return : MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.SysInt , SizeConst = 10)]
		public static extern bool WVR_PollEventQueue_S(ref WVR_Event_t t);
		public override bool PollEventQueue(ref WVR_Event_t e)
		{
			return WVR_PollEventQueue_S(ref e);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetRenderTargetSize_S")]
		public static extern void WVR_GetRenderTargetSize_S(ref uint width, ref uint height);
		public override void GetRenderTargetSize(ref uint width, ref uint height)
		{
			WVR_GetRenderTargetSize_S(ref width, ref height);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetTransformFromEyeToHead_S")]
		public static extern WVR_Matrix4f_t WVR_GetTransformFromEyeToHead_S(WVR_Eye eye, WVR_NumDoF dof);
		public override WVR_Matrix4f_t GetTransformFromEyeToHead(WVR_Eye eye, WVR_NumDoF dof)
		{
			return WVR_GetTransformFromEyeToHead_S(eye, dof);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetClippingPlaneBoundary_S")]
		public static extern void WVR_GetClippingPlaneBoundary_S(WVR_Eye eye, ref float left, ref float right, ref float top, ref float bottom);
		public override void GetClippingPlaneBoundary(WVR_Eye eye, ref float left, ref float right, ref float top, ref float bottom)
		{
			WVR_GetClippingPlaneBoundary_S(eye, ref left, ref right, ref top, ref bottom);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_GetRenderProps_S")]
		public static extern bool WVR_GetRenderProps_S(ref WVR_RenderProps_t props);
		public override bool GetRenderProps(ref WVR_RenderProps_t props)
		{
			return WVR_GetRenderProps_S(ref props);
		}

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_SetPrintCallback")]
		public static extern void WVR_SetPrintCallback_S(debugcallback callback);

		[DllImport("wvr_plugins_directpreview", EntryPoint = "WVR_SetRenderImageHandles")]
		public static extern bool WVR_SetRenderImageHandles(IntPtr[] ttPtr);

		public static void PrintLog(string msg)
		{
			UnityEngine.Debug.Log("WVR_DirectPreview : " + msg);
		}
		// wvr.h
		public override WVR_InitError Init(WVR_AppType eType)
		{
			return WVR_InitError.WVR_InitError_None;
		}

		public override void PostInit()
		{
			UnityEngine.Debug.LogWarning("WaveVR_Render Instance is null, skip PostInit");
		}

		public override void Quit()
		{
			WVR_Quit_S();
		}

		bool leftCall = false;
		bool rightCall = false;

		public static long getCurrentTimeMillis()
		{
			DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
		}

		public static bool dpServerProcessChecker()
		{
			bool flag = false;
			Process[] processlist = Process.GetProcesses();
			foreach (Process theProcess in processlist)
			{
				if (theProcess.ProcessName == "dpServer")
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
	}
#endif
}