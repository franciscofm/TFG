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

	public void CallbackExit() {
		Scenes.LoadScene ("Main");
	}
	public void CallbackSave() {
		User.ChangeVolumes (sliderMaster.value, sliderMusic.value, sliderEffects.value, sliderVoice.value);
		Sound.SetAllVolumes (sliderMaster.value, sliderMusic.value, sliderEffects.value, sliderVoice.value);

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
	}
}
