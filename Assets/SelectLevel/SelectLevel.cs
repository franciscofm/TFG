using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour {

	[Header("Horizontal bar levels")]
	public LevelInfo[] levels;
	public GameObject levelPrefab;
	public RectTransform levelsParent;

	void Start () {
		DisplayLevels ();
	}
	void DisplayLevels() {
		RectTransform firstEntry = Instantiate (levelPrefab, levelsParent).GetComponent<RectTransform>();
		SetLevelInfo(0, firstEntry.GetComponent<LevelEntry>());
			
		for(int i=1; i<levels.Length; ++i)
			SetLevelInfo(i, Instantiate(levelPrefab, levelsParent).GetComponent<LevelEntry>());

		Canvas.ForceUpdateCanvases ();
		levelsParent.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, firstEntry.rect.width * levels.Length);
	}
	void SetLevelInfo(int i, LevelEntry entry) {
		entry.id = i;
		entry.scene = levels[i].scene;

		entry.levelText.text = levels [i].name;
		entry.objectiveText.text = levels [i].objective;
		entry.levelImage.sprite = levels [i].image;

		entry.conceptsText.text = "";
		foreach(string s in levels [i].concepts)
			entry.conceptsText.text = s + System.Environment.NewLine;
	}
	
	public void CallbackReturn() {
		SceneManager.LoadScene("Main");
	}

	[Header("Horizontal bar levels")]
	public ImagePanel imagePanel;
	public void CallbackLevel(LevelEntry entry) {
		imagePanel.SetLevel (entry);
	}

	[System.Serializable]
	public class LevelInfo {
		public string name;
		public Sprite image;
		public string[] concepts;
		public string objective;
		public string scene;
	}
}
