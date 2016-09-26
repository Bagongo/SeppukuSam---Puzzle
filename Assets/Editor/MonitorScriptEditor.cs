using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Monitor))]
public class MonitorScriptEditor : Editor {

	public override void OnInspectorGUI()
	{
		Monitor monitor = (Monitor)target;

		monitor.level = EditorGUILayout.IntField("Level", monitor.level);
        EditorGUILayout.LabelField("Difficulty", monitor.levelMan.Diff.ToString());

        if(GUILayout.Button("Update level"))
		{
			monitor.UpdateLevel();
		}

		monitor.turnMan.turnNmr = EditorGUILayout.IntField("Turn", monitor.turnMan.turnNmr);
		monitor.turnMan.turnThreshold = EditorGUILayout.IntField("Treshold",  monitor.turnMan.turnThreshold);

		EditorGUILayout.LabelField("Max npcs x row: ", monitor.spawnMan.maxNpcXRow.ToString());
		EditorGUILayout.LabelField("Npcs row: ", monitor.ParseProbsArray(monitor.spawnMan.npcsRowDimProbs));
		EditorGUILayout.LabelField("Npcs pooling: ", monitor.ParseProbsArray(monitor.spawnMan.npcsPoolingProbs));
		EditorGUILayout.LabelField("Enemies pooling: ", monitor.ParseProbsArray(monitor.spawnMan.enemiesPoolingProbs));			
	}

	
}
