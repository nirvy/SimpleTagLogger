using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (SimpleTagLoggerManager))]
public class SimpleTagLoggerManagerEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		ShowElements ();
		((SimpleTagLoggerManager)target).UpdateEnables ();
		serializedObject.ApplyModifiedProperties();
	}

	private void ShowElements ()
	{
		SerializedProperty enabled = serializedObject.FindProperty ("enabled");
		SerializedProperty tags = serializedObject.FindProperty ("tags");
		SerializedProperty size = tags.FindPropertyRelative ("Array.size");
		EditorGUILayout.PropertyField (size);

		enabled.boolValue = EditorGUILayout.Toggle ("Enable", enabled.boolValue);
		EditorGUILayout.PropertyField (tags);
		if (tags.isExpanded) {
			EditorGUI.BeginDisabledGroup (!enabled.boolValue);
			for (int i=0; i<tags.arraySize; i++) {
				SerializedProperty managedTag = tags.GetArrayElementAtIndex (i);
				EditorGUILayout.BeginHorizontal ();
				SerializedProperty isAcitve = managedTag.FindPropertyRelative ("enabled");
				isAcitve.boolValue = EditorGUILayout.Toggle (isAcitve.boolValue, GUILayout.Width (16));
				SerializedProperty tag = managedTag.FindPropertyRelative ("tag");
				tag.stringValue = EditorGUILayout.TextField (tag.stringValue);
				if ( GUILayout.Button ("X", GUILayout.Width (20)) ) {
					tags.DeleteArrayElementAtIndex (i);
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUI.EndDisabledGroup ();
		}
	}
}
