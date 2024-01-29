#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelPiecePositioner))]
public class LevelPiecePositionerEditor : Editor
{
    private LevelPiecePositioner me;

    private void Awake()
    {
        me = target as LevelPiecePositioner;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Position all pieces"))
        {
            me.PosePieces();
            EditorUtility.SetDirty(me);
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif