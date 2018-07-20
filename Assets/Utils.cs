using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

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

	public static Interface HasInterface(this Node node, string interf) {
		foreach (Interface i in node.Interfaces) {
			if (interf == i.Name) {
				return i;
			}
		}
		return null;
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

	public static void PrintArray<T>(this T[] array) {
		string log = "";
		foreach (T t in array)
			log += t + ", ";
	}
	public static string PrintIp(this int[] array) {
		if (array.Length < 1)
			return "";
		string ret = array[0]+"";
		for (int i = 1; i < array.Length; ++i)
			ret += "." + array [i];
		return ret;
	}
}
