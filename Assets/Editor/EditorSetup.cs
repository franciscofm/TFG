using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorSetup {

	[MenuItem("Custom/Load level basic config")]
	public static void LoadLevelSetup() {
		GameObject levelObject = GameObject.FindGameObjectWithTag ("GameController");
		Level levelScript = levelObject.GetComponent<Level> ();
		levelScript.keyboard = levelObject.GetComponent<Keyboard> ();
		levelScript.canvasTransform = levelObject.transform;

		levelScript.allNodes = GameObject.FindObjectsOfType<Node> ();
		levelScript.cam = Camera.main;
		levelScript.speech = GameObject.FindObjectOfType<UtilsSpeech.Speech> ();
		levelScript.blocker = GameObject.FindObjectOfType<UtilsBlocker.Blocker> ();
		levelScript.shortcutPanel = GameObject.FindObjectOfType<ShortcutPanel> ();

		levelScript.shellPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Shell/Shell.prefab", typeof(GameObject)) as GameObject;
		levelScript.interfaceInfoPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Interface/InfoVisuals.prefab", typeof(GameObject)) as GameObject;

		levelScript.pingPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Panels/PanelPing/PanelPing.prefab", typeof(GameObject)) as GameObject;
		levelScript.ifconfigPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Panels/PanelIfconfig/PanelIfconfig.prefab", typeof(GameObject)) as GameObject;
		levelScript.routePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Panels/PanelRoute/PanelRoute.prefab", typeof(GameObject)) as GameObject;
		//levelScript.manualPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Panels/PanelPing/PanelPing.prefab", typeof(GameObject)) as GameObject;

		levelScript.radialMenuPrefab = AssetDatabase.LoadAssetAtPath ("Assets/Levels/Panels/RadialMenu/v2/RadialMenu2.prefab", typeof(GameObject)) as GameObject;
	}

}
