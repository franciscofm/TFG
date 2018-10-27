using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelEntry : MonoBehaviour {

	public Text levelText;
	public Image levelImage;
	public Text conceptsText;
	public Text objectiveText;
	[HideInInspector] public int id;
	[HideInInspector] public string scene;

}
