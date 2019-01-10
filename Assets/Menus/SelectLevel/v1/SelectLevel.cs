using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Manager;

namespace SelectLevel_v1 {

	public class SelectLevel : MonoBehaviour {

		[Header("Levels")]
		public LevelInfo[] levels;

		[Header("Horizontal bar")]
		public GameObject levelPrefab;
		public RectTransform levelsParent;
		public RectTransform maskRect;

		public Color clearedColor = Color.green;

		public AnimationCurve movementCurve;
		public float movementDuration = 0.2f;
		public float fadeInDelay = 1f;
		public float fadeInOffset = 0.1f;
		public float fadeInDuration = 0.2f;

		float levelsY;
		float[] levelsPositions;
		int levelCurrent;
		int levelMax;

		void Awake() {
			LevelEntry.controller = this;
		}
		void Start () {
			DisplayLevels ();
		}
		void DisplayLevels() {
			RectTransform firstEntry = Instantiate (levelPrefab, levelsParent).GetComponent<RectTransform>();
			Canvas.ForceUpdateCanvases ();

			levelCurrent = 0;
			levelsY = levelsParent.anchoredPosition.y;
			levelsPositions = new float[levels.Length];

			SetLevelInfo(0, firstEntry.GetComponent<LevelEntry>());
			float levelWidth = firstEntry.rect.width;
			int window = (int)(maskRect.rect.width / levelWidth);
			levelMax = levels.Length - window;
				
			for (int i = 1; i < levels.Length; ++i) {
				SetLevelInfo (i, Instantiate (levelPrefab, levelsParent).GetComponent<LevelEntry> ());
				levelsPositions [i] = levelWidth * i;
			}

			levelsParent.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, levelWidth * levels.Length);
		}
		void SetLevelInfo(int i, LevelEntry entry) {
			entry.scene = levels[i].scene;
			entry.canvasGroup.alpha = 0f;

			entry.levelText.text = levels [i].name;
			entry.objectiveText.text = levels [i].objective;
			entry.levelImage.sprite = levels [i].image;

			entry.conceptsText.text = "";
			foreach(string s in levels [i].concepts)
				entry.conceptsText.text += s + System.Environment.NewLine;

			if (User.cleared_levels.Contains (entry.scene))
				entry.clearedImage.color = clearedColor;
			
			StartCoroutine (SetLevelInfoRoutine (i, entry));
		}
		IEnumerator SetLevelInfoRoutine(int i, LevelEntry entry) {
			yield return new WaitForSeconds (fadeInDelay + fadeInOffset * i);
			float t = 0f;
			while (t < fadeInDuration) {
				yield return null;
				t += Time.deltaTime;
				entry.canvasGroup.alpha = t / fadeInDuration;
			}
		}

		public void CallbackLeft() {
			levelCurrent = Mathf.Max (0, levelCurrent - 1);
			StartCoroutine (LevelsMovementRoutine ());
		}
		public void CallbackRight() {
			levelCurrent = Mathf.Min (levelMax, levelCurrent + 1);
			StartCoroutine (LevelsMovementRoutine ());
		}
		IEnumerator LevelsMovementRoutine() {
			Vector2 start = levelsParent.anchoredPosition;
			Vector2 end = new Vector2 (-levelsPositions [levelCurrent], levelsY);
			float f = 0;
			while (f < movementDuration) {
				yield return null;
				f += Time.deltaTime;
				levelsParent.anchoredPosition = Vector2.Lerp (start, end, movementCurve.Evaluate(f / movementDuration));
			}
		}

		[Header("Horizontal bar levels")]
		public ImagePanel imagePanel;
		public void CallbackLevelEntry(LevelEntry entry) {
			imagePanel.SetLevel (entry);
		}

		[System.Serializable]
		public struct LevelInfo {
			public string name;
			public Sprite image;
			public string[] concepts;
			public string objective;
			public string scene;
		}
	}

}