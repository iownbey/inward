using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

class CameraPreview : EditorWindow
{
    Camera camera;
    bool overrideTexture;
    RenderTexture renderTexture;

    [MenuItem("Window/Custom/Preview Camera")]
    static void Init()
    {
        var editorWindow = (EditorWindow)GetWindow<CameraPreview>(typeof(CameraPreview));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.titleContent = new GUIContent("Camera Preview");
        editorWindow.Show();
    }

    void Update()
    {
        if (camera != null)
            GetRender();
    }

    void GetRender()
    {
        EnsureRenderTexture();

        camera.renderingPath = RenderingPath.UsePlayerSettings;
        RenderTexture oldValue = camera.targetTexture;
        camera.targetTexture = renderTexture;
        camera.Render();
        camera.targetTexture = oldValue;
    }

    void EnsureRenderTexture()
    {
        if (overrideTexture) return;
        Vector2 previewSize = Handles.GetMainGameViewSize();
        if (renderTexture == null
            || (int)previewSize.x != renderTexture.width
            || (int)previewSize.y != renderTexture.height)
        {
            Debug.Log("Updating Render Texture for Preview");
            renderTexture = new RenderTexture((int)previewSize.x, (int)previewSize.y, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        }
    }

    public Rect GetPreviewRect(float yOffset)
    {
        return new Rect(0,0+yOffset,position.width,position.height-yOffset);
    }

    void OnGUI()
    {
        float yOffset = 0;
        Camera c = (Camera)EditorGUILayout.ObjectField("Camera", camera, typeof(Camera), true);
        yOffset += GUILayoutUtility.GetLastRect().height;
        if (c != camera)
        {
            camera = c;
            GetRender();
        }
        overrideTexture = EditorGUILayout.BeginToggleGroup("Override RenderTexture", overrideTexture);
        renderTexture = (RenderTexture)EditorGUILayout.ObjectField("RenderTexture", renderTexture, typeof(RenderTexture), false);
        yOffset += GUILayoutUtility.GetLastRect().height;
        EditorGUILayout.EndToggleGroup();
        yOffset += GUILayoutUtility.GetLastRect().height;


        if (renderTexture != null)
            GUI.DrawTexture(GetPreviewRect(yOffset), renderTexture, ScaleMode.ScaleToFit, false, 0);
    }
}
