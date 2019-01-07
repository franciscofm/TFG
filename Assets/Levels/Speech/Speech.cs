using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UtilsSpeech {
	
	public class Speech : MonoBehaviour {

		public GameObject parent;
		public Text dialogText;

		Sentence[] speech;
		int index;

		public void SetSpeech(Sentence[] speech) {
			this.speech = speech;
			index = 0;
			dialogText.text = speech [0].text;
		}

		public void Next() {
			if (speech [index].callback != null)
				speech [index].callback ();
			
			++index;

			if (index >= speech.Length)
				parent.SetActive(false);
			else
				dialogText.text = speech [index].text;
		}

	}

	public class Sentence {
		public string text;
		public Action callback;
		public Sentence(string text) {
			this.text = text;
		}
	}

}