using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolInspector : Editor {

    private ReorderableList _list;
    private ObjectPool _pool;

    private void OnEnable()
    {
        _pool = (ObjectPool)target;
        _list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("_pools"),
                true, true, true, true);
        _list.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) => {
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("PoolObject"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + 165, rect.y, rect.width - 165, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("Size"), GUIContent.none);
    };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
