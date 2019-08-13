using UnityEngine;
using UnityEditor;
using System.Collections;
using static UnityEngine.Globalization.Translation;

[CustomEditor(typeof(UIScript))]
public class UIScriptInspector : InspectorBase
{
	private string explanation = _("Use the UI to visualise points and health for the players.");
	private string lifeReminder = _("Don't forget to use the script HealthSystemAttribute on the player(s)!");

	private int nOfPlayers = 0, gameType = 0;
	private string[] readablePlayerEnum = new string[]{_("One player"), _("Two players")};
	private string[] readableGameTypesEnum = new string[]{_("Score"), _("Life"), _("Endless")};

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		nOfPlayers = serializedObject.FindProperty("numberOfPlayers").intValue;
		gameType = serializedObject.FindProperty("gameType").intValue;

		nOfPlayers = EditorGUILayout.Popup(_("Number of players"), nOfPlayers, readablePlayerEnum);

		gameType = EditorGUILayout.Popup(_("Game type"), gameType, readableGameTypesEnum);
		if(gameType == 0) //score game
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("scoreToWin"));
		}

		if(gameType == 1) //life
		{
			EditorGUILayout.HelpBox(lifeReminder, MessageType.Info);
		}

		//write all the properties back
		serializedObject.FindProperty("gameType").intValue = gameType;
		serializedObject.FindProperty("numberOfPlayers").intValue = nOfPlayers;

		if(GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
