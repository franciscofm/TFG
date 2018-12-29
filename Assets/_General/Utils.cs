using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for generic non-native methods.
/// </summary>
public static class Utils {

	/// <summary>
	/// Default all granted permissions file options.
	/// </summary>
	public static bool[] PermissionDefault = new bool[] { true, true, true, true, true, true, true, true, true };

	/// <summary>
	/// Is any value of the array above 'value'?
	/// </summary>
	/// <returns><c>true</c>, if it is, <c>false</c> otherwise.</returns>
	public static bool ArrayAboveInt(this int[] array, int value) {
		foreach (int i in array)
			if (i > value)
				return true;
		return false;
	}
	/// <summary>
	/// Is any value of the array under 'value'?
	/// </summary>
	/// <returns><c>true</c>, if it is, <c>false</c> otherwise.</returns>
	public static bool ArrayUnderInt(this int[] array, int value) {
		foreach (int i in array)
			if (i < value)
				return true;
		return false;
	}
	/// <summary>
	/// Are all the values of the array between 'min' and 'max'?
	/// </summary>
	/// <returns><c>true</c>, if it is, <c>false</c> otherwise.</returns>
	public static bool ArrayBetweenInt(this int[] array, int min, int max) {
		foreach (int i in array)
			if (i < min || i > max)
				return false;
		return true;
	}

	/// <summary>
	/// Is any value of the array above 'value'?
	/// </summary>
	/// <returns><c>true</c>, if it is, <c>false</c> otherwise.</returns>
	public static bool ArrayAboveUInt(this uint[] array, int value) {
		foreach (uint i in array)
			if (i > value)
				return true;
		return false;
	}
	/// <summary>
	/// Is any value of the array under 'value'?
	/// </summary>
	/// <returns><c>true</c>, if it is, <c>false</c> otherwise.</returns>
	public static bool ArrayUnderUInt(this uint[] array, int value) {
		foreach (uint i in array)
			if (i < value)
				return true;
		return false;
	}

	/// <summary>
	/// Returns a copy starting at 'index' of length 'length'.
	/// </summary>
	public static T[] SubArray<T>(this T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy (data, index, result, 0, length);
		return result;
	}
	/// <summary>
	/// Does the array have the element 'obj'?. (Uses object.Equals(object o) overridable method.)
	/// </summary>
	public static bool Has<T>(this T[] data, T obj) {
		foreach (T t in data)
			if (t.Equals(obj))
				return true;
		return false;
	}

	/// <summary>
	/// Converts a X.X.X.X format string into an int array.
	/// </summary>
	public static int[] IPToInt4(this string ip) {
		return ip.Split (new string[]{ "." }, 0x0).StringToInt4 ();
	}
	/// <summary>
	/// Converts an string array into an int array.
	/// </summary>
	public static int[] StringToInt4(this string[] array) {
		int[] ret = new int[array.Length];
		for (int i = 0; i < array.Length; ++i) {
			if (!int.TryParse (array [i], out ret [i]))
				return null;
		}
		return ret;
	}
	/// <summary>
	/// Converts an string array into an uint array.
	/// </summary>
	public static uint[] StringToUInt(this string[] array) {
		uint[] ret = new uint[array.Length];
		for (int i = 0; i < array.Length; ++i) {
			if (!UInt32.TryParse (array [i], out ret [i]))
				return null;
		}
		return ret;
	}

	/// <summary>
	/// Returns the content of the array as a formated one line string.
	/// </summary>
	public static string PrintArray<T>(this T[] array) {
		string log = "";
		foreach (T t in array)
			log += t + ", ";
		return log;
	}
	/// <summary>
	/// Returns the content of an int array as a formated one line string.
	/// </summary>
	public static string PrintIp(this int[] array) {
		if (array.Length < 1)
			return "";
		string ret = array [0] + "";
		for (int i = 1; i < array.Length; ++i)
			ret += "." + array [i];
		return ret;
	}
}

/// <summary>
/// Helper class with common routine templates.
/// </summary>
public static class Routines {
	/// <summary>
	/// Delays an action execution.
	/// </summary>
	/// <returns>Routine iterator.</returns>
	/// <param name="f">Delay in seconds.</param>
	/// <param name="a">Action callback, can NOT be null.</param>
	public static IEnumerator WaitFor(float f, Action a) {
		yield return new WaitForSeconds (f);
		a ();
	}
	public delegate void FloatMethod(float f);
	/// <summary>
	/// Executes an action once per frame, method called has the time passed as parameter in % (Range 0f to 1f).
	/// </summary>
	/// <returns>Routine iterator.</returns>
	/// <param name="f">Duration in seconds.</param>
	/// <param name="a">Action callback, can NOT be null.</param>
	public static IEnumerator DoWhile(float f, FloatMethod a) {
		float t = 0f;
		while (t < f) {
			yield return null;
			t += Time.deltaTime;
			a (t/f);
		}
	}
}

/// <summary>
/// Helper class with common event templates.
/// </summary>
public static class Events {
	public delegate void Void();
	public delegate void Obj(object o);
}

public static class Lines {

	public static Pair RenderStraightLine(Transform start, Vector3 end, float width, float offsets = 0f) {
		GameObject line = GameObject.Instantiate (new GameObject (), start);
		line.transform.localPosition = Vector3.zero;
		LineRenderer render = line.AddComponent<LineRenderer> ();
		render.positionCount = 2;

		if (offsets != 0f) {
			Vector3 direction = end - start.position;
			direction.Normalize ();
			render.SetPosition (0, start.position + direction * offsets);
			render.SetPosition (1, end - direction * offsets);
			render.widthMultiplier = width;
		} else {
			render.SetPosition (0, start.position);
			render.SetPosition (1, end);
			render.widthMultiplier = width;
		}

		Pair pair = new Pair ();
		pair.gameObject = line;
		pair.lineRenderer = render;
		return pair;
	}

	public static Pair RenderStraightLine(Transform parent, Vector3 start, Vector3 end, float width, Material material) {
		GameObject line = GameObject.Instantiate (new GameObject (), parent);
		LineRenderer render = line.AddComponent<LineRenderer> ();
		line.transform.localPosition = Vector3.zero;
		render.positionCount = 2;
		render.material = material;
		render.useWorldSpace = false;

		render.SetPosition (0, start);
		render.SetPosition (1, end);
		render.widthMultiplier = width;

		Pair pair = new Pair ();
		pair.gameObject = line;
		pair.lineRenderer = render;
		return pair;
	}

	[System.Serializable]
	public class Pair {
		public GameObject gameObject;
		public LineRenderer lineRenderer;
	}
}


[System.Serializable]
public class AnimationInfo {
	public int layer = 0;
	public string state = "";
}