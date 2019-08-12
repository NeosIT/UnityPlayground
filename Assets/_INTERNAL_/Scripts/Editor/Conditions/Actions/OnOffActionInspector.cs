using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(OnOffAction))]
public class OnOffActionInspector : InspectorBase
{
	private string explanation = _("Use this script to turn an object on or off.");
	private string invisibleTip = _("TIP: The object will be made invisible, but it will still collide with others.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		GUILayout.Space(10);
		base.OnInspectorGUI();

		if(serializedObject.FindProperty("justMakeInvisible").boolValue)
		{
			EditorGUILayout.HelpBox(invisibleTip, MessageType.Info);
		}
	}
}
