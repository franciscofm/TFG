using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Panel {

	public class Ifconfig : MonoBehaviour {

		[HideInInspector] public Node node;
		Interface[] interfaces;
		Interface selectedInterface;

		public RectTransform shellResponseRect;
		public Text shellResponseText;

		public Dropdown dropdownInterfaceSelection;

		public InputField inputDirection;
		public InputField inputMask;
		public InputField inputBroadcast;

		public Toggle toggleIsUp;

		void Start () { //&& on focus
			//populate dropdown with interfaces
			List<string> options = new List<string>();
			interfaces = node.Interfaces;
			foreach (Interface i in interfaces)
				options.Add (i.Name);
			dropdownInterfaceSelection.ClearOptions ();
			dropdownInterfaceSelection.AddOptions (options);
		}

		public void OnValueChangedInterfaceSelection(int i) {
			selectedInterface = interfaces [dropdownInterfaceSelection.value];
			inputDirection.text = selectedInterface.ip.word;
			inputMask.text = selectedInterface.netmask.word;
			inputBroadcast.text = selectedInterface.broadcast.word;
		}

		public void OnClickCancel() {
			Destroy (gameObject);
		}
		public void OnClickGo() {
			// ifconfig eth0 192.168.60.1 netmask 255.255.255.0 broadcast 192.169.60.255
			string[] command = new string[] {
				"ifconfig",
				selectedInterface.Name,
				inputDirection.text,
				"netmask",
				inputMask.text,
				"broadcast",
				inputBroadcast.text
			};
			CommandStructure result = Console.ReadCommand (command, node);

			string output = "Resulting command: ";
			foreach (string s in command) output += s + " ";
			output += Console.jump;

			if (result.prompt) output += result.value;
		}

		public void OnEndEditDirection(string str) {

		}
		public void OnEndEditMask(string str) {

		}
		public void OnEndEditBroadcast(string str) {
			print ("Broadcast edited");
		}
	}

}