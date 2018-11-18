using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

	public static bool[] PermissionDefault = new bool[] { true, true, true, true, true, true, true, true, true };

	public static bool ArrayAboveInt(this int[] array, int value) {
		foreach (int i in array)
			if (i > value)
				return true;
		return false;
	}
	public static bool ArrayUnderInt(this int[] array, int value) {
		foreach (int i in array)
			if (i < value)
				return true;
		return false;
	}
	public static bool ArrayBetweenInt(this int[] array, int min, int max) {
		foreach (int i in array)
			if (i < min || i > max)
				return false;
		return true;
	}

	public static bool ArrayAboveUInt(this uint[] array, int value) {
		foreach (uint i in array)
			if (i > value)
				return true;
		return false;
	}
	public static bool ArrayUnderUInt(this uint[] array, int value) {
		foreach (uint i in array)
			if (i < value)
				return true;
		return false;
	}

	public static T[] SubArray<T>(this T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy (data, index, result, 0, length);
		return result;
	}

	public static int[] IPToInt4(this string ip) {
		return ip.Split (new string[]{ "." }, 0x0).StringToInt4 ();
	}
	public static int[] StringToInt4(this string[] array) {
		int[] ret = new int[array.Length];
		for (int i = 0; i < array.Length; ++i) {
			if (!int.TryParse (array [i], out ret [i]))
				return null;
		}
		return ret;
	}
	public static uint[] StringToUInt(this string[] array) {
		uint[] ret = new uint[array.Length];
		for (int i = 0; i < array.Length; ++i) {
			if (!UInt32.TryParse (array [i], out ret [i]))
				return null;
		}
		return ret;
	}

	public static string PrintArray<T>(this T[] array) {
		string log = "";
		foreach (T t in array)
			log += t + ", ";
		return log;
	}
	public static string PrintIp(this int[] array) {
		if (array.Length < 1)
			return "";
		string ret = array [0] + "";
		for (int i = 1; i < array.Length; ++i)
			ret += "." + array [i];
		return ret;
	}
}

public static class Routines {
	public static IEnumerator WaitFor(float f, Action a) {
		yield return new WaitForSeconds (f);
		a ();
	}
	public delegate void FloatMethod(float f);
	public static IEnumerator DoWhile(float f, FloatMethod a) {
		float t = 0f;
		while (t < f) {
			yield return null;
			t -= Time.deltaTime;
			a (t);
		}
	}
}

public static class Events {
	public delegate void Void();
	public delegate void Obj(object o);
}

[System.Serializable]
public class AnimationInfo {
	public int layer = 0;
	public string state = "";
}