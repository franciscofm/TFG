using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilsSpeech;
using UtilsBlocker;

namespace Level3 {
		
	public class Controller : Level {

		Sentence[] speech1 = new Sentence[] {
			new Sentence ("Hello there." + Console.jump + "It is time to get into networking concepts!!"), 
			new Sentence ("The point of networking is managing the <b>Node</b>s communication so they can interact between them."),
			new Sentence ("Each <b>Node</b> has a certain number of <b>interface</b>s, in a common computer we usually have one <b>interface</b>, and said <b>interface</b>s use an interet cable " +
				"to be connected."),
			new Sentence ("In our case, <b>Node</b>s will have up to a total of three <b>interface</b>s."),
			new Sentence ("<b>Interface</b>s have two states: <b>Up</b> and <b>Down</b>."
				+ Console.jump + "When <b>Up</b> there is communication through the <b>interface</b>, there isn't otherwise."), 
			new Sentence ("All <b>interface</b>s have an <b>IP</b> direction and <b>name</b>, that is an identifier for the <b>Node</b>s, so they know what <b>interface</b> they refer to."), 
			new Sentence ("We can see <b>interface</b>s as doors, and each connection cables as roads, following this example, doors would be open when the <b>interface</b>s is <b>Up</b> " +
				"and <b>IP</b>s would be the door's direction, its name remains the same."),
			new Sentence ("Information related to all <b>interface</b>s can be seen while pressing <b>Left ALT</b> key. Each panel shows <b>IP</b> + name of the <b>interface</b> beneath, " +
				"if the panel is painted greenish means the <b>interface</b> is <b>Up</b>. (Go ahead, you can try it now)"),
			new Sentence ("Now that the basic concepts are told, lets see how we can test connectivity. " 
				+ Console.jump + "The basic command to check connectivity is <b>Ping</b>, we can <b>Ping</b> from a <b>Node</b> to its own <b>interface</b>s to check if they are <b>Up</b>."
				+ Console.jump + "When we Ping we need to specify the <b>IP</b> we want to reach. Once you <b>Ping</b> to an <b>Up interface</b> the level will be cleared.")
		};

		protected override void Start2 () {
			speech.gameObject.SetActive (true);
			blocker.gameObject.SetActive (false);

			speech.SetSpeech (speech1);

			Node.OnPing += CheckPing;
		}
		void CheckPing(Node sender, object obj) {
			PingInfo pi = obj as PingInfo;
			if(pi.reached)
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