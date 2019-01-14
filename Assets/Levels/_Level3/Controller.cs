using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilsSpeech;
using UtilsBlocker;

namespace Level3 {
		
	public class Controller : Level {

		Sentence[] speech1 = new Sentence[] {
			new Sentence ("We know how to <b>Ping</b> <b>interface</b>s that belong to the same <b>Node</b> where we send the <b>Ping</b> from."),
			new Sentence ("Now let's <b>Ping</b> between two directly connected <b>Node</b>s."),
			new Sentence ("All we need to do is:"),
			new Sentence ("All we need to do is:" +
				Console.jump + "\t- Connect two <b>interface</b>s of different <b>Node</b>s"),
			new Sentence ("All we need to do is:" +
				Console.jump + "\t- Connect two <b>interface</b>s of different <b>Node</b>s" +
				Console.jump + "\t- Make sure both <b>interface</b>s are <b>Up</b>."),
			new Sentence ("All we need to do is:" +
				Console.jump + "\t- Connect two <b>interface</b>s of different <b>Node</b>s" +
				Console.jump + "\t- Make sure both <b>interface</b>s are <b>Up</b>." +
				Console.jump + "\t- Have a different <b>@IP</b> (<b>IP</b> address) in the <b>interface</b> we are <b>Pinging</b> " +
				Console.jump + "\t  than all the <b>interface</b> <b>@IP</b> where we are <b>Pinging</b> from."),

			new Sentence ("Let's see how we can change <b>interface</b>s <b>@IP</b>."),
			new Sentence ("By default, all <b>interface</b>s have the same <b>@IP</b> in all <b>Node</b>s, they start with 192.168.0.1, and add 1 to it each <b>interface</b> (192.168.0.1, 192.168.0.2, 192.168.0.3, and so on)."),
			new Sentence ("It is a common practice to change the third value to the <b>Node</b>'s number where the <b>interface</b> is from, so a second <b>Node</b> <b>interface</b>s would be 192.168.1.1, 192.168.1.2, 192.168.1.3 ..."),
			new Sentence ("For doing that we can use the <b>ifconfig panel</b>."),
			new Sentence ("Here we can specify which <b>interface</b> we are modifying."),
			new Sentence ("Here what @IP it will have."),
			new Sentence ("And here the <b>interface</b> state."),
			new Sentence ("This over here is the <b>@Mask</b> & <b>@Broadcast</b>, we will leave them for now as they are, we will talk about them later."),

			new Sentence ("<b>Ping</b> between <b>Node</b>s to clear this level.")
		};

		protected override void Start2 () {
			speech.gameObject.SetActive (true);
			blocker.gameObject.SetActive (false);

			speech1 [8].callback = OpenIfconfigPanel;
			speech1 [9].callback = FocusInterface;
			speech1 [10].callback = FocusIP;
			speech1 [11].callback = FocusState;
			speech1 [12].callback = FocusMask;
			speech1 [13].callback = CloseIfconfigPanel;

			speech.SetSpeech (speech1);

			Node.OnPing += CheckPing;
		}
		GameObject ifconfigPanel;
		void OpenIfconfigPanel() {
			ifconfigPanel = Instantiate (ifconfigPrefab, canvasTransform);
			ifconfigPanel.GetComponent<Panel.Ifconfig> ().node = allNodes [0];
			blocker.gameObject.SetActive (true);
			blocker.Block (false);
			RectTransform rt = ifconfigPanel.transform as RectTransform;
			blocker.focusRect = blocker.focusImage.transform as RectTransform;
			blocker.SetSize (rt.sizeDelta);
			rt.anchoredPosition = Vector2.zero;
		}
		void FocusInterface() {
			blocker.SetSize (new Vector2 (291, 45));
			blocker.SetAnchoredPos (new Vector2 (-224, 22.5f));
		}
		void FocusIP() {
			blocker.SetSize (new Vector2 (176, 67));
			blocker.SetAnchoredPos (new Vector2 (-274, -47.3f));
		}
		void FocusState() {
			blocker.SetSize (new Vector2 (92, 39));
			blocker.SetAnchoredPos (new Vector2 (46, 24));
		}
		void FocusMask() {
			blocker.SetSize (new Vector2 (355, 70.5f));
			blocker.SetAnchoredPos (new Vector2 (-8.7f, -46.28f));
		}
		void CloseIfconfigPanel() {
			Destroy (ifconfigPanel);
			blocker.gameObject.SetActive (false);
		}
		void CheckPing(Node sender, object obj) {
			PingInfo pi = obj as PingInfo;
			if(pi.reached && pi.destiny != sender)
				End ();
		}

		protected override void End2 () {
			print ("Ended");
			blocker.gameObject.SetActive (false);
			speech.gameObject.SetActive (false);
		}

		protected override void Clear2 () {
			Node.OnPing -= CheckPing;
		}
	}

}