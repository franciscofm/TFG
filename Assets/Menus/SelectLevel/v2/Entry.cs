using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SelectLevel_v2 {

	public class Entry : MonoBehaviour, IPointerClickHandler {

		public static SelectLevel controller;

		public Text titleText;
		public Text objectiveText;
		public Text conceptText;
		public Image panelImage;
		public int id;

		public void OnPointerClick(PointerEventData data) {
			controller.CallbackEntry (id);
		}

		public void Set(SelectLevel.LevelInfo levelInfo, int id) {
			titleText.text = levelInfo.name;
			objectiveText.text = levelInfo.objective;
			conceptText.text = "";
			foreach (string s in levelInfo.concepts)
				conceptText.text += s + Console.jump;
			this.id = id;
		}

		public void Mark(Color c) {
			panelImage.color = c;
		}
	}

}