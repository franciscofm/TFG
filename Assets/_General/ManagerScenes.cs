using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager { 

	public static class ManagerScenes {

		public static string activeScene;
		public static Scene activeSceneObject;

		public static void Initialize() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		/// <summary>
		/// Unloads the scene.
		/// </summary>
		/// <returns>Returns AsyncOperation, suscribe to return.completed event to know when it ends.</returns>
		public static AsyncOperation UnloadScene() {
			return UnloadScene (activeScene);
		}
		public static AsyncOperation UnloadScene(string scene) {
			return SceneManager.UnloadSceneAsync (scene);
		}

		/// <summary>
		/// Loads the scene.
		/// </summary>
		/// <returns>Returns AsyncOperation, suscribe to return.completed event to know when it ends.</returns>
		public static AsyncOperation LoadScene(string scene) {
			return SceneManager.LoadSceneAsync (scene, LoadSceneMode.Single);
		}

		static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			//Unity bug when launching from Editor
			//it is load as additive by default
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

		public static void LoadSceneAdditive(string scene, bool clearOnChangeScene) {
			SceneManager.LoadScene (scene, LoadSceneMode.Additive);
			
			Additive a = new Additive (scene, lastAdditive);
			additiveScenes.Add (a);
		}
		static Action actionOnLoad = null;
		public static void LoadSceneAdditiveMerge(string scene) {
			SceneManager.LoadScene(scene, LoadSceneMode.Additive);
			actionOnLoad = delegate {
				SceneManager.MergeScenes (lastAdditive, activeSceneObject);
			};
		}
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
	}

}