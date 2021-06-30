// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wave.Essence
{
    internal class EssenceSettingsProvider : SettingsProvider
    {
        public EssenceSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            bool m_FeatureControllerImported = Directory.Exists("Assets/Wave/Essence/Controller");
            bool m_FeatureInputModuleImported = Directory.Exists("Assets/Wave/Essence/InputModule");
            bool m_FeatureHandImported = Directory.Exists("Assets/Wave/Essence/Hand");
			bool m_FeatureInteractionToolkitImported = Directory.Exists("Assets/Wave/Essence/Interaction/Toolkit");

			/**
             * GUI layout of features.
             * 1. Controller
             * 2. Input Module
             * 3. Hand
			 * 4. Camera Texture
			 * 5. RenderDoc
			 * 6. Interaction Toolkit
             **/
			GUILayout.BeginVertical(EditorStyles.helpBox);
            {
				GUILayout.Label("Controller", EditorStyles.boldLabel);
				GUILayout.Label("This package provides features of render model, button effect and controller tips. Please refer Demo scene to check how to use it.", new GUIStyle(EditorStyles.label) { wordWrap = true });
				GUILayout.Label("The feature will be imported at Assets/Wave/Essence/Controller.", EditorStyles.label);
				GUILayout.Space(5f);
				GUI.enabled = !m_FeatureControllerImported;
                if (GUILayout.Button("Import Feature - Controller", GUILayout.ExpandWidth(false)))
                    ImportModule("wave_essence_controller.unitypackage");
				GUILayout.Space(5f);
				GUI.enabled = true;
			}
			GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Label("Input Module", EditorStyles.boldLabel);
                GUILayout.Label("The Input Module feature provides a controller input module and a gaze input module. In the demo you will see how to interact with scene objects.", new GUIStyle(EditorStyles.label) { wordWrap = true });
                GUILayout.Label("The feature will be imported at Assets/Wave/Essence/InputModule.", EditorStyles.label);
                GUILayout.Space(5f);
                GUI.enabled = !m_FeatureInputModuleImported;
                if (GUILayout.Button("Import Feature - Input Module", GUILayout.ExpandWidth(false)))
                    ImportModule("wave_essence_inputmodule.unitypackage");
                GUILayout.Space(5f);
                GUI.enabled = true;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Label("Hand", EditorStyles.boldLabel);
                GUILayout.Label("The Hand feature provides information of\n" +
                    "- Positions of hand joints and tips.\n" +
                    "- Pinch information containing the origin, direction and strength.\n" +
                    "- Hand gestures.", new GUIStyle(EditorStyles.label) { wordWrap = true });
                GUILayout.Label("The feature will be imported at Assets/Wave/Essence/Hand.", EditorStyles.label);
                GUILayout.Space(5f);
                GUI.enabled = !m_FeatureHandImported;
                if (GUILayout.Button("Import Feature - Hand", GUILayout.ExpandWidth(false)))
                    ImportModule("wave_essence_hand.unitypackage");
                GUILayout.Space(5f);
                GUI.enabled = true;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.Label("CameraTexture", EditorStyles.boldLabel);
			GUILayout.Label("This feature provides a way to access native camera and pose info.", new GUIStyle(EditorStyles.label) { wordWrap = true });
			GUILayout.Label("The feature will be imported at Assets/Wave/Essence/CameraTexture.", EditorStyles.label);
			GUILayout.Space(5f);
			if (GUILayout.Button("Import Feature - CameraTexture", GUILayout.ExpandWidth(false)))
                ImportModule("wave_essence_cameratexture.unitypackage");
            GUILayout.EndVertical();


            GUILayout.BeginVertical(EditorStyles.helpBox);
            string renderDocLabel =
                "Developer can check out the graphic's detail problem with RenderDoc profiling tool.  " +
                "This tool is integrated within Wave's XR plugin.  " +
                "In this package, provide a basic class and sample.  " +
                "Because RenderDoc will cost performance, you can remove the imported content after your test.";
            GUILayout.Label("RenderDoc", EditorStyles.boldLabel);
            GUILayout.Label(renderDocLabel, new GUIStyle() { wordWrap = true });
            GUILayout.Space(5f);
            GUILayout.Label("The feature will be imported at Assets/Wave/Essence/RenderDoc.", EditorStyles.label);
            if (GUILayout.Button("Import RenderDoc tool", GUILayout.ExpandWidth(false)))
                ImportModule("wave_essence_renderdoc.unitypackage");
            GUILayout.EndVertical();


			GUILayout.BeginVertical(EditorStyles.helpBox);
			{
				GUILayout.Label("Interaction Toolkit", EditorStyles.boldLabel);
				GUILayout.Label("The Wave Extension of Unity XR Interaction Toolkit.", new GUIStyle(EditorStyles.label) { wordWrap = true });
				GUILayout.Label("The feature will be imported at Assets/Wave/Essence/Interaction/Toolkit.", EditorStyles.label);
				GUILayout.Space(5f);
				GUI.enabled = !m_FeatureInteractionToolkitImported;
				if (GUILayout.Button("Import Feature - Interaction Toolkit", GUILayout.ExpandWidth(false)))
					ImportModule("wave_essence_interaction_toolkit.unitypackage");
				GUILayout.Space(5f);
				GUI.enabled = true;
			}
			GUILayout.EndVertical();
		}

		private void ImportModule(string packagePath)
        {
            string target = Path.Combine("Packages/com.htc.upm.wave.essence/UnityPackages~", packagePath);
            Debug.Log("Import: " + target);
            AssetDatabase.ImportPackage(target, true);
        }

        [SettingsProvider]
        static SettingsProvider Create()
        {
            Debug.Log("Create EssenceSettingsProvider");
            return new EssenceSettingsProvider("Project/Wave XR/Essence");
        }
    }
}

