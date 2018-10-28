using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationPanel : MonoBehaviour {

	public Slider sliderMaster;
	public Slider sliderMusic;
	public Slider sliderEffects;
	public Slider sliderVoice;

	public Dropdown dropdownLenguage;

	public void CallbackExit() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
	}
	public void CallbackSave() {
		Configuration.ChangeVolumeMaster (sliderMaster.value);
		Configuration.ChangeVolumeMusic (sliderMusic.value);
		Configuration.ChangeVolumeEffects (sliderEffects.value);
		Configuration.ChangeVolumeVoice (sliderVoice.value);

		switch (dropdownLenguage.value) {
		case 0:
			Configuration.ChangeLenguage(Configuration.Lenguage.English);
			break;
		case 1:
			Configuration.ChangeLenguage(Configuration.Lenguage.Spanish);
			break;
		case 2:
			Configuration.ChangeLenguage(Configuration.Lenguage.Catalan);
			break;
		}
	}
}
