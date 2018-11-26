using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager { 

	/// <summary>
	/// Helper manager to manage scene loading and information.
	/// </summary>
	public static class Scenes {

		public static string activeScene;
		public static Scene activeSceneObject;

		/// <summary>
		/// Prepares the manager events, must be called before the second scene loading.
		/// </summary>
		public static void Initialize() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		/// <summary>
		/// Unloads the current active scene.
		/// </summary>
		/// <returns>Suscribe to AsyncOperation.completed event to know when it ends.</returns>
		public static AsyncOperation UnloadScene() {
			return UnloadScene (activeScene);
		}
		/// <summary>
		/// Unloads the scene matching name to 'scene'.
		/// </summary>
		/// <returns>Suscribe to AsyncOperation.completed event to know when it ends.</returns>
		public static AsyncOperation UnloadScene(string scene) {
			return SceneManager.UnloadSceneAsync (scene);
		}

		/// <summary>
		/// Loads the scene.
		/// </summary>
		/// <returns>Suscribe to AsyncOperation.completed event to know when it ends.</returns>
		public static AsyncOperation LoadScene(string scene) {
			return SceneManager.LoadSceneAsync (scene, LoadSceneMode.Single);
		}

		/// <summary>
		/// Sets the currents active scene if the loaded scene is Single mode, if Additive
		/// last additive is set. Lastly, if there's any action on load assigned, it is called.
		/// </summary>
		/// <param name="scene">Scene is being loaded.</param>
		/// <param name="mode">Mode in which the scene is being loaded.</param>
		static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			//Unity bug when launching from Editor, it is load as additive by default
			if (activeScene == null) { 
				activeScene = scene.name;
				activeSceneObject = scene;
				return;
			}

			if (mode == LoadSceneMode.Single) {
				activeScene = scene.name;
				activeSceneObject = scene;
			} else {
				lastAdditive = scene;
			}
			
			if (actionOnLoad != null) {
				actionOnLoad ();
				actionOnLoad = null;
			}
		}

		static List<Additive> additiveScenes = new List<Additive> ();
		static Scene lastAdditive;
		static Action actionOnLoad = null;
		/// <summary>
		/// Loads the scene additively.
		/// </summary>
		/// <param name="scene">Scene to load.</param>
		/// <param name="clearOnChangeScene">If set to <c>true</c> clear on change scene.</param>
		public static void LoadSceneAdditive(string scene, bool clearOnChangeScene) {
			SceneManager.LoadScene (scene, LoadSceneMode.Additive);
			
			Additive a = new Additive (scene, lastAdditive);
			additiveScenes.Add (a);
		}
		/// <summary>
		/// Loads a scene and merges it with the current scene loaded.
		/// </summary>
		/// <param name="scene">Scene.</param>
		public static void LoadSceneAdditiveMerge(string scene) {
			SceneManager.LoadScene(scene, LoadSceneMode.Additive);
			actionOnLoad = delegate {
				SceneManager.MergeScenes (lastAdditive, activeSceneObject);
			};
		}
		/// <summary>
		/// Unloads the additive scene matching name to 'scene'.
		/// </summary>
		/// <returns>Suscribe to AsyncOperation[n].completed event to know when it ends.</returns>
		/// <param name="scene">Scene to unload.</param>
		/// <param name="all">If set to <c>true</c> unloads all the scenes with the same name.</param>
		public static List<AsyncOperation> UnloadSceneAdditive(string scene, bool all) {
			List<AsyncOperation> list = new List<AsyncOperation> ();
			for (int i = additiveScenes.Count - 1; i >= 0; --i) {
				if (scene == additiveScenes [i].name) {
					if (!all) {
						Additive a = additiveScenes [i];
						additiveScenes.Remove (a);
						list.Add(SceneManager.UnloadSceneAsync (a.scene));
						return list;
					} else {
						Additive a = additiveScenes [i];
						additiveScenes.Remove (a);
						list.Add(SceneManager.UnloadSceneAsync (a.scene));
					}
				}
			}
			return list;
		}

		class Additive {
			public string name;
			public Scene scene;
			public Additive(string name, Scene scene) {
				this.name = name;
				this.scene = scene;
			}
		}

		/// <summary>
		/// Exits the game. Pauses emulation in Unity Editor.
		/// </summary>
		public static void ExitGame() {
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}

}