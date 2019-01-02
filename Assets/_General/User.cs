using UnityEngine;

using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Manager { 

	public static class User {

		public static float volume_master;
		public static float volume_effects;
		public static float volume_music;
		public static float volume_voice;

		public static bool classic_nodes;
		public static bool classic_ifaces;
		public static bool first_time;

		public static List<int> cleared_levels;

		public enum Lenguage { Spanish, Catalan, English };
		public static Lenguage lenguage;

		public static bool ChangeVolumes(float master, float music, float effects, float voice) {
			volume_master = master;
			volume_music = music;
			volume_effects = effects;
			volume_voice = voice;
			SaveConfiguration ();
			return true;
		}
		public static bool ChangeVolumeMaster(float f) {
			volume_master = f;
			return true;
		}
		public static bool ChangeVolumeEffects(float f) {
			volume_effects = f;
			return true;
		}
		public static bool ChangeVolumeMusic(float f) {
			volume_music = f;
			return true;
		}
		public static bool ChangeVolumeVoice(float f) {
			volume_voice = f;
			return true;
		}
		public static bool ChangeLenguage(Lenguage l) {
			lenguage = l;
			return true;
		}

		public static bool SaveConfiguration() {
			SaveFile saveFile = new SaveFile ();
			saveFile.volume_master = volume_master;
			saveFile.volume_effects = volume_effects;
			saveFile.volume_music = volume_music;
			saveFile.volume_voice = volume_voice;
			saveFile.lenguage = lenguage;
			saveFile.classic_nodes = classic_nodes;
			saveFile.classic_ifaces = classic_ifaces;
			saveFile.cleared_levels = cleared_levels;

			string json = JsonUtility.ToJson (saveFile, true);
			System.IO.File.WriteAllText (DataPath (), json);

			return true;
		}
		public static void EraseConfiguration() {
			if (System.IO.File.Exists (DataPath())) {
				System.IO.File.Delete (DataPath());
				Debug.Log ("Data erased");
			}
		}
		public static bool LoadConfiguration() {
			string path = DataPath ();
			Debug.Log (path);
			if (!System.IO.File.Exists (path)) {
				volume_master = 1f;
				volume_effects = 1f;
				volume_music = 1f;
				volume_voice = 1f;
				classic_nodes = false;
				classic_ifaces = false;
				first_time = true;
				cleared_levels = new List<int> ();
				lenguage = GetUserLanguage();
				SaveConfiguration ();
				return false;
			}

			SaveFile saveFile = JsonUtility.FromJson<SaveFile> (System.IO.File.ReadAllText (path));
			volume_master = saveFile.volume_master;
			volume_effects = saveFile.volume_effects;
			volume_music = saveFile.volume_music;
			volume_voice = saveFile.volume_voice;
			lenguage = saveFile.lenguage;
			classic_ifaces = saveFile.classic_ifaces;
			classic_nodes = saveFile.classic_nodes;
			cleared_levels = saveFile.cleared_levels;
			first_time = false;
			return true;
		}

		static string DataPath() {
			return Path.Combine (Application.persistentDataPath, "SaveState.json");
		}
		static Lenguage GetUserLanguage() {
			Lenguage languageString;
			SystemLanguage language = Application.systemLanguage;
			switch (language) {
			case SystemLanguage.Catalan:    // Spanish
				languageString = Lenguage.Catalan;
				break;
			case SystemLanguage.Spanish:
				languageString = Lenguage.Spanish;
				break;
			case SystemLanguage.Basque:
				languageString = Lenguage.Spanish;
				break;
			default:                        // English
				languageString = Lenguage.English;
				break;
			}
			Debug.Log("Final language: " + languageString);
			return languageString;
		}
	}

	[System.Serializable]
	public class SaveFile {

		public float volume_master;
		public float volume_effects;
		public float volume_music;
		public float volume_voice;

		public bool classic_nodes;
		public bool classic_ifaces;

		public List<int> cleared_levels;

		public User.Lenguage lenguage;
	}

}