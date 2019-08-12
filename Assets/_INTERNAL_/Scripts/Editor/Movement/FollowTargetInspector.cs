using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(FollowTarget))]
public class FollowTargetInspector : InspectorBase
{
	private string explanation = _("This GameObject will pursue a target constantly.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		GUILayout.Space(5);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));

		//Draw custom inspector
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));

		GUILayout.Space(10);

		SerializedProperty lookAtTargetProperty = serializedObject.FindProperty("lookAtTarget");

		lookAtTargetProperty.boolValue = EditorGUILayout.BeginToggleGroup(_("Look at target"), lookAtTargetProperty.boolValue);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("useSide"));
		EditorGUILayout.EndToggleGroup();

		if (GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
