using UnityEngine;
using System.Collections;
using UnityEditor;
using static _INTERNAL_.Scripts.Utilities.EditorTranslation;

[CanEditMultipleObjects]
[CustomEditor(typeof(PickUpAndHold))]
public class PickUpAndHoldInspector : InspectorBase
{
	private string explanation = _("The Player can pick up and drop objects by pressing a key.");
	private string warning = _("The Pickup object must be tagged 'Pickup' and have component Rigidbody2D");

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);
		GUILayout.Space(10);
		base.OnInspectorGUI();
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(warning, MessageType.Warning);
	}
}
