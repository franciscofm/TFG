using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configuration {

	public static float volume_master;
	public static float volume_effects;
	public static float volume_music;
	public static float volume_voice;

	public enum Lenguage { Spanish, Catalan, English };
	public static Lenguage lenguage;

	public static bool LoadConfiguration() {

		return true;
	}
	public static bool EraseConfiguration() {

		return true;
	}

	public static bool ChangeLenguage(Lenguage lenguage) {

		return true;
	}
}
