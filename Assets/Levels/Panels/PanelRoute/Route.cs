using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Panel {

	public class Route : MonoBehaviour {

		[HideInInspector] public Node node;
		Interface[] interfaces;

		public Text headerText;

		public RectTransform shellResponseRect;
		public Text shellResponseText;
		bool shellResponseOut;
		Vector2 shellPosOut, shellPosIn;

		public Dropdown dropdownGateway;
		public Dropdown dropdownMode;
		public Dropdown dropdownNet;

		public InputField inputDestination;
		public InputField inputNetmask;

		IEnumerator Start () { //&& on focus
			//populate dropdown with interfaces
			headerText.text += node.name;

			List<string> options = new List<string>();
			interfaces = node.Interfaces;
			foreach (Interface i in interfaces)
				options.Add (i.Name);
			dropdownGateway.ClearOptions ();
			dropdownGateway.AddOptions (options);

			yield return new WaitForEndOfFrame ();

			shellPosOut = new Vector2(0f, -shellResponseRect.rect.size.y);
			shellPosIn = Vector2.zero;
			shellResponseOut = false;
		}

		public void OnClickCancel() {
			Destroy (gameObject);
		}
		public void OnClickGo() {
			string[] command = new string[] {
				"route",
				dropdownMode.options[dropdownMode.value].text,
				"-"+dropdownNet.options[dropdownNet.value].text,
				inputDestination.text,
				"netmask",
				inputNetmask.text,
				"gw",
				interfaces[dropdownGateway.value].ip.word,
				"dev",
				interfaces[dropdownGateway.value].Name
			};
			CommandStructure result = Console.ReadCommand (command, node);

			string output = "Command: ";
			foreach (string s in command) output += s + " ";
			output += Console.jump;
			if (result.prompt) output += result.value;

			shellResponseText.text = output;

			if(!shellResponseOut) {
				StartCoroutine (Routines.WaitFor (0.2f, delegate {
					shellResponseOut = true;
				}));
				StartCoroutine (Routines.DoWhile (0.2f, delegate(float f) {
					shellResponseRect.anchoredPosition = Vector2.Lerp(shellPosIn, shellPosOut, f);
				}));
			}

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
	}

}