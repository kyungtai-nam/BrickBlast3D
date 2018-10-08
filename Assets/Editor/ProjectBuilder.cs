using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

class ProjectBuilder {
    static string[] SCENES = FindEnabledEditorScenes();
    //static string APP_NAME = "ChestRPG";
    static string TARGET_DIR = "Build";

    [MenuItem ("Custom/CI/Build Android")]
    static void PerformAndroidBuild ()
    {		
		string target_filename = PlayerSettings.productName + ".apk";

		PlayerSettings.Android.keystoreName = "Market/user.keystore";
		PlayerSettings.Android.keyaliasName = "retrocell-" + PlayerSettings.productName;
		PlayerSettings.Android.keystorePass = "x";
		PlayerSettings.Android.keyaliasPass = "x";

        GenericBuild(SCENES, target_filename, BuildTarget.Android , BuildOptions.None);
    }

    [MenuItem ("Custom/CI/Build iOS")]
    static void PerformiOSBuild()
    {
        //      BuildOptions opt = BuildOptions.SymlinkLibraries | 
        //          BuildOptions.Development;

        //BuildOptions opt = BuildOptions.AcceptExternalModificationsToPlayer;
        BuildOptions opt = BuildOptions.None;

        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersion = iOSTargetOSVersion.iOS_7_1;
        PlayerSettings.statusBarHidden = true;
      
        char sep = Path.DirectorySeparatorChar;
        string buildDirectory = Path.GetFullPath(".") + sep + TARGET_DIR;
        Directory.CreateDirectory(TARGET_DIR + "/iOS");

        string BUILD_TARGET_PATH = buildDirectory + "/iOS";
        GenericBuild(SCENES, BUILD_TARGET_PATH, BuildTarget.iOS, opt);
    }



    private static string[] FindEnabledEditorScenes() 
    {
        List<string> EditorScenes = new List<string>();
    
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) 
        {
            if (!scene.enabled) 
                continue;
    
            EditorScenes.Add(scene.path);
        }
    
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_filename, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_filename, build_target, build_options);
        if (res.Length > 0) {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }
}