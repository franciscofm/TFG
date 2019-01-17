using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilsSpeech;
using UtilsBlocker;

namespace Level4 {
		
	public class Controller : Level {

		Sentence[] speech1 = new Sentence[] {
			new Sentence ("Let's move into serious business now."),
			new Sentence ("What we want to do is send a Ping between two Nodes not directly connected."),
			new Sentence ("That is, having <b>Node</b>s A, B and C:"),
			new Sentence ("That is, having <b>Node</b>s A, B and C:" +
				Console.jump + "\t- A connected to B"),
			new Sentence ("That is, having <b>Node</b>s A, B and C:" +
				Console.jump + "\t- A connected to B" +
				Console.jump + "\t- B connected to C"),
			new Sentence ("That is, having <b>Node</b>s A, B and C:" +
				Console.jump + "\t- A connected to B" +
				Console.jump + "\t- B connected to C" +
				Console.jump + "\t- Send a Ping from A to C"),
			new Sentence ("Let's start with the connection setup.")
		};

		Sentence[] speech2 = new Sentence[] {
			new Sentence ("What we need now, it changing the @IP of Nodes B & C interfaces."),
			new Sentence ("Use 192.168.1.1 & 192.168.1.2 for Node B and 192.168.2.1. for Node C."),
			new Sentence ("Go for it!")
		};

		Sentence[] speech3 = new Sentence[] {
			new Sentence ("For the last part lets dive a little bit into how Ping works."),
			new Sentence ("When we Ping from a Node there are three main steps:"),
			new Sentence ("When we Ping from a Node there are three main steps:" +
				Console.jump + "\t- Node checks for its own interfaces for a match"),
			new Sentence ("When we Ping from a Node there are three main steps:" +
				Console.jump + "\t- Node checks for its own interfaces for a match" +
				Console.jump + "\t- Node checks for its direct connections for a match"),
			new Sentence ("When we Ping from a Node there are three main steps:" +
				Console.jump + "\t- Node checks for its own interfaces for a match" +
				Console.jump + "\t- Node checks for its direct connections for a match" +
				Console.jump + "\t- Node asks to directly connected Nodes for Routes leading to the destination"),
			
			new Sentence ("What we need to do is create a Route in Node B, so when A Node Pings C Node's interface, B forwards the Ping from A to C."),
			new Sentence ("Doing so is telling B that if he gets a Ping with 192.168.2.1 @IP, it needs to send it through the interface connecting him to Node C."),

			//7
			new Sentence ("Let's check the Route command panel."),
			new Sentence ("Mode lets us add a new Route or remove an existing Route with the same parameters."),
			new Sentence ("When the Node gets the Ping, will check if the route destination matches the Ping destination, in that case it will be forwarded."),
			new Sentence ("Gateway is the interface where the Ping will be forwarded to."),
			new Sentence ("Netmasks &  Net will still be explained later."),
			new Sentence ("Now it's your turn, set the A->C Route in Node B and send a Ping from Node A to Node C to clear the level")
		};
		bool ev1, ev2;
		Node A, B, C;
		protected override void Start2 () {
			speech.gameObject.SetActive (true);
			blocker.gameObject.SetActive (false);

			A = allNodes [0];
			B = allNodes [1];
			C = allNodes [2];

			speech1 [6].callback = WaitForConnections;
			speech2 [2].callback = WaitForIfconfigs;

			speech3 [6].callback = OpenRoutePanel;
			speech3 [7].callback = FocusMode;
			speech3 [8].callback = FocusDestination;
			speech3 [9].callback = FocusGateway;
			speech3 [10].callback = FocusMask;
			speech3 [11].callback = CloseRoutePanel;

			speech.SetSpeech (speech1);

			Node.OnPing += CheckPing;
		}
		void WaitForConnections() {
			ev1 = true;
			foreach (Interface i in allInterfaces)
				i.OnConnect += CheckConnections;
		}
		void CheckConnections(Interface iface) {
			if (A.Interfaces [0].connectedTo == null || A.Interfaces [0].connectedTo.node != B) return;
			if (C.Interfaces [0].connectedTo == null || C.Interfaces [0].connectedTo.node != B) return;

			if ((B.Interfaces [0].connectedTo == null) || (B.Interfaces [1].connectedTo == null)) return;
			bool connA = false, connC = false;
			connA = (B.Interfaces [0].connectedTo.node == A) || (B.Interfaces [1].connectedTo.node == A);
			connC = (B.Interfaces [0].connectedTo.node == C) || (B.Interfaces [1].connectedTo.node == C);
			if (!connA || !connC) return;

			ev1 = false;
			foreach (Interface i in allInterfaces)
				i.OnConnect -= CheckConnections;
			
			speech.gameObject.SetActive (true);
			speech.SetSpeech (speech2);
		}
		void WaitForIfconfigs() {
			ev2 = true;
			Console.OnCommandRead += CheckIfconfigs;
		}
		void CheckIfconfigs(CommandStructure comm) {
			print (comm.command [0]);
			if (comm.command [0] == "ifconfig") {
				bool done = ((B.Interfaces [0].ip.word == "192.168.1.1") && (B.Interfaces [1].ip.word == "192.168.1.2")) ||
							((B.Interfaces [0].ip.word == "192.168.1.2") && (B.Interfaces [1].ip.word == "192.168.1.1"));
				done = done && (A.Interfaces [0].ip.word == "192.168.0.1");
				done = done && (C.Interfaces [0].ip.word == "192.168.2.1");
				if (done) {
					ev2 = false;
					Console.OnCommandRead -= CheckIfconfigs;
					speech.gameObject.SetActive (true);
					speech.SetSpeech (speech3);
				}
			}
		}
		GameObject routePanel;
		void OpenRoutePanel() {
			routePanel = Instantiate (routePrefab, canvasTransform);
			routePanel.GetComponent<Panel.Route> ().node = allNodes [0];
			blocker.gameObject.SetActive (true);
			blocker.Block (false);
			RectTransform rt = routePanel.transform as RectTransform;
			blocker.focusRect = blocker.focusImage.transform as RectTransform;
			blocker.SetSize (rt.sizeDelta);
			blocker.SetAnchoredPos (Vector2.zero);
			rt.anchoredPosition = Vector2.zero;
		}
		void FocusMode() {
			blocker.SetSize (new Vector2 (175, 51));
			blocker.SetAnchoredPos (new Vector2 (-284, 28));
		}
		void FocusDestination() {
			blocker.SetSize (new Vector2 (178, 67));
			blocker.SetAnchoredPos (new Vector2 (-274, -33f));
		}
		void FocusGateway() {
			blocker.SetSize (new Vector2 (178, 67));
			blocker.SetAnchoredPos (new Vector2 (75, -33f));
		}
		void FocusMask() {
			blocker.SetSize (new Vector2 (195, 120));
			blocker.SetAnchoredPos (new Vector2 (-92.7f, -13.28f));
		}
		void CloseRoutePanel() {
			Destroy (routePanel);
			blocker.gameObject.SetActive (false);
		}

		void CheckPing(Node sender, object obj) {
			PingInfo pi = obj as PingInfo;
			if(sender == A && pi.destiny == C)
				End ();
		}

		protected override void End2 () {
			print ("Ended");
			blocker.gameObject.SetActive (false);
			speech.gameObject.SetActive (false);
		}

		protected override void Clear2 () {
			Node.OnPing -= CheckPing;
			if(ev1)
				foreach (Interface i in allInterfaces)
					i.OnConnect -= CheckConnections;
			if(ev2)
				Console.OnCommandRead -= CheckIfconfigs;
		}
	}

}