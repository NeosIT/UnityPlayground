using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.Translation;

[CanEditMultipleObjects]
[CustomEditor(typeof(AutoRotate))]
public class AutoRotateInspector : InspectorBase
{
	private string explanation = _("The GameObject rotates automatically.");
	private string tip = _("TIP: Use negative value to rotate in the other direction.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		base.OnInspectorGUI();

		EditorGUILayout.HelpBox(tip, MessageType.Info);
	}
}
