using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine;

public class EditorSettings : MonoBehaviour
{
	[MenuItem("Build/Build WEB")]
	public static void MyBuild()
	{
		PlayerSettings.WebGL.memorySize = 8196;
		PlayerSettings.WebGL.threadsSupport = true;
		PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = new[] { "Assets/Scenes/GameScene1.unity" };
		buildPlayerOptions.locationPathName = "F:\\Out";
		buildPlayerOptions.target = BuildTarget.WebGL;
		buildPlayerOptions.options = BuildOptions.AutoRunPlayer;
		

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
