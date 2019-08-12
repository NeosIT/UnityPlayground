using UnityEngine;
using UnityEditor;
using System.Collections;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CustomEditor(typeof(DialogueSystem))]
public class DialogueSystemInspector : InspectorBase
{
	private string explanation = _("This script is responsible of creating dialogue balloons. Create dialogues by using DialogueBalloonAction in Conditions.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);
	}
}
