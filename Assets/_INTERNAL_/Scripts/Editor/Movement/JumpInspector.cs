using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(Jump))]
public class JumpInspector : InspectorBase
{
	private string explanation = _("Makes the GameObject jump at the press of a button.");
	private bool checkGround;
	private string checkGroundTip = _("Enable ground check to restrict the Player from jumping while in the air.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpStrength"));


		GUILayout.Label(_("Ground setup"), EditorStyles.boldLabel);
		checkGround = EditorGUILayout.Toggle(_("Check ground"), serializedObject.FindProperty("checkGround").boolValue);
		if(checkGround)
		{
			serializedObject.FindProperty("groundTag").stringValue = EditorGUILayout.TagField(_("Ground tag"), serializedObject.FindProperty("groundTag").stringValue);
		}
		else
		{
			EditorGUILayout.HelpBox(checkGroundTip, MessageType.Info);
		}
		serializedObject.FindProperty("checkGround").boolValue = checkGround;

		serializedObject.ApplyModifiedProperties();
	}
}
