using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Configuration {

	public static float volume_master;
	public static float volume_effects;
	public static float volume_music;
	public static float volume_voice;

	public enum Lenguage { Spanish, Catalan, English };
	public static Lenguage lenguage;

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

	public static string DataPath() {
		return Path.Combine (Application.persistentDataPath, "SaveState");
	}
	public static bool SaveConfiguration() {
		SaveFile saveFile = new SaveFile ();
		saveFile.volume_master = volume_master;
		saveFile.volume_effects = volume_effects;
		saveFile.volume_music = volume_music;
		saveFile.volume_voice = volume_voice;
		saveFile.lenguage = lenguage;

		string json = JsonUtility.ToJson (saveFile, true);
		System.IO.File.WriteAllText (DataPath (), json);

		return true;
	}
	public static bool LoadConfiguration() {
		string path = DataPath ();
		if (!System.IO.File.Exists (path)) {
			volume_master = 1f;
			volume_effects = 1f;
			volume_music = 1f;
			volume_voice = 1f;
			lenguage = Lenguage.English;
			return false;
		}

		SaveFile saveFile = JsonUtility.FromJson<SaveFile> (System.IO.File.ReadAllText (path));
		volume_master = saveFile.volume_master;
		volume_effects = saveFile.volume_effects;
		volume_music = saveFile.volume_music;
		volume_voice = saveFile.volume_voice;
		lenguage = saveFile.lenguage;
		return true;
	}
}

[System.Serializable]
public class SaveFile {

	public float volume_master;
	public float volume_effects;
	public float volume_music;
	public float volume_voice;

	public Configuration.Lenguage lenguage;
}