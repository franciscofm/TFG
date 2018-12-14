using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Panel {

	public class Ifconfig : MonoBehaviour {

		[HideInInspector] public Node node;
		Interface[] interfaces;
		Interface selectedInterface;

		public Text headerText;

		public RectTransform shellResponseRect;
		public Text shellResponseText;
		bool shellResponseOut;
		Vector2 shellPosOut, shellPosIn;

		public Dropdown dropdownInterfaceSelection;

		public InputField inputDirection;
		public InputField inputMask;
		public InputField inputBroadcast;

		public Toggle toggleIsUp;

		IEnumerator Start () { //&& on focus
			//populate dropdown with interfaces
			headerText.text += node.name;

			List<string> options = new List<string>();
			interfaces = node.Interfaces;
			foreach (Interface i in interfaces)
				options.Add (i.Name);
			dropdownInterfaceSelection.ClearOptions ();
			dropdownInterfaceSelection.AddOptions (options);
			selectedInterface = interfaces [0];
			LoadValues ();

			yield return new WaitForEndOfFrame ();

			shellPosOut = new Vector2(0f, -shellResponseRect.rect.size.y);
			shellPosIn = Vector2.zero;
			shellResponseOut = false;
		}

		public void OnValueChangedInterfaceSelection(int i) {
			selectedInterface = interfaces [dropdownInterfaceSelection.value];
			LoadValues ();
		}
		void LoadValues() {
			inputDirection.text = selectedInterface.ip.word;
			inputMask.text = selectedInterface.netmask.word;
			inputBroadcast.text = selectedInterface.broadcast.word;

			toggleIsUp.isOn = selectedInterface.IsUp ();
		}

		public void OnClickCancel() {
			Destroy (gameObject);
		}
		public void OnClickGo() {
			// ifconfig eth0 192.168.60.1 netmask 255.255.255.0 broadcast 192.169.60.255
			bool wasUp = selectedInterface.IsUp ();

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

			string output = "Command: ";
			foreach (string s in command) output += s + " ";
			output += Console.jump;
			if (result.prompt) output += result.value;

			if (result.correct && wasUp != toggleIsUp.isOn) { //ifup
				command = new string[] {
					"ifconfig",
					selectedInterface.Name,
					(toggleIsUp ? "up" : "down")
				};
				result = Console.ReadCommand (command, node);
				foreach (string s in command) output += s + " ";
			}

			shellResponseText.text = output;

			if(!shellResponseOut) {
				StartCoroutine (Routines.WaitFor (0.2f, delegate {
					shellResponseOut = true;
				}));
				StartCoroutine (Routines.DoWhile (0.2f, delegate(float f) {
					shellResponseRect.anchoredPosition = Vector2.Lerp(shellPosIn, shellPosOut, f);
				}));
			}

			LoadValues ();
		}
		public void OnClickCloseShell() {
			if(shellResponseOut) {
				StartCoroutine (Routines.WaitFor (0.2f, delegate {
					shellResponseOut = false;
				}));
				StartCoroutine (Routines.DoWhile (0.2f, delegate(float f) {
					shellResponseRect.anchoredPosition = Vector2.Lerp(shellPosOut, shellPosIn, f);
				}));
			}
		}

		public void OnEndEditDirection(string str) {
			//Modificar broadcast
		}
		public void OnEndEditMask(string str) {
			//Modificar broadcast
		}
		public void OnEndEditBroadcast(string str) {
			print ("Broadcast edited");
		}
	}

}