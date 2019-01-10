using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Manager;

namespace SelectLevel_v2 {

	public class SelectLevel : MonoBehaviour {


		[System.Serializable]
		public struct LevelInfo {
			public string name;
			public string scene;

			public string objective;
			public string[] concepts;
		}
		public LevelInfo[] levels;
	
		public RectTransform entriesParent;
		public GameObject entryPrefab;

		void Start() {

		}


	}

}