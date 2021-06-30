using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using static Wave.XR.Constants;

namespace Wave.XR.Settings
{
    [XRConfigurationData("WaveXRSettings", k_SettingsKey)]
    [System.Serializable]
    public class WaveXRSettings : ScriptableObject
    {
        static WaveXRSettings s_RuntimeInstance = null;

        public enum StereoRenderingPath
        {
            MultiPass,
            SinglePass
        }

        public enum FoveationMode
        {
            Disable,
            Enable,
            Default
        }

        public enum FoveatedRenderingPeripheralQuality
        {
            Low,
            Middle,
            High,
        }

        [SerializeField, Tooltip("In URP, only SinglePass can be used.")]
        public StereoRenderingPath preferedStereoRenderingPath = StereoRenderingPath.SinglePass;

        [SerializeField, Tooltip("Use Double Width texture.  On Android, only MultiPass can have double width texture.")]
        public bool useDoubleWidth = false;

        [SerializeField, Tooltip("Enable RenderMask (Occlusion Mesh).")]
        public bool useRenderMask = true;

        [SerializeField, Tooltip("Enable Adaptive Quality.")]
        public bool useAdaptiveQuality = true;

        [SerializeField, Tooltip("Allow set quality strategy is send quality event. SendQualityEvent = false if quality strategy use default.")]
        public bool AQ_SendQualityEvent = true;

        [SerializeField, Tooltip("Allow set auto foveation quality strategy. AutoFoveation = false if quality strategy disable foveation.")]
        public bool AQ_AutoFoveation = false;

        [SerializeField, Tooltip("Enable Dynamic Resolution based on Adaptive Quality event.")]
        public bool useAQDynamicResolution = false;

        [Tooltip("You can choose one of resolution scale from this list as a default resolution scale by setting the default index.")]
        [SerializeField]
        public int DR_DefaultIndex = 0;

        [Tooltip("The unit used for measuring text size here is dmm (Distance-Independent Millimeter). The method of conversion from Unity text size into dmm can be found in the documentation of the SDK.")]
        [SerializeField]
        [Range(20, 40)]
        public int DR_TextSize = 20;

        [SerializeField]
        public List<float> DR_ResolutionScaleList = new List<float>();

        [SerializeField, Tooltip("Set foveationMode.  Choose default mode will apply device's config.")]
        public FoveationMode foveationMode = FoveationMode.Default;

        [SerializeField, Tooltip("Set LeftClearVisionFOV")]
        [Range(1, 179)]
        public float leftClearVisionFOV = 38;

        [SerializeField, Tooltip("Set RightClearVisionFOV")]
        [Range(1, 179)]
        public float rightClearVisionFOV = 38;

        [SerializeField, Tooltip("Set LeftPeripheralQuality")]
        public FoveatedRenderingPeripheralQuality leftPeripheralQuality = FoveatedRenderingPeripheralQuality.High;

        [SerializeField, Tooltip("Set RightPeripheralQuality")]
        public FoveatedRenderingPeripheralQuality rightPeripheralQuality = FoveatedRenderingPeripheralQuality.High;


        [SerializeField, Tooltip("Enabled to override system's PixelDensity.  Disabled to use system's PixelDensity.")]
        public bool overridePixelDensity = false;

        [SerializeField, Tooltip("PixelDensity is a scale to the real display's width and height.  It is use to set a default eye buffer size.  The default eye buffer size will be calculated by (display w, display h) * pixelDensity.")]
        [Range(0.1f, 2)]
        public float pixelDensity = 1;

        [SerializeField, Tooltip("ResolutionScale is a scale to the default eye buffer size which is decided by PixelDensity.  Default is 1.  You can also use XRSettings.eyeTextureResolutionScale to configure it in the runtime.  The final eye buffer size will be calculated by (eye buffer w, eye buffer h) * resolutionScale.")]
        [Range(0.1f, 1)]
        public float resolutionScale = 1;

        [SerializeField, Tooltip("Debug log flag which native XR plugin should follow.")]
        public uint debugLogFlagForNative = (uint)(DebugLogFlag.BasicMask | DebugLogFlag.LifecycleMask | DebugLogFlag.RenderMask | DebugLogFlag.InputMask);

        [SerializeField, Tooltip("Debug log flag which Unity Player should follow.")]
        public uint debugLogFlagForUnity = (uint)(DebugLogFlag.BasicMask | DebugLogFlag.LifecycleMask | DebugLogFlag.RenderMask | DebugLogFlag.InputMask);

        [SerializeField, Tooltip("Override the LogFlag for native.")]
        public bool overrideLogFlagForNative = false;

        void Awake()
        {
            Debug.Log("WaveXRSettings.Awake()");
#if UNITY_EDITOR
            if (Application.isEditor)
                return;
#endif
            s_RuntimeInstance = this;
        }

        public static WaveXRSettings GetInstance()
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                Object obj = null;
                UnityEditor.EditorBuildSettings.TryGetConfigObject(k_SettingsKey, out obj);
                if (obj == null || !(obj is WaveXRSettings))
                    return null;
                return (WaveXRSettings)obj;
            }
#endif
            if (s_RuntimeInstance == null)
                s_RuntimeInstance = new WaveXRSettings();
            return s_RuntimeInstance;
        }
    }
}
