#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YG;

[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear save data"))
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
            YandexGame.SaveEditor();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif