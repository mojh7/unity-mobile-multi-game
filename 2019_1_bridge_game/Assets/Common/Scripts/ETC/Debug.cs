// 사용법
// Ctrl + Shift + B : Player Setting
// Configuration - Scripting Define Symbols
// USE_LOG 추가
// 추가 하지 않을시에는 빌드후 로그가 나오지 않음.
using UnityEngine;


#if !UNITY_EDITOR

public static class DebugForBuild
{
    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

	public static bool developerConsoleVisible{
		get{return UnityEngine.Debug.developerConsoleVisible;}
	}
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message.ToString());
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }
 
    [System.Diagnostics.Conditional("USE_LOG")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new System.Exception();
    }

	[System.Diagnostics.Conditional("USE_LOG")]
	public static void Assert(bool condition, object message){
		UnityEngine.Debug.Assert(condition , message);
	}

		[System.Diagnostics.Conditional("USE_LOG")]
	public static void ClearDeveloperConsole(){
		UnityEngine.Debug.ClearDeveloperConsole();
	}
}
#endif

