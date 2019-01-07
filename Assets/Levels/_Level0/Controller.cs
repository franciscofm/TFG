using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilsSpeech;
using UtilsBlocker;

namespace Level0 {
		
	public class Controller : Level {

		[Header("Level0")]
		public GameObject focusPrefab;
		public RectTransform shortcutPanelFocusPos;

		Sentence[] speech1 = new Sentence[] {
			new Sentence ("Welcome!!!" + Console.jump + "In this level you will learn the basics of the enviorment."), //mover foco a nodo
			new Sentence ("First of all, look at this, it is the representation of a computer, we also call them <b>Node</b>s."), //mover foco a ifaz
			new Sentence ("This smaller figures are the <b>interface</b>s of the <b>Node</b>s. Basically where you can connect a good old RJ45 LAN (internet cable you know...)."),
			new Sentence ("Two interfaces of different nodes can be connected, select the first <b>interface</b>, while selected (it gets bigger) select another one from the second <b>Node</b>. " +
				"A line will appear when two <b>interface</b>s are connected.") 
		};

		Sentence[] speech2 = new Sentence[] {
			new Sentence("Good job!" + Console.jump + "You can undo a selection following the same process."), //mover foco a nodo
			new Sentence("Now, click in the <b>Node</b> to open a <b>Node panel</b>.")
		};

		Sentence[] speech3 = new Sentence[] {
			new Sentence("In node panels you can, enter <b>Commands</b> to change the behaviour of the <b>Node</b> through pannels, open <b>Shell</b>s and change the <b>Node</b> color."), //mover shortcut
			new Sentence("Now let's look the left side of the screen." + Console.jump + " Here we have the <b>Shortcut panel</b>, multiple options can be found. If by any chance you want to exit the level or game, " +
				"click on the last option, the <b>Level menu</b> will pop up, letting you to do so."),
			new Sentence("That's everything for now, see you in the next chapter.") //End game
		};

		protected override void Start2 () {
			blocker.Block (true);

			speech.SetSpeech (speech1);
			speech1 [0].callback = MoveFocusToNode;
			speech1 [1].callback = MoveFocusToFirstIface;
			allInterfaces [0].OnClick += MoveFocusToSecondIface;
			allInterfaces [1].OnClick += StartSecondSpeech;
			speech2 [0].callback = MoveFocusToNode;
			allNodes [0].OnClickUp += StartThirdSpeech;
			speech3 [0].callback = MoveFocusToShortcutPanel;
			speech3 [2].callback = End;
		}
		void MoveFocusToNode() {
			blocker.SetPosition(cam.WorldToScreenPoint (allNodes[0].transform.position));
			blocker.SetSize(new Vector2(200f, 300f));
			blocker.Block (false);
		}
		void MoveFocusToFirstIface() {
			blocker.SetPosition(cam.WorldToScreenPoint (allIfaceVisuals[0].modelTransform.position));
			blocker.SetSize(new Vector2(85f, 180f));
		}
		void MoveFocusToSecondIface(Interface iface) {
			blocker.SetPosition(cam.WorldToScreenPoint (allIfaceVisuals[1].modelTransform.position));
			allInterfaces [0].OnClick -= MoveFocusToSecondIface;
		}
		void StartSecondSpeech(Interface iface) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech2);
			blocker.Block (true);
			allInterfaces [1].OnClick -= StartSecondSpeech;
		}
		void StartThirdSpeech(Node node) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech3);
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(20f, 5f));
			blocker.SetSize(new Vector2(250f, 275f));
			allNodes [0].OnClickUp -= StartThirdSpeech;
		}
		void MoveFocusToShortcutPanel() {
			blocker.SetPosition(shortcutPanelFocusPos.position, new Vector2(85f, 1080f));
		}

		protected override void End2 () {
			
		}
	}

}