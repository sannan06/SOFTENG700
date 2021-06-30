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
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using Wave.Essence.Render;
using Wave.XR.Settings;
using Wave.XR.Loader;

namespace Wave.Essence
{
	public class WaveXR_RuntimeInitializeOnLoad : MonoBehaviour
	{
		static string TAG = "WaveXRRuntimeOnInitialize";
		static bool isFirstScene = true;

		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeMethodLoad()
		{
			Debug.Log(TAG + ": OnRuntimeMethodLoad");
			if (XRGeneralSettings.Instance.Manager.activeLoader == null || XRGeneralSettings.Instance.Manager.activeLoader.GetType() != typeof(WaveXRLoader))
				return; //Don't create GO and script instance if active loader is not found or is not WaveXRLoader

			GameObject obj = new GameObject(TAG, typeof(WaveXR_RuntimeInitializeOnLoad));
			DontDestroyOnLoad(obj);
			isFirstScene = true;
		}

		private void Awake()
		{
			Debug.Log(TAG + ": Awake");
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneLoadActions();
			Debug.Log(TAG + ": OnSceneLoaded: " + scene.name);
		}

		private void SceneLoadActions()
		{
			if (WaveXRSettings.GetInstance().useAdaptiveQuality && WaveXRSettings.GetInstance().AQ_SendQualityEvent && WaveXRSettings.GetInstance().useAQDynamicResolution)
			{
				if (DynamicResolution.instance == null)
					new GameObject("DynamicResolution", typeof(DynamicResolution));
			}
		}

		private void OnEnable()
		{
			Debug.Log(TAG + ": OnEnable");
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void Start()
		{
			Debug.Log(TAG + ": Start");
			if (isFirstScene) //Manually run SceneLoadActions actions in first scene as hooking OnSceneLoaded to delegate happens after first scene load
			{
				SceneLoadActions();
				isFirstScene = false;
			}
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Debug.Log(TAG + ": OnDisable");
		}
	}
}