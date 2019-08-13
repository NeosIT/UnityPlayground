﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using static UnityEngine.Globalization.Translation;

[CanEditMultipleObjects]
[CustomEditor(typeof(Wander))]
public class WanderInspector : InspectorBase
{
	private string explanation = _("The GameObject will move around randomly. Use keepNearStartingPoint if you want it to keep near its starting position.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("directionChangeInterval"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("keepNearStartingPoint"));

		GUILayout.Space(5);
		GUILayout.Label(_("Orientation"), EditorStyles.boldLabel);
		bool orientToDirectionTemp = EditorGUILayout.Toggle(_("Orient to direction"), serializedObject.FindProperty("orientToDirection").boolValue);
		if(orientToDirectionTemp)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lookAxis"));
		}
		serializedObject.FindProperty("orientToDirection").boolValue = orientToDirectionTemp;

		serializedObject.ApplyModifiedProperties();
	}
}
