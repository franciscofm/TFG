using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Panel {

	public class Ping : MonoBehaviour {

		[HideInInspector] public Node node;
		[HideInInspector] public List<Interface> allInterfaces;
		public Text headerText;

		public RectTransform shellResponseRect;
		public Text shellResponseText;
		bool shellResponseOut;
		Vector2 shellPosOut, shellPosIn;

		public Dropdown dropdownSuggestions;
		public InputField inputDirection;

		IEnumerator Start () { //&& on focus
			headerText.text += node.name;

			List<string> options = new List<string>();
			foreach (Interface i in allInterfaces)
				if(!options.Contains(i.ip.word))				
					options.Add (i.ip.word);
			dropdownSuggestions.ClearOptions ();
			dropdownSuggestions.AddOptions (options);

			yield return new WaitForEndOfFrame ();

			shellPosOut = new Vector2(0f, -shellResponseRect.rect.size.y);
			shellPosIn = Vector2.zero;
			shellResponseOut = false;
		}

		public void OnValueChangedInterfaceSelection(int i) {
			inputDirection.text = dropdownSuggestions.options [i].text;
		}

		public void OnClickCancel() {
			Destroy (gameObject);
		}
		public void OnClickGo() {
			//ping 192.168.0.3
			string[] command = new string[] {
				"ping",
				inputDirection.text
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