using UnityEngine;

using Manager;
using SelectLevel_v2;

public class Main : MonoBehaviour {

	public CanvasGroup mainCanvas;
	public Transform nodeTransform;
	public Vector3 nodeSelectLevelPos = new Vector3 (-2.5f, 0);

	Vector3 nodeStartingPos;

	void Start() {
		nodeStartingPos = nodeTransform.position;
		if (User.level_played)
			SkipMain ();
	}

	void SkipMain() {
		inSelectLevel = true;
		nodeTransform.position = nodeSelectLevelPos;
		mainCanvas.alpha = 0f;
		Scenes.LoadSceneAdditiveMerge ("SelectLevel");
	}

	void Update() {
		if (inSelectLevel)
			if (Input.GetKeyDown (KeyCode.Escape))
				ReturnMain ();
	}

	void ReturnMain() {
		if (!inSelectLevel) return;

		SelectLevel.instance.Fade (true, delegate {
			StartCoroutine (Routines.DoWhile (0.3f, delegate(float f) {
				mainCanvas.alpha = f;
			}));
			inSelectLevel = false;
		});
		StartCoroutine (Routines.DoWhile (1f, delegate(float f) {
			nodeTransform.position = Vector3.Lerp(nodeSelectLevelPos, nodeStartingPos, f);
		}));
	}

	bool inSelectLevel = false;
	public void CallbackSelectLevel() {
		inSelectLevel = true;
		StartCoroutine (Routines.DoWhile (0.3f, delegate(float f) {
			mainCanvas.alpha = 1f - f;
		}));
		StartCoroutine (Routines.DoWhile (1f, delegate(float f) {
			nodeTransform.position = Vector3.Lerp(nodeStartingPos, nodeSelectLevelPos, f);
		}));

		if (SelectLevel.instance != null)
			SelectLevel.instance.Fade (false, null);
		else
			Scenes.LoadSceneAdditiveMerge ("SelectLevel");
	}
	public void CallbackConfiguration() {
		Scenes.LoadSceneAdditiveMerge ("Configuration");
	}
	public void CallbackExit() {
		Scenes.ExitGame ();
	}
}
