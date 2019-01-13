using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilsSpeech;
using UtilsBlocker;

namespace Level1 {
		
	public class Controller : Level {

		Sentence[] speech1 = new Sentence[] {
			//blockear
			new Sentence ("Welcome back." + Console.jump + "Now we are learning about the <b>Node panel</b> commands."), 
			new Sentence ("Let's start with opening a <b>Node panel</b> once again.")
			//block abierto en nodo 0
		};

		Sentence[] speech2 = new Sentence[] {
			//block abierto en NodePanel
			new Sentence("Nice." + Console.jump + "As you can see, the panel has three diferent options:"),
			//block abierto en Open shell del NodePanel
			new Sentence("Nice." + Console.jump + "As you can see, the panel has three diferent options:"
				+Console.jump+"\t - Open a <b>Shell</b>"),
			//block abierto en Commands del NodePanel
			new Sentence("Nice." + Console.jump + "As you can see, the panel has three diferent options:"
				+Console.jump+"\t - Open a <b>Shell</b>"
				+Console.jump+"\t - Show fast <b>Shell</b> commands"),
			//block abierto en Color node del NodePanel
			new Sentence("Nice." + Console.jump + "As you can see, the panel has three diferent options:"
				+Console.jump+"\t - Open a <b>Shell</b>"
				+Console.jump+"\t - Show fast <b>Shell</b> commands"
				+Console.jump+"\t - Change <b>Node</b> color"), 
			//block abierto en Color node + Color submenu del NodePanel
			new Sentence ("Now let's change the <b>Node</b> color, hover the <b>Brush button</b> with the mouse to open the color selection panel, then change the node color to witchever you want.")
		};

		Sentence[] speech3 = new Sentence[] {
			//blockear + cambiar color texto al del node
			new Sentence("Perfect."),
			new Sentence("Let's move on to the next option, opening a <b>Shell</b>."),
			new Sentence("Click the <b>Shell button</b> to open a <b>Shell</b>.")
		};

		Sentence[] speech4 = new Sentence[] {
			//blockear + cambiar color texto al del node
			new Sentence("Great. As we said earlier, those <b>Shell</b>s allow you to input commands to modify the behaviour of the <b>Node</b>s. "
				+Console.jump+ "We will learn more about them later. I'll close it for now."),
			new Sentence("Finally, let's check the fast <b>Shell</b> commands."
				+Console.jump+ "They allow you to input commands into the <b>Node</b>, the same way we could do with <b>Shell</b>s, except we have hints and no need of knowing the exact structure of them."
				+ " Once the fast command is complete, the panel will show the translation from the action in the panel into the actual <b>Shell</b> command."),
			new Sentence("Hover the <b>Rocket button</b> with the mouse to open the fast commands panel, then click in <b>ANY</b> label.")
		};

		Sentence[] speech5 = new Sentence[] {
			new Sentence("That's everything for now, see you in the next chapter.") //End game
		};

		protected override void Start2 () {
			blocker.gameObject.SetActive (true);
			speech.gameObject.SetActive (true);
			blocker.Block (true);

			speech.SetSpeech (speech1);
			speech1 [1].callback = MoveFocusToNode;
			allNodes [0].OnClickUp += StartSecondSpeech;

			speech2 [0].callback = MoveToOpenShellButton;
			speech2 [1].callback = MoveToOpenFastCommandsButton;
			speech2 [2].callback = MoveToOpenColorButton;
			speech2 [3].callback = MoveToColorSection;

			speech3 [0].callback = MoveToOpenShellButton;

			speech4 [0].callback = CloseShells;
			speech4 [1].callback = MoveToFastCommandsSection;

			speech5 [0].callback = End;
		}
		void MoveFocusToNode() {
			blocker.SetPosition(cam.WorldToScreenPoint (allNodes[0].transform.position));
			blocker.SetSize(new Vector2(200f, 300f));
			blocker.Block (false);
		}
		void StartSecondSpeech(Node node) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech2);
			Center center = nodePanels [allNodes [0]].center;
			blocker.SetPosition(center.transform.position + new Vector3(95f, 0));
			blocker.SetSize(new Vector2(105, 250));
			allNodes [0].OnClickUp -= StartSecondSpeech;

			center.OnColor += StartThirdSpeech;
			center.OnShell += StartFourthSpeech;
			center.OnCommand += StartFifthSpeech;
		}

		void MoveToOpenShellButton() {
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(78f, 89f));
			blocker.SetSize(new Vector2(70, 70));
		}
		void MoveToOpenColorButton() {
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(78f, -89f));
		}
		void MoveToOpenFastCommandsButton() {
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(115f, 0f));
		}

		void MoveToColorSection() {
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(87f, -107f));
			blocker.SetSize(new Vector2(138, 127));
		}
		void MoveToFastCommandsSection() {
			blocker.SetPosition(nodePanels[allNodes [0]].center.transform.position + new Vector3(210f, 0f));
			blocker.SetSize(new Vector2(257, 160));
		}

		void StartThirdSpeech(Center center) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech3);
			center.OnColor -= StartThirdSpeech;
		}
		void StartFourthSpeech(Center center) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech4);
			center.OnShell -= StartFourthSpeech;
		}
		void StartFifthSpeech(Center center) {
			speech.parent.SetActive (true);
			speech.SetSpeech (speech5);
			center.OnCommand -= StartFifthSpeech;
		}

		void CloseShells() {
			Shell.CloseAll ();
		}

		protected override void End2 () {
			blocker.gameObject.SetActive (false);
			speech.gameObject.SetActive (false);
		}
	}

}