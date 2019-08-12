using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(AutoMove))]
public class AutoMoveInspector : InspectorBase
{
	private string explanation = _("The GameObject moves automatically in a specific direction.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		base.OnInspectorGUI();
	}
}
