using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.Translation;

[CanEditMultipleObjects]
[CustomEditor(typeof(TimedSelfDestruct))]
public class TimedSelfDestructInspector : InspectorBase
{
	private string explanation = _("This GameObject will self destruct after a set amount of time, useful for bullets so they don't accumulate.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		base.OnInspectorGUI();
	}
}
