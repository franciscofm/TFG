using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Manager;

namespace SelectLevel_v2 {

	public class SelectLevel : MonoBehaviour {

		public static SelectLevel instance;

		[System.Serializable]
		public struct LevelInfo {
			public string name;
			public string scene;

			public string objective;
			public string[] concepts;
		}
		public LevelInfo[] levels;

		public RectTransform entriesParent;
		public RectTransform entriesMask;
		public GameObject pointerObject;
		public GameObject arrowsContainer;
		public GameObject downObject;
		public GameObject upObject;
		public GameObject entryPrefab;

		public Color clearedColor = Color.green;
		public Color normalColor = Color.white;
		public AnimationCurve movementCurve;
		public CanvasGroup selectLevelCanvas;

		RectTransform[] entriesRect;
		RectTransform pointerRect;

		float entryHeight, totalHeight, maskHeight;
		int entriesInView;

		void Awake() {
			Entry.controller = this;
			instance = this;
		}
		void OnDestroy() {
			instance = null;
		}

		void Start() {
			upObject.SetActive (false);
			entriesRect = new RectTransform[levels.Length];

			for (int i = 0; i < levels.Length; ++i) {
				GameObject entryObject = Instantiate (entryPrefab, entriesParent);
				entriesRect [i] = entryObject.transform as RectTransform;
				Entry entrySctipt = entryObject.GetComponent<Entry> ();
				entrySctipt.Set (levels [i], i);
				Color c = User.cleared_levels.Contains (levels [i].scene) ? clearedColor : normalColor;
				entrySctipt.Mark(c);
				CanvasGroup cg = entryObject.AddComponent<CanvasGroup> ();
				cg.alpha = 0f;
				StartCoroutine(FadeRoutine(cg, i));
			}

			Canvas.ForceUpdateCanvases ();

			entryHeight = entriesRect[0].sizeDelta.y;
			totalHeight = entryHeight * levels.Length;
			maskHeight = entriesMask.sizeDelta.y;
			entriesInView = Mathf.FloorToInt(maskHeight / entryHeight);
			topIndex = 0;
			routineEntries = null;
			routinePointer = null;
			if (totalHeight < maskHeight) {
				canUp = canDown = false;
				downObject.SetActive (false);
			} else {
				canUp = false;
				canDown = true;
				downObject.SetActive (true);
			}
			arrowsContainer.SetActive (false);
			StartCoroutine (Routines.WaitFor (1f + 0.2f * entriesInView, delegate {
				arrowsContainer.SetActive(true);
			}));

			lastId = -1;
			pointerRect = pointerObject.transform as RectTransform;
			pointerObject.SetActive (false);
			travelling = false;
		}
		IEnumerator FadeRoutine(CanvasGroup cg, int id) {
			yield return new WaitForSeconds (1f + 0.2f * id);
			float t = 0f;
			while (t < 0.3f) {
				yield return null;
				t += Time.deltaTime;
				cg.alpha = t / 0.3f;
			}
		}

		int lastId;
		bool travelling;
		public void CallbackEntry(int id) {
			if (lastId == id)
				pointerObject.SetActive (!pointerObject.activeSelf);
			else
				pointerObject.SetActive (true);

			lastId = id;
			pointerRect.position = entriesRect [id].position;
		}
		public void CallbackPlay() {
			User.level_played = true;
			Fade (true, delegate {
				Scenes.LoadScene (levels [lastId].scene);
				if(Main.backgroundSound != null) {
					Main.backgroundSound.Stop(0.3f);
				}
			});
		}

		public void Fade(bool Out, System.Action a) {
			if (travelling) return;
			travelling = true;

			if (Out) {
				StartCoroutine (Routines.DoWhile (0.4f, delegate(float f) {
					selectLevelCanvas.alpha = 1f - f;
				}));
			} else {
				StartCoroutine (Routines.DoWhile (0.4f, delegate(float f) {
					selectLevelCanvas.alpha = f;
				}));
			}
			if(a != null)
				StartCoroutine (Routines.WaitFor (0.5f, a));
		}

		bool canUp, canDown;
		int topIndex;
		IEnumerator routineEntries, routinePointer;
		public void CallbackUp() {
			if (!canUp) return;

			--topIndex;
			if (topIndex == 0) {
				canUp = false;
				upObject.SetActive (false);
			}
			if (!canDown) {
				canDown = true;
				downObject.SetActive (true);
			}

			UpdateEntries ();
			UpdatePointer ();
		}
		public void CallbackDown() {
			if (!canDown) return;

			++topIndex;
			if (topIndex == levels.Length - 1) {
				canDown = false;
				downObject.SetActive (false);
			}
			if (!canUp) {
				canUp = true;
				upObject.SetActive (true);
			}

			UpdateEntries();
			UpdatePointer ();
		}

		void UpdateEntries() {
			if (routineEntries != null) StopCoroutine (routineEntries);
			StartCoroutine (routineEntries = UpdateEntriesRoutine ());
		}
		void UpdatePointer() {
			if (routinePointer != null) StopCoroutine (routinePointer);
			if (lastId < topIndex) {
				pointerObject.SetActive (false);
				lastId = -1;
				return;
			}
			if (lastId > topIndex + entriesInView - 1) {
				pointerObject.SetActive (false);
				lastId = -1;
				return;
			}

			StartCoroutine (routinePointer = UpdatePointerRoutine ());
		}
		IEnumerator UpdateEntriesRoutine() {
			Vector2 start = entriesParent.anchoredPosition;
			Vector2 end = new Vector2 (0f, entryHeight * topIndex);
			float t = 0f;
			while (t < 0.2f) {
				yield return null;
				t += Time.deltaTime;
				entriesParent.anchoredPosition = Vector2.Lerp (start, end, t / 0.2f);
			}
			routineEntries = null;
		}
		IEnumerator UpdatePointerRoutine() {
			float t = 0f;
			while (t < 0.2f) {
				yield return null;
				t += Time.deltaTime;
				pointerRect.position = Vector3.Lerp (pointerRect.position, entriesRect [lastId].position, t / 0.2f);
			}
			routinePointer = null;
		}
	}

}