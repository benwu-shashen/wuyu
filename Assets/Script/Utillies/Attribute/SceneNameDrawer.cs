using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

# if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int sceneIndex = -1;
    GUIContent[] sceneNames;

    readonly string[] scenePathSplit = { "/", ".unity" };
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;

        if (sceneIndex == -1)
            GetSceneNameArray(property);

        int oldIndex = sceneIndex;

        sceneIndex = EditorGUI.Popup(position, label, sceneIndex, sceneNames);
        if (oldIndex != sceneIndex)
            property.stringValue = sceneNames[sceneIndex].text;
    }

    private void GetSceneNameArray(SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;
        sceneNames = new GUIContent[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitPath = path.Split(scenePathSplit, System.StringSplitOptions.RemoveEmptyEntries);

            string sceneName = "";

            if (splitPath.Length > 0)
                sceneName = splitPath[splitPath.Length - 1];
            else
                sceneName = "(Deleted Scene)";
            sceneNames[i] = new GUIContent(sceneName);
        }

        if (sceneNames.Length == 0)
            sceneNames = new[] { new GUIContent("Check Your Build Settings") };

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool nameFound = false;

            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }
            if (nameFound == false)
                sceneIndex = 0;
        }
        else
            sceneIndex = 0;

        property.stringValue = sceneNames[sceneIndex].text;

        // ���������Ĵ򿪺͹ر��¼����Ա��ڳ��������仯ʱ���»���
        EventHandler.SceneNameDraw += OnSceneNameDraw;
    }

    private void OnSceneNameDraw()
    {
        // �����������仯ʱ���»���
        SceneView.RepaintAll();
    }

    private void OnDestroy()
    {
        // �Ƴ��������������ڴ�й©
        EventHandler.SceneNameDraw -= OnSceneNameDraw;
    }
}
#endif