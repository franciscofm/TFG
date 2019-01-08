using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Manager;

public class ConfigurationPanel : MonoBehaviour {

	public Slider sliderMaster;
	public Slider sliderMusic;
	public Slider sliderEffects;
	public Slider sliderVoice;

	public Dropdown dropdownLenguage;

	public Toggle toggleNodes;
	public Toggle toggleIfaces;

	void Start() {
		sliderMaster.value = User.volume_master;
		sliderMusic.value = User.volume_music;
		sliderEffects.value = User.volume_effects;
		sliderVoice.value = User.volume_voice;

		switch (User.lenguage) {
		case User.Lenguage.English:
			dropdownLenguage.value = 0;
			break;
		case User.Lenguage.Spanish:
			dropdownLenguage.value = 1;
			break;
		case User.Lenguage.Catalan:
			dropdownLenguage.value = 2;
			break;
		}

		toggleNodes.isOn = User.classic_nodes;
		toggleIfaces.isOn = User.classic_ifaces;
	}

	public void CallbackExit() {
		Destroy (gameObject);
	}
	public void CallbackSave() {
		switch (dropdownLenguage.value) {
			case 0:
				User.ChangeLenguage(User.Lenguage.English);
				break;
			case 1:
				User.ChangeLenguage(User.Lenguage.Spanish);
				break;
			case 2:
				User.ChangeLenguage(User.Lenguage.Catalan);
				break;
		}
		User.classic_nodes = toggleNodes.isOn;
		User.classic_ifaces = toggleIfaces.isOn;
		User.ChangeVolumes (sliderMaster.value, sliderMusic.value, sliderEffects.value, sliderVoice.value);
		Sound.SetAllVolumes (sliderMaster.value, sliderMusic.value, sliderEffects.value, sliderVoice.value);
	}
}
