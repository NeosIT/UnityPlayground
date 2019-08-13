using UnityEngine;
using System.Collections;
using UnityEditor;
using static UnityEngine.Globalization.Translation;

[CanEditMultipleObjects]
[CustomEditor(typeof(CreateObjectAction))]
public class CreateObjectActionInspector : InspectorBase
{
	private string explanation = _("Use this script to create a new GameObject from a Prefab in a specific position.");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		GUILayout.Space(10);
		base.OnInspectorGUI();

		ShowPrefabWarning("prefabToCreate");
	}
}
