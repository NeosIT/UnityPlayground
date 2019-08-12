using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(Push))]
public class PushInspector : InspectorBase
{
	private string explanation = _("The GameObject will move at the push of a button, as if a thruster or an invisible force was pushing it.");
	private string absoluteTip = _("TIP: The GameObject will always move in the direction chosen regardless of its orientation.");
	private string relativeTip = _("TIP: The GameObject will move in the direction chosen relative to its orientation.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		base.OnInspectorGUI();

		if(serializedObject.FindProperty("relativeAxis").boolValue)
		{
			EditorGUILayout.HelpBox(relativeTip, MessageType.Info);
		}
		else
		{
			EditorGUILayout.HelpBox(absoluteTip, MessageType.Info);
		}
	}
}
