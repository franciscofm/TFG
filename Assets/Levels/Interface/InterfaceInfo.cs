using UnityEngine;
using UnityEngine.UI;

public class InterfaceInfo : MonoBehaviour {

	static Color upColor = new Color (0.066f, 0.33f, 0f, 0.468f);
	static Color downColor = new Color (0f, 0f, 0f, 0.468f);
	public Text text;
	public Image stateImage;

	public void Set(string text, bool state) {
		this.text.text = text;
		stateImage.color = state ? upColor : downColor;
	}
}
