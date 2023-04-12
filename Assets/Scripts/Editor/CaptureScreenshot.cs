#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

static class CaptureScreenshot
{
    [MenuItem("Tools/Screenshot")]
    static void TakeScreenshot()
    {
        long now = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        ScreenCapture.CaptureScreenshot("screenshot" + now + ".png");
    }
}
#endif